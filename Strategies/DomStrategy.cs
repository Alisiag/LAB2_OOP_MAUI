using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml; 
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
                    
                    string name = node.Attributes["Name"]?.Value;
                    string coach = node.Attributes["Coach"]?.Value;
                    string time = node.Attributes["Time"]?.Value;
                    string places = node.Attributes["Places"]?.Value;

                    bool matchName = string.IsNullOrEmpty(criteria.Name) || name == criteria.Name;
                    bool matchCoach = string.IsNullOrEmpty(criteria.Coach) || coach == criteria.Coach;
                    bool matchTime = string.IsNullOrEmpty(criteria.Time) || time == criteria.Time;

                    if (matchName && matchCoach && matchTime)
                    {
                        Section s = new Section
                        {
                            Name = name,
                            Coach = coach,
                            Time = time,
                            Places = places
                        };

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
            }

            return results;
        }
    }
}
