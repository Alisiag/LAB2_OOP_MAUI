using Microsoft.Maui.Controls;
using LAB2_OOP_MAUI.Controllers;
using LAB2_OOP_MAUI.Models;
using LAB2_OOP_MAUI.Strategies; // Переконайтесь, що цей рядок не світиться червоним

namespace LAB2_OOP_MAUI;

public partial class NewPage1 : ContentPage
{
    public NewPage1()
    {
        InitializeComponent();
    }

    // === 1. Обробник кнопки "ПОШУК" ===
    private async void OnSearchClicked(object sender, EventArgs e)
    {
        // Встановлюємо стратегію залежно від обраного RadioButton
        if (RbLinq.IsChecked)
        {
            SystemControl.Instance.CurrentStrategy = new LinqStrategy();
        }
        else if (RbDom.IsChecked)
        {
            SystemControl.Instance.CurrentStrategy = new DomStrategy();
        }
        else if (RbSax.IsChecked)
        {
            SystemControl.Instance.CurrentStrategy = new SaxStrategy();
        }

        // Збираємо критерії пошуку
        Section criteria = new Section
        {
            Name = NameEntry.Text,
            Coach = CoachEntry.Text,
            Time = TimeEntry.Text
        };

        // Виконуємо пошук
        var results = SystemControl.Instance.FindSection(criteria);

        // Виводимо результати
        if (results.Count > 0)
        {
            string output = "";
            foreach (var sec in results)
            {
                output += sec.GetDetails() + "\n-----------------\n";
            }
            ResultsEditor.Text = output;
        }
        else
        {
            ResultsEditor.Text = "Нічого не знайдено.";
            await DisplayAlert("Результат", "За вашим запитом нічого не знайдено", "ОК");
        }
    }

    // === 2. Обробник кнопки "CLEAR" ===
    private void OnClearClicked(object sender, EventArgs e)
    {
        NameEntry.Text = string.Empty;
        CoachEntry.Text = string.Empty;
        TimeEntry.Text = string.Empty;
        ResultsEditor.Text = string.Empty;

        RbLinq.IsChecked = true;
    }

    // === 3. Обробник кнопки "ВИХІД" ===
    private async void OnExitClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Вихід", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");

        if (answer)
        {
            System.Environment.Exit(0);
        }
    }
}