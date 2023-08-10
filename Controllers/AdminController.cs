using JobPortal.Entities;
using JobPortal.Models;
using JobPortal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace JobPortal.Controllers
{
    [Route("[controller]/[action]")]
   // [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }
        [HttpGet]
        //[HttpGet]
       // [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAllUnverifiedCompanies()
        {
            try
            {
                var data = await adminRepository.GetAllUnverifiedCompanies();
                return Ok(new { data = data});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
       [Route("{CompanyId}")]
        [HttpGet]
        //[HttpGet]
       

        public async Task<IActionResult> VerifyCompany(int CompanyId)
        {

            try
            {
                var data = await adminRepository.VerifyCompany(CompanyId);
                return Ok(new {data=data});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpGet]
       // [HttpGet]
       // [Authorize(Policy = "Company")]

        public async Task<IActionResult> GetJobPostedToday()
        {
            try
            {
                var data = await adminRepository.GetJobPostedToday();
                return Ok(new { data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpGet]
        //[HttpGet]
       // [Authorize(Policy = "Admin")]

        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                var data = await adminRepository.GetCompanies();
                return Ok(new {data = data});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

    }




