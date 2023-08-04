using JobPortal.Entities;
using JobPortal.Models;
using JobPortal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    [Route("[controller]/[action]")]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddJob(JobModel jobModel)
        {
            try
            {
                return Ok(await companyRepository.AddJob(jobModel));
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
                return Ok(await companyRepository.GetAllJobsByCompanyId(companyId));
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
                return Ok(await companyRepository.GetAllJobsByCompanyLocation(companyId, location));
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
                return Ok(await companyRepository.GetStudentsAppliedForJob(jobId));
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
                return Ok(await companyRepository.GetSuggestionsForRole(jobId));
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
                return Ok(await companyRepository.DeleteJob(jobId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertCompany(CompanyModel company)
        {
            try
            {
                return Ok(await companyRepository.InsertCompany(company));
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}