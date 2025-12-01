using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Maui.Controls;
using LAB2_OOP_MAUI.Controllers;
using LAB2_OOP_MAUI.Models;

namespace LAB2_OOP_MAUI;

public partial class NewPage1 : ContentPage
{
    // Змінна для збереження секції, яку ми зараз переглядаємо
    private Section _currentSectionViewed;

    public NewPage1()
    {
        InitializeComponent();
    }

    // === Обробник кнопки "Увійти" ===
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string login = LoginEntry.Text;
        string pass = PasswordEntry.Text;

        // Викликаємо наш Singleton контролер
        bool success = SystemControl.Instance.CheckLogin(login, pass);

        if (success)
        {
            var role = SystemControl.Instance.CurrentUser.GetRole();
            await DisplayAlert("Успіх", $"Вітаємо, {login}! Ви увійшли як {role}.", "ОК");
        }
        else
        {
            await DisplayAlert("Помилка", "Невірний логін або пароль.", "ОК");
        }
    }

    // === Обробник кнопки "Знайти та переглянути" ===
    private async void OnFindClicked(object sender, EventArgs e)
    {
        string searchName = SearchEntry.Text;

        // Звертаємося до контролера для пошуку
        _currentSectionViewed = SystemControl.Instance.FindSection(searchName);

        if (_currentSectionViewed != null)
        {
            // Оновлюємо Label інформацією з моделі
            InfoLabel.Text = _currentSectionViewed.GetDetails();
            InfoLabel.TextColor = Colors.Black;

            // Вмикаємо кнопку запису і робимо її зеленою
            EnrollButton.IsEnabled = true;
            EnrollButton.BackgroundColor = Colors.Green;
        }
        else
        {
            InfoLabel.Text = $"Секцію '{searchName}' не знайдено.";
            InfoLabel.TextColor = Colors.Red;

            // Вимикаємо кнопку запису
            EnrollButton.IsEnabled = false;
            EnrollButton.BackgroundColor = Colors.Gray;
            await DisplayAlert("Увага", "Секцію не знайдено. Спробуйте 'Basketball' або 'Tennis'", "ОК");
        }
    }

    // === Обробник кнопки "ЗАПИСАТИСЬ" ===
    private async void OnEnrollClicked(object sender, EventArgs e)
    {
        // Проста перевірка на "магічну кнопку" - якщо секція не вибрана, нічого не робимо
        if (_currentSectionViewed == null) return;

        // Перевіряємо, чи користувач взагалі увійшов
        if (SystemControl.Instance.CurrentUser == null)
        {
            await DisplayAlert("Помилка", "Спочатку увійдіть в систему!", "ОК");
            return;
        }

        // Делегуємо логіку запису контролеру
        bool success = SystemControl.Instance.EnrollStudent(_currentSectionViewed.Name);

        if (success)
        {
            await DisplayAlert("Ура!", "Вас успішно записано на секцію!", "Чудово");
            // Оновлюємо інформацію на екрані, щоб показати нову кількість студентів
            InfoLabel.Text = _currentSectionViewed.GetDetails();
        }
        else
        {
            // Якщо контролер повернув false, значить користувач не студент
            await DisplayAlert("Помилка", "Записуватись можуть тільки студенти.", "ОК");
        }
    }
}