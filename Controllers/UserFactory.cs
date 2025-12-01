using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAB2_OOP_MAUI.Models;

namespace LAB2_OOP_MAUI.Controllers
{
    public static class UserFactory
    {
        public static User CreateUser(string role, string login)
        {
            switch (role.ToLower())
            {
                case "student": return new Student { Login = login };
                case "instructor": return new Instructor { Login = login };
                default: throw new Exception("Unknown role");
            }
        }
    }
}
