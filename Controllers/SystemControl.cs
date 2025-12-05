using LAB2_OOP_MAUI.Models;
using LAB2_OOP_MAUI.Strategies;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace LAB2_OOP_MAUI.Controllers
{
    public class SystemControl
    {
        private static SystemControl _instance;

        public ISearchStrategy CurrentStrategy { get; set; }
        public string CurrentUserName { get; set; }

        public UserRole CurrentUserRole { get; set; } = UserRole.Guest;

        public string XmlFilePath { get; set; } = @"C:\Users\Admin\source\repos\LAB2_OOP_MAUI\sports.xml";

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
            CurrentStrategy = new LinqStrategy();
        }

        public List<Section> FindSection(Section criteria)
        {
            return CurrentStrategy.Search(criteria, XmlFilePath);
        }


        public bool CheckLogin(string login, string password)
        {
            try
            {

                XDocument doc = XDocument.Load(XmlFilePath);

                bool isStudent = doc.Descendants("Student").Any(s =>
                    (string)s.Attribute("Name") == login &&
                    (string)s.Attribute("Password") == password);

                bool isCoach = false;
                if (!isStudent)
                {
                    isCoach = doc.Descendants("Coach").Any(c =>
                        (string)c.Attribute("Name") == login &&
                        (string)c.Attribute("Password") == password);
                }
                if (isCoach) CurrentUserRole = UserRole.Coach;
                if (isStudent) CurrentUserRole = UserRole.Student;

                return isStudent || isCoach;
            }
            catch
            {


                return false;
            }
            
        }

        public string GetMySchedule()
        {
            try
            {
                XDocument doc = XDocument.Load(XmlFilePath);
         
                List<string> resultList = new List<string>();

                IEnumerable<XElement> sections = null;

                switch (CurrentUserRole)
                {
                    case UserRole.Student:
               
                        sections = doc.Descendants("Section")
                            .Where(s => s.Elements("Student")
                                         .Any(st => (string)st.Attribute("Name") == CurrentUserName));
                        break;

                    case UserRole.Coach:
                   
                        sections = doc.Descendants("Section")
                            .Where(s => s.Elements("Coach")
                                         .Any(c => (string)c.Attribute("Name") == CurrentUserName));
                        break;
                }

     
                if (sections == null || !sections.Any())
                {
                    return "У вас немає занять.";
                }

              
                foreach (var sec in sections)
                {
                    string name = (string)sec.Attribute("Name");
                    string time = (string)sec.Attribute("Time");


                    resultList.Add($"{name} - {time}");
                }

              
                return string.Join("\n", resultList);
            }
            catch
            {
                return "Помилка при читанні розкладу.";
            }
        }

     
        public void TransformToHtml()
        {
            try
            {
         
                string xslPath = XmlFilePath.Replace("sports.xml", "style.xsl");
                string htmlPath = XmlFilePath.Replace("sports.xml", "report.html");

               
                XslCompiledTransform transform = new XslCompiledTransform();

            
                transform.Load(xslPath);

             
                transform.Transform(XmlFilePath, htmlPath);
            }
            catch (Exception ex)
            {
           
            }
        }
    }
}