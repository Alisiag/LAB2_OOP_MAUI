using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml; // Бібліотека для XmlReader (SAX)
using LAB2_OOP_MAUI.Models;

namespace LAB2_OOP_MAUI.Strategies
{
    public class SaxStrategy : ISearchStrategy
    {
        public List<Section> Search(Section criteria, string filePath)
        {
            var results = new List<Section>();
            Section currentSection = null; // Тимчасова змінна

            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    while (reader.Read()) // Читаємо файл рядок за рядком
                    {
                        // Якщо знайшли відкриваючий тег <Section>
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Section")
                        {
                            string name = reader.GetAttribute("Name");
                            string coach = reader.GetAttribute("Coach");
                            string time = reader.GetAttribute("Time");
                            string places = reader.GetAttribute("Places");

                            // Перевірка фільтрів
                            bool matchName = string.IsNullOrEmpty(criteria.Name) || name == criteria.Name;
                            bool matchCoach = string.IsNullOrEmpty(criteria.Coach) || coach == criteria.Coach;
                            bool matchTime = string.IsNullOrEmpty(criteria.Time) || time == criteria.Time;

                            if (matchName && matchCoach && matchTime)
                            {
                                currentSection = new Section
                                {
                                    Name = name,
                                    Coach = coach,
                                    Time = time,
                                    Places = places
                                };
                            }
                            else
                            {
                                currentSection = null; // Ця секція нам не підходить
                            }
                        }
                        // Якщо знайшли студента і ми зараз всередині підходящої секції
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Student" && currentSection != null)
                        {
                            currentSection.Students.Add(reader.GetAttribute("Name"));
                        }
                        // Якщо закриваючий тег </Section> і секція була створена -> додаємо в результат
                        else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Section")
                        {
                            if (currentSection != null)
                            {
                                results.Add(currentSection);
                                currentSection = null;
                            }
                        }
                    }
                }
            }
            catch { }

            return results;
        }
    }
}
