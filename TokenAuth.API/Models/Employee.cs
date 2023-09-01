using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenAuth.API.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public string Position { get; set; }
        public int Wage { get; set; }
    }
}