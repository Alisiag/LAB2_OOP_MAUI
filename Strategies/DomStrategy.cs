using System;
using System.Collections.Generic;
using System.Xml; // Бібліотека для DOM
using LAB2_OOP_MAUI.Models;

namespace LAB2_OOP_MAUI.Strategies
{
    public class DomStrategy : ISearchStrategy
    {
        public List<Section> Search(Section criteria, string filePath)
        {
            var results = new List<Section>();
            var doc = new XmlDocument();

            try
            {
                doc.Load(filePath);
                XmlNodeList nodes = doc.GetElementsByTagName("Section");

                foreach (XmlNode node in nodes)
                {
                    // 1. Читаємо атрибути самої секції
                    string name = node.Attributes["Name"]?.Value;
                    string time = node.Attributes["Time"]?.Value;
                    string places = node.Attributes["Places"]?.Value;

                    // 2. Шукаємо ім'я тренера у вкладеному тегу <Coach>
                    string coachName = "";
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.Name == "Coach")
                        {
                            coachName = child.Attributes["Name"]?.Value;
                            break; // Знайшли тренера - виходимо з внутрішнього циклу
                        }
                    }

                    // 3. Перевіряємо критерії (Фільтрація)
                    // Важливо: перевіряємо coachName, який знайшли всередині
                    bool matchName = string.IsNullOrEmpty(criteria.Name) || name == criteria.Name;
                    bool matchCoach = string.IsNullOrEmpty(criteria.Coach) || coachName == criteria.Coach;
                    bool matchTime = string.IsNullOrEmpty(criteria.Time) || time == criteria.Time;

                    if (matchName && matchCoach && matchTime)
                    {
                        Section s = new Section
                        {
                            Name = name,
                            Coach = coachName, // Записуємо знайдене ім'я
                            Time = time,
                            Places = places
                        };

                        // 4. Додаємо студентів
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            if (child.Name == "Student")
                            {
                                s.Students.Add(child.Attributes["Name"]?.Value);
                            }
                        }
                        results.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                // Тут можна обробити помилку
            }

            return results;
        }
    }
}