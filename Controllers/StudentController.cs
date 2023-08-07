using JobPortal.Models;
using JobPortal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{

    [Route("[controller]/[action]")]
    [Authorize(Policy ="Student")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository studentRepository;
        public StudentController(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }
        [HttpPost]
        public async Task<IActionResult> InsertStudentDetails([FromBody] StudentModel studentModel)
        {
            try
            {
                return Ok(await studentRepository.InsertStudentDetails(studentModel));
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
                return Ok(await studentRepository.GetJobsByLocation(location, userid));
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
                return Ok(await studentRepository.GetAllJobs(userid));
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
                return Ok(await studentRepository.GetJobsByYourSkills(userid));
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
                return Ok(await studentRepository.GetAppliedJobs(userid));
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
                return Ok(await studentRepository.GetInterviewsScheduled(userid));
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
                return Ok(await studentRepository.InsertSkill(skillName, userId));
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}