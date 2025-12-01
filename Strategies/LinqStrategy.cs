using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq; // Обов'язково для LINQ to XML
using LAB2_OOP_MAUI.Models;

namespace LAB2_OOP_MAUI.Strategies
{
    public class LinqStrategy : ISearchStrategy
    {
        public List<Section> Search(Section criteria, string filePath)
        {
            var results = new List<Section>();

            try
            {
                // 1. Завантажуємо документ
                XDocument doc = XDocument.Load(filePath);

                // 2. Шукаємо всі елементи <Section>
                // Where фільтрує результати. Якщо критерій пустий - беремо все.
                var query = from section in doc.Descendants("Section")
                            where (string.IsNullOrEmpty(criteria.Name) || (string)section.Attribute("Name") == criteria.Name)
                               && (string.IsNullOrEmpty(criteria.Coach) || (string)section.Attribute("Coach") == criteria.Coach)
                               && (string.IsNullOrEmpty(criteria.Time) || (string)section.Attribute("Time") == criteria.Time)
                            select section;

                // 3. Перетворюємо знайдене XML у об'єкти C# Section
                foreach (var xElement in query)
                {
                    Section s = new Section();
                    s.Name = (string)xElement.Attribute("Name");
                    s.Coach = (string)xElement.Attribute("Coach");
                    s.Time = (string)xElement.Attribute("Time");
                    s.Places = (string)xElement.Attribute("Places");

                    // Витягуємо студентів
                    foreach (var studentNode in xElement.Descendants("Student"))
                    {
                        s.Students.Add((string)studentNode.Attribute("Name"));
                    }

                    results.Add(s);
                }
            }
            catch (Exception ex)
            {
                // Якщо файлу немає або помилка - просто повернемо пустий список (або можна кинути помилку)
            }

            return results;
        }
    }
}