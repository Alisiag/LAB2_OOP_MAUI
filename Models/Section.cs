using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LAB2_OOP_MAUI.Models
{

    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Schedule { get; set; }
        private List<string> _students = new List<string>();

        public Section(int id, string name, string schedule)
        {
            Id = id;
            Name = name;
            Schedule = schedule;
        }

        public void AddStudent(string studentName)
        {
            _students.Add(studentName);
        }

        public string GetDetails()
        {
            return $"Sectin: {Name}, Time {Schedule}, Instructor: ";// Instructor details to be added later
        }
    }

   

}
