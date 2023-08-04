using JobPortal.Entities;
using JobPortal.Models;
using JobPortal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace JobPortal.Controllers
{
    [Route("[controller]/[action]")]
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
                return Ok(await adminRepository.GetAllUnverifiedCompanies());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
       [Route("{CompanyId}")]
        [HttpGet]
        //[HttpGet]
       // [Authorize(Policy = "Admin")]

        public async Task<IActionResult> VerifyCompany(int CompanyId)
        {

            try
            {
                return Ok(await adminRepository.VerifyCompany(CompanyId));
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
                return Ok(await adminRepository.GetJobPostedToday());
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
                return Ok(await adminRepository.GetCompanies());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

    }




