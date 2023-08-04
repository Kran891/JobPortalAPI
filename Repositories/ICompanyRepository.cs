using JobPortal.Models;

namespace JobPortal.Repositories
{
    public interface ICompanyRepository
    {
        Task<int> AddJob(JobModel jobModel);
        Task<int> InsertCompany(CompanyModel company);
        Task<List<JobModel>> GetAllJobsByCompanyId(int companyId);
        Task<List<JobModel>> GetAllJobsByCompanyLocation(int companyId,string location);
        Task<List<StudentModel>> GetStudentsAppliedForJob(int jobId);
        Task<List<StudentModel>> GetSuggestionsForRole(int jobId);
        Task<List<string>> GetCompanyLocations(int companyId);
        Task<int> DeleteJob(int jobId);
    }
}
