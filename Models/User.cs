using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2_OOP_MAUI.Models
{
    public abstract class User
    {
        public string Login { get; set; }
        public abstract string GetRole();
    }

    public class Student : User
    {
        public override string GetRole() => "Student";
    }

    public class Instructor : User
    {
        public override string GetRole() => "Instructor";
    }
}
