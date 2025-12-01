using LAB2_OOP_MAUI.Models;
using LAB2_OOP_MAUI.Strategies;
using System.Collections.Generic;
using System.Xml.Linq;

namespace LAB2_OOP_MAUI.Controllers
{
    public class SystemControl
    {
        private static SystemControl _instance;

        // 1. Посилання на алгоритм пошуку (Стратегію)
        public ISearchStrategy CurrentStrategy { get; set; }
        public string CurrentUserName { get; set; }

        // 2. Шлях до файлу XML (Вкажіть реальний шлях на вашому ПК!)
        // Для лабораторної найпростіше покласти файл на диск C: або D:
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
            // Стратегія за замовчуванням
            CurrentStrategy = new LinqStrategy();
        }

        // 3. Головний метод пошуку
        public List<Section> FindSection(Section criteria)
        {
            // Делегуємо роботу конкретній стратегії (SAX, DOM або LINQ)
            return CurrentStrategy.Search(criteria, XmlFilePath);
        }

        // === НОВИЙ МЕТОД: Запис студента у файл XML ===
        public bool EnrollStudent(string sectionName)
        {
            if (string.IsNullOrEmpty(CurrentUserName)) return false;

            try
            {
                // 1. Завантажуємо файл
                XDocument doc = XDocument.Load(XmlFilePath);

                // 2. Шукаємо потрібну секцію за назвою
                var sectionElement = doc.Descendants("Section")
                    .FirstOrDefault(s => (string)s.Attribute("Name") == sectionName);

                if (sectionElement != null)
                {
                    // 3. Створюємо новий елемент Student
                    XElement newStudent = new XElement("Student",
                        new XAttribute("Name", CurrentUserName),
                        new XAttribute("Course", "1") // Курс можна зробити параметром, поки хай буде 1
                    );

                    // 4. Додаємо студента в секцію
                    sectionElement.Add(newStudent);

                    // 5. ЗБЕРІГАЄМО файл назад на диск
                    doc.Save(XmlFilePath);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}