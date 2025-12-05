using System;
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
                            let coachElement = section.Element("Coach") 
                            let coachName = (string)coachElement?.Attribute("Name") 

                            where (string.IsNullOrEmpty(criteria.Name) || (string)section.Attribute("Name") == criteria.Name)
                               && (string.IsNullOrEmpty(criteria.Coach) || coachName == criteria.Coach) 
                               && (string.IsNullOrEmpty(criteria.Time) || (string)section.Attribute("Time") == criteria.Time)
                            select section;

                foreach (var xElement in query)
                {
                    Section s = new Section();
                    s.Name = (string)xElement.Attribute("Name");

                  
                    var coachNode = xElement.Element("Coach");
                    s.Coach = (string)coachNode?.Attribute("Name");

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