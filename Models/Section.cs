using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LAB2_OOP_MAUI.Models
{

    public class Section
    {
        public string Name { get; set; }
        public string Coach { get; set; }
        public string Time { get; set; }
        public string Places { get; set; }
        public List<string> Students = new List<string>();

        public Section() { }
        

        

        public string GetDetails()
        {
            string studentList = Students.Count > 0 ? string.Join(", ", Students) : "Немає";
            return $"Секція: {Name}\nТренер: {Coach}\nЧас: {Time}\nСтуденти: {studentList}";
        }
    }

   

}
