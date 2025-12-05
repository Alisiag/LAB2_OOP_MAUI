using LAB2_OOP_MAUI.Controllers;
using LAB2_OOP_MAUI.Models;
using LAB2_OOP_MAUI.Strategies;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Maui.Controls;

namespace LAB2_OOP_MAUI;

public partial class NewPage1 : ContentPage
{
    private string _foundSectionName = null;

    public NewPage1()
    {
        InitializeComponent();
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        string login = LoginEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            DisplayAlert("Помилка", "Будь ласка, заповніть логін та пароль!", "ОК");
            return;
        }
        

        if(!SystemControl.Instance.CheckLogin(login, password))
        {
            DisplayAlert("Помилка", "Невірний логін або пароль!", "ОК");
            return;
        }
        string role;
        if(SystemControl.Instance.CurrentUserRole == UserRole.Student)
        {
            role =  "Студент";
        }
       else if(SystemControl.Instance.CurrentUserRole == UserRole.Coach)
        {
            role = "Тренер";
        }
        else
        {
            role = "Гість";
        }
        if(SystemControl.Instance.CurrentUserRole == UserRole.Coach)
        {
            StrategyFrame.IsVisible = false;
            SearchFrame.IsVisible = false;
            ResultsLayout.IsVisible = false;
        }
        else 
        {
   
            StrategyFrame.IsVisible = true;
            SearchFrame.IsVisible = true;
            ResultsLayout.IsVisible = true;
            
        }
        SystemControl.Instance.CurrentUserName = login;
        DisplayAlert("Вхід успішний", $"Ви увійшли як {role}", "Чудово");
        UserStatusLabel.TextColor = Colors.Green;
        UserStatusLabel.Text = $"Вітаємо, {login}!";
    }

        
    

        

 
    private async void OnSearchClicked(object sender, EventArgs e)
    {

        if (RbLinq.IsChecked) SystemControl.Instance.CurrentStrategy = new LinqStrategy();
        else if (RbDom.IsChecked) SystemControl.Instance.CurrentStrategy = new DomStrategy();
        else if (RbSax.IsChecked) SystemControl.Instance.CurrentStrategy = new SaxStrategy();

        Section criteria = new Section
        {
            Name = NameEntry.Text,
            Coach = CoachEntry.Text,
            Time = TimeEntry.Text
        };
        if (string.IsNullOrWhiteSpace(criteria.Name) &&
            string.IsNullOrWhiteSpace(criteria.Coach) &&
            string.IsNullOrWhiteSpace(criteria.Time))
        {
            await DisplayAlert("Помилка", "Будь ласка, введіть хоча б один критерій пошуку!", "ОК");
            return;
        }

        var results = SystemControl.Instance.FindSection(criteria);

        if (results.Count > 0)
        {
            string output = "";
            foreach (var sec in results)
            {
                output += sec.GetDetails() + "\n-----------------\n";
            }
            ResultsEditor.Text = output;

            _foundSectionName = results[0].Name;

 
        }
        else
        {
            ResultsEditor.Text = "Нічого не знайдено.";

            _foundSectionName = null;
            await DisplayAlert("Інфо", "Секцій не знайдено", "ОК");
        }
    }

   

    private void OnClearClicked(object sender, EventArgs e)
    {
        NameEntry.Text = string.Empty;
        CoachEntry.Text = string.Empty;
        TimeEntry.Text = string.Empty;
        ResultsEditor.Text = string.Empty;

        _foundSectionName = null;
    }

    private async void OnExitClicked(object sender, EventArgs e)
    {
        if (await DisplayAlert("Вихід", "Завершити роботу?", "Так", "Ні"))
        {
            System.Environment.Exit(0);
        }
    }

    private async void OnMyScheduleClicked(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(SystemControl.Instance.CurrentUserName))
        {
            await DisplayAlert("Помилка", "Спочатку увійдіть у систему!", "ОК");
            return;
        }


        string schedule = SystemControl.Instance.GetMySchedule();

    
        MyScheduleEditor.Text = schedule;

    
        if (schedule.Contains("Помилка") || schedule.Contains("немає"))
        {
            await DisplayAlert("Розклад", schedule, "ОК");
        }
    }

    private async void OnHtmlClicked(object sender, EventArgs e)
    {
  
        SystemControl.Instance.TransformToHtml();

  
        await DisplayAlert("Успіх", "Файл report.html успішно створено!", "ОК");
    }

}
