using JobPortal.Models;

namespace JobPortal.Repositories
{
    public interface IAdminRepository
    {
        Task<List<CompanyModel>> GetAllUnverifiedCompanies();
        Task<int> VerifyCompany(int CompanyId) ;
        Task<List<CompanyModel>> GetCompanies();
        //Task<List<jobModel>> GetCompanies();
        Task<List<JobModel>> GetJobPostedToday();


    }
}
