using System.Collections.Generic;
using System.Xml; 
using LAB2_OOP_MAUI.Models;

namespace LAB2_OOP_MAUI.Strategies
{
    public class SaxStrategy : ISearchStrategy
    {
        public List<Section> Search(Section criteria, string filePath)
        {
            var results = new List<Section>();
            Section currentSection = null;

            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    while (reader.Read())
                    {

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Section")
                        {
                            currentSection = new Section();
                            currentSection.Name = reader.GetAttribute("Name");
                            currentSection.Time = reader.GetAttribute("Time");
                            currentSection.Places = reader.GetAttribute("Places");

                        }

                   
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Coach" && currentSection != null)
                        {
                            currentSection.Coach = reader.GetAttribute("Name");
                        }

                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Student" && currentSection != null)
                        {
                            currentSection.Students.Add(reader.GetAttribute("Name"));
                        }

                      
                        else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Section" && currentSection != null)
                        {
                       
                            bool matchName = string.IsNullOrEmpty(criteria.Name) || currentSection.Name == criteria.Name;
                            bool matchCoach = string.IsNullOrEmpty(criteria.Coach) || currentSection.Coach == criteria.Coach;
                            bool matchTime = string.IsNullOrEmpty(criteria.Time) || currentSection.Time == criteria.Time;

                         
                            if (matchName && matchCoach && matchTime)
                            {
                                results.Add(currentSection);
                            }

                           
                            currentSection = null;
                        }
                    }
                }
            }
            catch { }

            return results;
        }
    }
}