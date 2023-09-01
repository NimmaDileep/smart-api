using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TokenAuth.API.Models;

namespace TokenAuth.API.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(): base("DefaultConnection")
        {
                
        }
       public DbSet<User> Users { get; set; }
       public DbSet<Employee> Employees { get; set; }
       public DbSet<Submission> Submissions { get; set; }

    }

}