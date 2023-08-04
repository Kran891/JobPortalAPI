using JobPortal.Models;

namespace JobPortal.Repositories
{
    public interface IAdminRepository
    {
        Task<List<CompanyModel>> GetAllUnverifiedCompanies();
        Task<int> VerifyCompany(int CompanyId) ;
        Task<List<CompanyModel>> GetJobPostedToday();
        Task<List<CompanyModel>> GetCompanies();
       
    }
}
