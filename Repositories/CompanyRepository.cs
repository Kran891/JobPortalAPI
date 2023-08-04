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
        private readonly ISkillRepository skillRespository;
        private readonly ILocationRepository locationRepository;

        public CompanyRepository(ApplicationDbContext dbContext, ISkillRepository skillRespository,ILocationRepository locationRepository) {
            this.dbContext = dbContext;
            this.skillRespository = skillRespository;
            this.locationRepository = locationRepository;
        }
        public async Task<int> AddJob(JobModel jobModel)
        {
           Jobs? job=await(from j in dbContext.Jobs where j.Title.ToLower()==jobModel.Title.ToLower() select j).FirstOrDefaultAsync();
            if (job != null)
            {
                return -1;
            }
           Company? company=(from c in dbContext.Companies where c.Id==jobModel.CompanyId select c).FirstOrDefault(); 
           string obj=JsonConvert.SerializeObject(jobModel);
           Jobs? jobs=JsonConvert.DeserializeObject<Jobs>(obj);
            jobs.Company = company;
           dbContext.Jobs.Add(jobs);
            JobSkills jobSkill;
            List<JobSkills> skills = new List<JobSkills>();
           foreach(string skill in jobModel.RequiredSkills)
            {
                Skills requireSkill = await skillRespository.GetSkill(skill.ToLower());
                if(requireSkill == null) {
                    requireSkill = await skillRespository.InsertSkill(skill.ToLower());
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
            List<JobModel> jobs = await(from js in dbContext.JobSkills
                                        join j in dbContext.Jobs on js.job.Id equals j.Id
                                        join cl in dbContext.CompanyLocations on j.Company.Id equals cl.Company.Id
                                        where j.Company.Id == companyId &&
                                        cl.Location.Name.Trim().ToLower() == location.Trim().ToLower()
                                        select new JobModel
                                        {
                                            CompanyId = j.Company.Id,
                                            CompanyName = j.Company.Name,
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

        public async Task<List<string>> GetCompanyLocations(int companyId)
        {
            List<string> companyLocations=await(from cl  in dbContext.CompanyLocations where cl.Company.Id==companyId select cl.Location.Name).ToListAsync();
            return companyLocations;
        }

        public async Task<List<StudentModel>> GetStudentsAppliedForJob(int jobId)
        {
            List<StudentModel> appliedJobs=(from Aj in dbContext.AppliedJobs where Aj.Job.Id ==jobId 
                                            select new StudentModel
                                            {
                                                StudentId=Aj.User.Id,
                                                Resume=Aj.User.Resume,
                                                FullName=Aj.User.FullName,
                                                studentskills=(from sk in dbContext.StudentSkills where sk.user.Id==Aj.User.Id
                                                               select sk.skill.Name).ToList(),
                                                preferredLocations=(from pl in dbContext.PreferredLocations where pl.User.Id==Aj.User.Id
                                                                    select pl.Location.Name).ToList()

                                            }
                                            ).ToList();
            return appliedJobs;
        }

        public async Task<List<StudentModel>> GetSuggestionsForRole(int jobId)
        {
            List<StudentModel> studentModels = (from jk in dbContext.JobSkills
                                                join sk in dbContext.StudentSkills on jk.Skill.Id equals sk.skill.Id
                                                group sk by sk.user into studentSkillGroup
                                                where studentSkillGroup.Count() > 2
                                                select new StudentModel
                                                {
                                                    FullName=studentSkillGroup.Key.FullName,
                                                    studentskills=(from sk in dbContext.StudentSkills where sk.user.Id ==studentSkillGroup.Key.Id select sk.skill.Name).ToList()
                                                }
                                                ).ToList();
            return studentModels;
        }

        public async Task<int> InsertCompany(CompanyModel company)
        {
            ApplicationUser user =await (from u in dbContext.Users where u.Id == company.OwnerId select u).FirstOrDefaultAsync();
            string obj = JsonConvert.SerializeObject(company);
            Company company1 = JsonConvert.DeserializeObject<Company>(obj);
            company1.Owner = user;
            company1.Status = false;
            dbContext.Companies.Add(company1);
            List<CompanyLocation> companyLocations = new List<CompanyLocation>();
            PreferredLocation preferredLocation;
            foreach (string place in company.CompanyLocations)
            {
                Location location = await locationRepository.GetLocation(place);
                if (location == null)
                {
                    location = await locationRepository.InsertLocation(place.ToLower());
                }
                CompanyLocation companyLocation = new CompanyLocation() { Location = location, Company = company1 };
                companyLocations.Add(companyLocation);
            }
            dbContext.CompanyLocations.AddRange(companyLocations);
            dbContext.SaveChanges();
            if (company1 == null)
                return -1;
            return company1.Id;
        }
    }
}
