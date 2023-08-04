using JobPortal.Models;


namespace JobPortal.Repositories
{
    public interface IUserRepository
    {
        Task<Object> InsertUser(UserModel userModel);
        Task<Object> LoginUser(LoginModel loginModel);
    }
}
