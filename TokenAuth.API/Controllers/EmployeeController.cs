using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TokenAuth.API.Data;
using TokenAuth.API.Models;
using System.Web;

namespace TokenAuth.API.Controllers
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        // "User", "Admin", and "SuperUser" can all GET employees
        [Authorize(Roles = "User, Admin, SuperUser")]
        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetEmployees()
        {
            var employees = dbContext.Employees.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, employees);
        }

        // "User", "Admin", and "SuperUser" can all GET employee by id
        [Authorize(Roles = "User, Admin, SuperUser")]
        [Route("{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeById(int id)
        {
            var employee = dbContext.Employees.FirstOrDefault(e => e.Id == id);
            return Request.CreateResponse(HttpStatusCode.OK, employee);
        }

        // "Admin" can POST new employees
        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpPost]
        public HttpResponseMessage PostEmployee(Employee employee)
        {
            dbContext.Employees.Add(employee);
            dbContext.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, employee);
        }

        // "Admin" and "SuperUser" can PUT (update) employees
        [Authorize(Roles = "Admin, SuperUser")]
        [Route("{id:int}")]
        [HttpPut]
        public HttpResponseMessage PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            dbContext.Entry(employee).State = EntityState.Modified;
            dbContext.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, employee);
        }

        // "Admin" can DELETE employees
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteEmployee(int id)
        {
            var employee = dbContext.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            dbContext.Employees.Remove(employee);
            dbContext.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}


