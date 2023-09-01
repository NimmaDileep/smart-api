using System;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using TokenAuth.API.Data;
using TokenAuth.API.Models;
using TokenAuth.API.UserRepository;
using System.Linq;
using System.Data.Entity;

namespace TokenAuth.API.Controllers
{
    [RoutePrefix("api/submission")]
    public class SubmissionController : ApiController
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();


        [Authorize(Roles = "User, Admin, SuperUser")]
        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetSubmissions()
        {

            var submissions = dbContext.Submissions.Select(s => new
            {
                s.Id,
                s.EmployeeId,
                s.Name,
                s.Date,
                s.Role,
                s.Client,
                s.VendorCompany,
                s.VendorName,
                s.Status
            }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, submissions);
        }



        [Authorize(Roles = "User, Admin, SuperUser")]
        [Route("{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetSubmissionById(int id)
        {
            var submission = dbContext.Submissions
                                      .Where(s => s.EmployeeId == id)
                                      .Select(s => new
                                      {
                                          s.Id,
                                          s.EmployeeId,
                                          s.Name,
                                          s.Date,
                                          s.Role,
                                          s.Client,
                                          s.VendorCompany,
                                          s.VendorName,
                                          s.Status
                                      });

            if (submission == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, submission);
        }




        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpPost]
        public HttpResponseMessage PostSubmission(Submission submission)
        {
            var employee = dbContext.Employees.FirstOrDefault(e => e.Id == submission.EmployeeId);
            if (employee == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found.");
            }

            submission.Name = employee.Name;
            dbContext.Submissions.Add(submission);
            dbContext.SaveChanges();

            var responseSubmission = new
            {
                submission.Id,
                submission.EmployeeId,
                submission.Name,
                submission.Date,
                submission.Role,
                submission.Client,
                submission.VendorCompany,
                submission.VendorName,
                submission.Status
            };

            return Request.CreateResponse(HttpStatusCode.Created, responseSubmission);
        }




        [Authorize(Roles = "Admin, SuperUser")]
        [Route("{id:int}")]
        [HttpPut]
        public HttpResponseMessage PutSubmission(int id, Submission submission)
        {
            if (id != submission.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var employee = dbContext.Employees.FirstOrDefault(e => e.Id == submission.EmployeeId);
            if (employee == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found.");
            }

            submission.Name = employee.Name;
            dbContext.Entry(submission).State = EntityState.Modified;
            dbContext.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, submission);
        }



        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteSubmission(int id)
        {
            var submission = dbContext.Submissions.FirstOrDefault(s => s.Id == id);
            if (submission == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            dbContext.Submissions.Remove(submission);
            dbContext.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Employee Deleted Successfully");
        }
    }
}

