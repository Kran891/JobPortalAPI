using JobPortal.Models;

namespace JobPortal.Repositories
{
    public interface ICompanyRepository
    {
        Task<int> AddJob(JobModel jobModel);
        Task<List<JobModel>> GetAllJobsByCompanyId(int companyId);
        Task<List<JobModel>> GetAllJobsByCompanyLoaction(int companyId,string location);
        Task<List<StudentModel>> GetStudentsAppliedForJob(int jobId);
        Task<List<StudentModel>> GetSuggestionsForRole(int jobId);
        Task<List<string>> GetCompanyLocations(int companyId);
        Task<int> DeleteJob(int jobId);
    }
}
