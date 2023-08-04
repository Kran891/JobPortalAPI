using JobPortal.Models;
using JobPortal.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    [Route("[controller]/[action]")]
    public class SkillController : Controller
    {
        private readonly ISkillRepository skillRepository;
        public SkillController(ISkillRepository skillRepository)
        {
            this.skillRepository = skillRepository;
        }
        [HttpPost]
        public async Task<IActionResult> InsertSkill(string skillname)
        {
            try
            {
                return Ok(await skillRepository.InsertSkill(skillname));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("{skillname}")]
        [HttpGet]
        public async Task<IActionResult> GetSkill(string skillname)
        {
            try
            {
                return Ok(await skillRepository.GetSkill(skillname));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
        