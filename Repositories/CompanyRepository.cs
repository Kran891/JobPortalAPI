using JobPortal.Entities;
using JobPortal.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
namespace JobPortal.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ISkillRespository skillRespository;

        public CompanyRepository(ApplicationDbContext dbContext,ISkillRespository skillRespository) {
            this.dbContext = dbContext;
            this.skillRespository = skillRespository;
        }
        public async Task<int> AddJob(JobModel jobModel)
        {
           Company company=(from c in dbContext.Companies where c.Id==jobModel.CompanyId select c).FirstOrDefault(); 
           string obj=JsonConvert.SerializeObject(jobModel);
           Jobs jobs=JsonConvert.DeserializeObject<Jobs>(obj);
            jobs.Company = company;
           dbContext.Jobs.Add(jobs);
            JobSkills jobSkill;
            List<JobSkills> skills = new List<JobSkills>();
           foreach(string skill in jobModel.RequiredSkills)
            {
                Skills requireSkill = await skillRespository.GetSKill(skill);
                if(requireSkill == null) {
                    requireSkill = await skillRespository.InsertSkill(skill);
                }
                jobSkill=new JobSkills() { job=jobs,Skill=requireSkill};
                skills.Add(jobSkill);
            }
           dbContext.JobSkills.AddRange(skills);
           await dbContext.SaveChangesAsync();
            return jobs.Id;
        }

        public Task<int> DeleteJob(int jobId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JobModel>> GetAllJobsByCompanyId(int companyId)
        {
            List<JobModel> jobs = await (from c in dbContext.Companies
                                         join j in dbContext.Jobs on c.Id equals j.Company.Id
                                         join js in dbContext.JobSkills on j.Id equals js.job.Id

                                         select new JobModel
                                         {
                                             CompanyId = c.Id,
                                             CompanyName = c.Name,
                                             Salary = j.Salary,
                                             RequiredSkills = (from rs in dbContext.JobSkills where rs.job == j select rs.Skill.Name).ToList(),
                                             Title = j.Title,
                                             JobId = j.Id,
                                             Description = j.Description,
                                             NoOfApplicants = (from ja in dbContext.AppliedJobs where ja.Job == j select ja.Id).ToList().Count(),
                                             Locations = (from c in dbContext.Companies
                                                          join
                                                        cl in dbContext.CompanyLocations on c.Id equals cl.Company.Id
                                                          select cl.Location.Name).ToList(),
                                         }
                                   ).ToListAsync();
            return jobs;
        }

        public async Task<List<JobModel>> GetAllJobsByCompanyLocation(int companyId, string location)
        {
            List<JobModel> jobs = await(from c in dbContext.Companies
                                        join j in dbContext.Jobs on c.Id equals j.Company.Id
                                        join js in dbContext.JobSkills on j.Id equals js.job.Id
                                        join cl in dbContext.CompanyLocations on c.Id equals cl.Company.Id
                                        where c.Id == companyId &&
                                        cl.Location.Name.Trim().ToLower() == location.Trim().ToLower()
                                        select new JobModel
                                        {
                                            CompanyId = c.Id,
                                            CompanyName = c.Name,
                                            Salary = j.Salary,
                                            RequiredSkills = (from rs in dbContext.JobSkills where rs.job == j select rs.Skill.Name).ToList(),
                                            Title = j.Title,
                                            JobId = j.Id,
                                            Description = j.Description,
                                            NoOfApplicants = (from ja in dbContext.AppliedJobs where ja.Job == j select ja.Id).ToList().Count(),
                                            
                                        }
                                   ).ToListAsync();
            return jobs;
        }

        public Task<List<string>> GetCompanyLocations(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudentModel>> GetStudentsAppliedForJob(int jobId)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudentModel>> GetSuggestionsForRole(int jobId)
        {
            throw new NotImplementedException();
        }
    }
}
