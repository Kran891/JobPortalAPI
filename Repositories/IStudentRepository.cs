using JobPortal.Entities;
using JobPortal.Models;

namespace JobPortal.Repositories
{
    public interface IStudentRepository
    {
        Task<ApplicationUser> InsertStudentDetails(StudentModel studentModel);
        Task<List<JobModel>> GetJobsByLocation(string location,string userid);
        Task<List<JobModel>> GetAllJobs(string userid);
        Task<List<JobModel>> GetJobsByYourSkills(string userid);
        Task<List<JobModel>> GetAppliedJobs(string userid);
        Task<List<JobModel>> GetInterviewsScheduled(string userid);
        Task<int> ApplyJob(int  jobId,string userId);
        Task<int> InsertSkill(string skillName,string userId);
    }
}
