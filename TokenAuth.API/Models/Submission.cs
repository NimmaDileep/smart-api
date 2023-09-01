using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenAuth.API.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Role { get; set; }
        public string Client { get; set; }
        public string VendorCompany { get; set; }
        public string VendorName { get; set; }
        public string Status { get; set; }
    }
}