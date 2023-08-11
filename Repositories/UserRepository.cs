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
        private readonly ApplicationDbContext dbContext;

        public UserRepository(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,IConfiguration configuration,ApplicationDbContext dbContext){
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
           
        }
        public async Task<object> InsertUser(UserModel userModel)
        {
            if (!await roleManager.RoleExistsAsync("admin"))
                {
                await roleManager.CreateAsync(new IdentityRole("admin"));
                ApplicationUser admin=new ApplicationUser() { UserName="admin" ,FirstName="admin",LastName="admin",Email="admin@gmail.com"};
                var result1 = await userManager.CreateAsync(admin, "Admin@1234");
                await userManager.AddToRoleAsync(admin, "admin");

            }
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
                return new  {token= JWTTokenGenerator(user, userModel.Role.Trim().ToLower()),userId=user.Id,
                role=userModel.Role
                };
            }
            else return null;
        }

        public async Task<object> LoginUser(LoginModel loginModel)
        {
            var user = await userManager.FindByEmailAsync(loginModel.LEmail.ToUpper());
            
            var result = await userManager.CheckPasswordAsync(user,loginModel.LPassword);
            
            if (user == null || !result)
            {
               
                return null;
            }
            
            else
            {
                int companyId = 0;
                var roles = await userManager.GetRolesAsync(user);
                //if(roles.FirstOrDefault()=="Company")
                companyId = dbContext.Companies.Where(x=>x.Owner.Id==user.Id && x.Status).Select(x=>x.Id).FirstOrDefault();
                   

                return new  {token= JWTTokenGenerator(user, roles.FirstOrDefault()),role=roles.FirstOrDefault(),userId=user.Id, companyId = companyId};
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
