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
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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
            var submissions = dbContext.Submissions.ToList();

            return Request.CreateResponse(HttpStatusCode.OK, submissions);
        }



        [Authorize(Roles = "User, Admin, SuperUser")]
        [Route("{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetSubmissionById(int id)
        {
            var submission = dbContext.Submissions
                                      .Where(s => s.EmployeeId == id).FirstOrDefault();

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
            if (employee is null)
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



        [HttpGet]
        [Route("DownloadResume/{submissionId:int}")]
        public HttpResponseMessage DownloadResume(int submissionId)
        {
            var submission = dbContext.Submissions.FirstOrDefault(s => s.Id == submissionId);

            if (submission?.Resume == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(submission.Resume)
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(submission.ResumeContentType);
            return result;
        }

        [HttpPost]
        [Route("UploadResume/{submissionId:int}")]
        public async Task<HttpResponseMessage> UploadResume(int submissionId)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            if (provider.Contents.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No file uploaded.");
            }

            var file = provider.Contents[0];
            var filename = file.Headers.ContentDisposition.FileName.Trim('"');
            byte[] fileArray = await file.ReadAsByteArrayAsync();

            var submission = dbContext.Submissions.FirstOrDefault(s => s.Id == submissionId);
            if (submission == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Submission not found.");
            }

            submission.Resume = fileArray;
            submission.ResumeContentType = file.Headers.ContentType.MediaType;

            dbContext.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "Resume uploaded successfully.");
        }


        [HttpPut]
        [Route("UpdateResume/{submissionId:int}")]
        public async Task<HttpResponseMessage> UpdateResume(int submissionId)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var submission = dbContext.Submissions.FirstOrDefault(s => s.Id == submissionId);
            if (submission == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Submission not found.");
            }

            if (submission.Resume == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No existing resume found to update.");
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            if (provider.Contents.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No file uploaded.");
            }

            var file = provider.Contents[0];
            byte[] fileArray = await file.ReadAsByteArrayAsync();

            submission.Resume = fileArray;
            submission.ResumeContentType = file.Headers.ContentType.MediaType;

            dbContext.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "Resume updated successfully.");
        }

        [HttpDelete]
        [Route("DeleteResume/{submissionId:int}")]
        public HttpResponseMessage DeleteResume(int submissionId)
        {
            var submission = dbContext.Submissions.FirstOrDefault(s => s.Id == submissionId);
            if (submission == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Submission not found.");
            }

            if (submission.Resume == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No resume found to delete.");
            }

            submission.Resume = null;
            submission.ResumeContentType = null;

            dbContext.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "Resume deleted successfully.");
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

