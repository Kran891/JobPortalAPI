using JobPortal.Models;
using JobPortal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{

    [Route("[controller]/[action]")]
   // [Authorize(Policy ="Student")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly string uploadsFolder;
        public StudentController(IWebHostEnvironment webHostEnvironment,IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
            this.uploadsFolder = Path.Combine(webHostEnvironment.ContentRootPath, "uploads");
        }
        [HttpPost]
        public async Task<IActionResult> InsertStudentDetails( StudentModel studentModel)
        {
            try
            {
                var data = await studentRepository.InsertStudentDetails(studentModel);
                return Ok(new {data = data});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("{location}/{userid}")]
        [HttpGet]
        public async Task<IActionResult> GetJobsByLocation(string location, string userid)
        {
            try
            {
                var data = await studentRepository.GetJobsByLocation(location, userid);
                return Ok(new {data=data});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("{userid}")]
        [HttpGet]
        public async Task<IActionResult> GetAllJobs(string userid)
        {
            try
            {
                var data =await studentRepository.GetAllJobs(userid);
                return Ok(new {data = data});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Route("{userid}")]
        [HttpGet]
        public async Task<IActionResult> GetJobsByYourSkills(string userid)
        {
            try
            {
                var data = await studentRepository.GetJobsByYourSkills(userid);
                return Ok(new {data = data});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Route("{userid}")]
        [HttpGet]
        public async Task<IActionResult> GetAppliedJobs(string userid)
        {
            try
            {
                var data = await studentRepository.GetAppliedJobs(userid);
                return Ok(new {data = data});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Route("{userid}")]
        [HttpGet]
        public async Task<IActionResult> GetInterviewsScheduled(string userid)
        {
            try
            {
                var data = await studentRepository.GetInterviewsScheduled(userid);
                return Ok(new {data=data});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Route("{skillName}/{userId}")]
        [HttpGet]

        public async Task<IActionResult> InsertSkill(string skillName, string userId)
        {
            try
            {
                var data = await studentRepository.InsertSkill(skillName, userId);
                return Ok(new {data = data});
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("{fileName}")]
        public IActionResult Download(string fileName)
        {
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (System.IO.File.Exists(filePath))
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return File(fileStream, "application/octet-stream", fileName);
            }

            return NotFound(); // File not found
        }
    }
}