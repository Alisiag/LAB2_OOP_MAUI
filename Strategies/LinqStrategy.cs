using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq; 
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
                XDocument doc = XDocument.Load(filePath);

                var query = from section in doc.Descendants("Section")
                            where (string.IsNullOrEmpty(criteria.Name) || (string)section.Attribute("Name") == criteria.Name)
                               && (string.IsNullOrEmpty(criteria.Coach) || (string)section.Attribute("Coach") == criteria.Coach)
                               && (string.IsNullOrEmpty(criteria.Time) || (string)section.Attribute("Time") == criteria.Time)
                            select section;

                foreach (var xElement in query)
                {
                    Section s = new Section();
                    s.Name = (string)xElement.Attribute("Name");
                    s.Coach = (string)xElement.Attribute("Coach");
                    s.Time = (string)xElement.Attribute("Time");
                    s.Places = (string)xElement.Attribute("Places");

                    foreach (var studentNode in xElement.Descendants("Student"))
                    {
                        s.Students.Add((string)studentNode.Attribute("Name"));
                    }

                    results.Add(s);
                }
            }
            catch (Exception ex)
            {
            }

            return results;
        }
    }
}