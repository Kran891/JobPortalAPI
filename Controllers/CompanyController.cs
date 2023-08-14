using JobPortal.Entities;
using JobPortal.Models;
using JobPortal.Repositories;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{

    [Route("[controller]/[action]")]
    // [Authorize(Policy ="Company")]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        [HttpPost]

        public async Task<IActionResult> AddJob([FromBody] JobModel jobModel)
        {
            try
            {
                var data = await companyRepository.AddJob(jobModel);
                return Ok(new { data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]

        public async Task<IActionResult> ScheduleInterview([FromBody]InterViewModel interViewModel)
        {
            try
            {
                var data = await companyRepository.ScheduleInterview(interViewModel);
                return Ok(new { data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("{companyId}")]
        [HttpGet]
        public async Task<IActionResult> GetAllJobsByCompanyId(int companyId)
        {
            try
            {
                var data = await companyRepository.GetAllJobsByCompanyId(companyId);
                return Ok(new { data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("{companyId}/{location}")]
        [HttpGet]
        public async Task<IActionResult> GetAllJobsByCompanyLocation(int companyId, string location)
        {
            try
            {
                var data = await companyRepository.GetAllJobsByCompanyLocation(companyId, location);
                return Ok(new { data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Route("{jobId}")]
        [HttpGet]
        public async Task<IActionResult> GetStudentsAppliedForJob(int jobId)
        {
            try
            {
                var data = await companyRepository.GetStudentsAppliedForJob(jobId);
                return Ok(new { data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("{jobId}")]
        [HttpGet]

        public async Task<IActionResult> GetSuggestionsForRole(int jobId)
        {
            try
            {
                var data = await companyRepository.GetSuggestionsForRole(jobId);
                return Ok(new { data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("{jobId}")]
        [HttpDelete]

        public async Task<IActionResult> DeleteJob(int jobId)
        {
            try
            {
                var data = await companyRepository.DeleteJob(jobId);
                return Ok(new { data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertCompany([FromBody] CompanyModel companymodel)
        {
            try
            {
                var data = await companyRepository.InsertCompany(companymodel);
                return Ok(new { data = data });
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("{jobId}")]
        public async Task<IActionResult> GetSheduledInterViews(int jobId)
        {
            try
            {
                var data =await companyRepository.GetSheduledInterViews(jobId);
                return Ok(new { data = data });
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}