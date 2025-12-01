using LAB2_OOP_MAUI.Controllers;
using LAB2_OOP_MAUI.Models;
using LAB2_OOP_MAUI.Strategies;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Maui.Controls;

namespace LAB2_OOP_MAUI;

public partial class NewPage1 : ContentPage
{
    // Збережемо назву секції, яку знайшли, щоб знати куди записуватись
    private string _foundSectionName = null;

    public NewPage1()
    {
        InitializeComponent();
    }

    // === 1. Логін (Просто зберігаємо ім'я) ===
    private void OnLoginClicked(object sender, EventArgs e)
    {
        string login = LoginEntry.Text;
        string password = PasswordEntry.Text;

        // Перевірка на порожні поля
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            DisplayAlert("Помилка", "Будь ласка, заповніть логін та пароль!", "ОК");
            return;
        }

        // Проста перевірка пароля (можна змінити на будь-який)
        if (password == "123")
        {
            SystemControl.Instance.CurrentUserName = login;

            // Візуальне оновлення
            UserStatusLabel.Text = $"Вітаємо, {login}! Ви авторизовані.";
            UserStatusLabel.TextColor = Colors.Green;

            // Очистимо поле пароля для безпеки
            PasswordEntry.Text = string.Empty;

            DisplayAlert("Вхід успішний", "Тепер ви можете шукати секції та записуватись на них.", "Чудово");
        }
        else
        {
            DisplayAlert("Помилка входу", "Невірний пароль! Спробуйте '123'.", "ОК");
        }
    }

    // === 2. Пошук ===
    private async void OnSearchClicked(object sender, EventArgs e)
    {
        // Вибір стратегії
        if (RbLinq.IsChecked) SystemControl.Instance.CurrentStrategy = new LinqStrategy();
        else if (RbDom.IsChecked) SystemControl.Instance.CurrentStrategy = new DomStrategy();
        else if (RbSax.IsChecked) SystemControl.Instance.CurrentStrategy = new SaxStrategy();

        Section criteria = new Section
        {
            Name = NameEntry.Text,
            Coach = CoachEntry.Text,
            Time = TimeEntry.Text
        };

        var results = SystemControl.Instance.FindSection(criteria);

        if (results.Count > 0)
        {
            string output = "";
            foreach (var sec in results)
            {
                output += sec.GetDetails() + "\n-----------------\n";
            }
            ResultsEditor.Text = output;

            // Запам'ятовуємо першу знайдену секцію для запису
            _foundSectionName = results[0].Name;

            // Активуємо кнопку запису
            EnrollButton.IsEnabled = true;
            EnrollButton.BackgroundColor = Colors.Blue;
            EnrollButton.Text = $"ЗАПИСАТИСЬ НА {_foundSectionName.ToUpper()}";
        }
        else
        {
            ResultsEditor.Text = "Нічого не знайдено.";
            EnrollButton.IsEnabled = false;
            EnrollButton.BackgroundColor = Colors.Gray;
            _foundSectionName = null;
            await DisplayAlert("Інфо", "Секцій не знайдено", "ОК");
        }
    }

    // === 3. Запис на секцію (Зміна XML) ===
    private async void OnEnrollClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SystemControl.Instance.CurrentUserName))
        {
            await DisplayAlert("Помилка", "Спочатку увійдіть (введіть ім'я зверху)!", "ОК");
            return;
        }

        if (_foundSectionName != null)
        {
            bool result = SystemControl.Instance.EnrollStudent(_foundSectionName);
            if (result)
            {
                await DisplayAlert("Успіх", $"Ви записані на {_foundSectionName}!\nДані збережено у XML.", "Чудово");
                // Очистимо пошук, щоб користувач побачив оновлені дані при наступному пошуку
                OnClearClicked(sender, e);
            }
            else
            {
                await DisplayAlert("Помилка", "Не вдалося записати у файл.", "ОК");
            }
        }
    }

    // === 4. Очищення ===
    private void OnClearClicked(object sender, EventArgs e)
    {
        NameEntry.Text = string.Empty;
        CoachEntry.Text = string.Empty;
        TimeEntry.Text = string.Empty;
        ResultsEditor.Text = string.Empty;
        EnrollButton.IsEnabled = false;
        EnrollButton.BackgroundColor = Colors.Gray;
        EnrollButton.Text = "ЗАПИСАТИСЬ";
        _foundSectionName = null;
    }

    // === 5. Вихід ===
    private async void OnExitClicked(object sender, EventArgs e)
    {
        if (await DisplayAlert("Вихід", "Завершити роботу?", "Так", "Ні"))
        {
            System.Environment.Exit(0);
        }
    }
}