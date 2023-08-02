using JobPortal.Entities;
using JobPortal.Models;
using Microsoft.AspNetCore.Identity;

using System.IdentityModel.Tokens.Jwt;
using System.Linq;

using Newtonsoft.Json;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace JobPortal.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;
        

        public UserRepository(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,IConfiguration configuration){
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
           
        }
        public async Task<string> InsertUser(UserModel userModel)
        {
            string obj=JsonConvert.SerializeObject(userModel);
            ApplicationUser user=JsonConvert.DeserializeObject<ApplicationUser>(obj);
            user.UserName = userModel.Email.Split('@')[0];
            
            var result=await userManager.CreateAsync(user,userModel.Password);
            if (result.Succeeded)
            {

                if (!await roleManager.RoleExistsAsync(userModel.Role.Trim().ToLower()))
                {
                    await roleManager.CreateAsync(new IdentityRole(userModel.Role.Trim().ToLower()));
                }
                await userManager.AddToRoleAsync(user, userModel.Role);
                return JWTTokenGenerator(user, userModel.Role);
            }
            else return null;
        }

        public async Task<string> LoginUser(LoginModel loginModel)
        {
            var user = await userManager.FindByEmailAsync(loginModel.Email.ToUpper());
            
            var result = await userManager.CheckPasswordAsync(user,loginModel.Password);
            
            if (user == null || !result)
            {
               
                return null;
            }
            else
            {
                var roles = await userManager.GetRolesAsync(user);

                return JWTTokenGenerator(user, roles.FirstOrDefault());
            }

        }


        


        private string JWTTokenGenerator(ApplicationUser user,string role)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
            var tokenHandler=new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, role),
                        new Claim("userid",user.Id)
                    }
                    ),
                Expires = DateTime.Now.AddHours(Convert.ToDouble(jwtSettings["ExpirationHours"])),
                SigningCredentials=new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                    )
            };
            var token=tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<bool> SignOutUser()
        {
            await signInManager.SignOutAsync();
            return true;
        }
        
    }
}
