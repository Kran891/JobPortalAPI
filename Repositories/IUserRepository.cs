using JobPortal.Models;


namespace JobPortal.Repositories
{
    public interface IUserRepository
    {
        Task<string> InsertUser(UserModel userModel);
        Task<string> LoginUser(LoginModel loginModel);
    }
}
