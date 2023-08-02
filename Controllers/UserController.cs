using JobPortal.Models;
using JobPortal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository) {
            this.userRepository = userRepository;
        }
        [HttpPost]
        public async Task<IActionResult> InsertUser(UserModel userModel)
        {
            string tk=await userRepository.InsertUser(userModel);
            if(tk != null)
            {
                return Ok(tk);
            }
            else { return BadRequest(); }
        }
        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginModel loginModel)
            {
            string tk= await userRepository.LoginUser(loginModel);
            if(tk != null)
            {
                return Ok(tk);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Authorize(Policy ="Student")]
        public async Task<IActionResult> CheckAuthrize()
        {
            return Json("Hi");
        }
    }
}
