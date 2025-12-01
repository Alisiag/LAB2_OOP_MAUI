
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAB2_OOP_MAUI.Models;

namespace LAB2_OOP_MAUI.Controllers
{
    public class SystemControl
    {
        private static SystemControl _instance;
        private List<Section> _sections;
        public User CurrentUser { get; private set; }

        // Singleton
        public static SystemControl Instance
        {
            get
            {
                if (_instance == null) _instance = new SystemControl();
                return _instance;
            }
        }

        private SystemControl()
        {
            _sections = new List<Section>
            {
                new Section(101, "Basketball", "Mon 18:00"),
                new Section(102, "Tennis", "Tue 16:00")
            };
        }

        public bool CheckLogin(string login, string password)
        {
            // Спрощена логіка
            if (password == "123")
            {
                CurrentUser = UserFactory.CreateUser("student", login);
                return true;
            }
            return false;
        }

        public Section FindSection(string name)
        {
            // Якщо Find не працює, додайте using System.Linq;
            return _sections.Find(s => s.Name == name);
        }

        public bool EnrollStudent(string sectionName)
        {
            var section = FindSection(sectionName);
            if (section != null && CurrentUser is Student)
            {
                section.AddStudent(CurrentUser.Login);
                return true;
            }
            return false;
        }
    }

   
}
