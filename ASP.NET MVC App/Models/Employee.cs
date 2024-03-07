using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_MVC_App.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Contact { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public bool tc { get; set; }

        public int countryid { get; set; }
        public Country countryobj { get; set; }

        public int stateid { get; set; }
        public State stateobj { get; set; }

    }
}