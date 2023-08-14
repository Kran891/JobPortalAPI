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
        private readonly INotificationRepository notificationRepository;

        public CompanyRepository(ApplicationDbContext dbContext,INotificationRepository notificationRepository, ISkillRepository skillRespository,ILocationRepository locationRepository) {
            this.dbContext = dbContext;
            this.skillRespository = skillRespository;
            this.locationRepository = locationRepository;
            this.notificationRepository = notificationRepository;
        }
        public async Task<int> AddJob(JobModel jobModel)
        {
           Jobs? job=await(from j in dbContext.Jobs where j.Title.ToLower()==jobModel.Title.ToLower() && j.Company.Id==jobModel.CompanyId select j).FirstOrDefaultAsync();
            if (job != null)
            {
                return -1;
            }
           Company? company=(from c in dbContext.Companies where c.Id==jobModel.CompanyId select c).FirstOrDefault(); 
           string obj=JsonConvert.SerializeObject(jobModel);
           Jobs? jobs=JsonConvert.DeserializeObject<Jobs>(obj);
            jobs.Company = company;
            jobs.PostedDate = DateTime.Now;
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

        public async Task<int> DeleteJob(int jobId)
        {
            Jobs job=dbContext.Jobs.FirstOrDefault(x=>x.Id==jobId);
            job.DeleteStatus = true;
            dbContext.Jobs.Update(job);
            dbContext.SaveChanges();
           return job.Id;
        }

        public async Task<List<JobModel>> GetAllJobsByCompanyId(int companyId)
        {
            List<JobModel> jobs = await (from  j in dbContext.Jobs
                                         
                                         where !j.DeleteStatus
                                         select new JobModel
                                         {
                                             CompanyId = j.Company.Id,
                                             CompanyName = j.Company.Name,
                                             Salary = j.Salary,
                                             RequiredSkills = (from rs in dbContext.JobSkills where rs.job == j select rs.Skill.Name).ToList(),
                                             Title = j.Title,
                                             JobId = j.Id,
                                             Description = j.Description,
                                             NoOfApplicants = (from ja in dbContext.AppliedJobs where ja.Job == j select ja.Id).ToList().Count,
                                             Locations = (from cl in dbContext.CompanyLocations
                                                          where cl.Company.Id==j.Company.Id
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
                                            NoOfApplicants = (from ja in dbContext.AppliedJobs where ja.Job == j select ja.Id).ToList().Count,
                                            
                                        }
                                   ).ToListAsync();
            return jobs;
        }

        public async Task<List<string>> GetCompanyLocations(int companyId)
        {
            List<string> companyLocations=await(from cl  in dbContext.CompanyLocations where cl.Company.Id==companyId select cl.Location.Name).ToListAsync();
            return companyLocations;
        }

        public async Task<List<StudentModel>> GetSheduledInterViews(int jobId)
        {
            List<StudentModel> studentModels=(from i in dbContext.Interviews
                                              where i.AppliedJob.Job.Id==jobId
                                              where i.InterViewDate.Date>=DateTime.Today.Date
                                              select new StudentModel
                                              {
                                                  Resume = i.AppliedJob.User.Resume,
                                                  FullName = i.AppliedJob.User.FullName,
                                                  studentskills = (from sk in dbContext.StudentSkills
                                                                   where sk.user.Id == i.AppliedJob.User.Id
                                                                   select sk.skill.Name).ToList(),
                                                  PhoneNumber = i.AppliedJob.User.PhoneNumber,
                                                  Email = i.AppliedJob.User.Email,
                                                  InterViewDate = i.InterViewDate.ToString("g"),
                                                  InterViewMode = i.InterViewMode.ToString(),
                                                  InterViewLocation = i.InterViewLocation == null ? "" : i.InterViewLocation,

                                              }).ToList();
            return studentModels;
        }

        public async Task<List<StudentModel>> GetStudentsAppliedForJob(int jobId)
        { 
            List<string> interviews=await(from i in dbContext.Interviews where i.AppliedJob.Job.Id==jobId select i.AppliedJob.User.Id).ToListAsync();
            List<StudentModel> appliedJobs=(from Aj in dbContext.AppliedJobs where Aj.Job.Id ==jobId 
                                            && !Aj.User.DeleteStatus 
                                            && !Aj.Job.DeleteStatus 
                                            && !interviews.Contains(Aj.User.Id)
                                            select new StudentModel
                                            {
                                                AppliedId=Aj.Id, 
                                                StudentId=Aj.User.Id,
                                                Resume=Aj.User.Resume,
                                                FullName=Aj.User.FullName,
                                                studentskills=(from sk in dbContext.StudentSkills where sk.user.Id==Aj.User.Id
                                                               select sk.skill.Name).ToList(),
                                                PhoneNumber=Aj.User.PhoneNumber,
                                                Email=Aj.User.Email,
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
                                                && !studentSkillGroup.Key.DeleteStatus
                                                && !dbContext.AppliedJobs.Any(ja => ja.User.Id == studentSkillGroup.Key.Id && ja.Job.Id == jobId)
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
            dbContext.SaveChanges();
            await CreateNotification(user, company1);
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

        public async Task<int> ScheduleInterview(InterViewModel interViewModel)
        {
            var  applied=(from ap in dbContext.AppliedJobs where ap.Id==interViewModel.AppliedId
                          select new
                          {
                              appliedJob=ap,
                              Title=ap.Job.Title,
                              userId=ap.User.Id
                          }).FirstOrDefault();
            InterViewMode interViewMode;
            Interview interview = new Interview()
            {
                InterViewDate = interViewModel.InterViewDate.AddHours(18),
                AppliedJob = applied.appliedJob
            };
            Enum.TryParse<InterViewMode>(interViewModel.InterViewMode, true,out interViewMode);
            interview.InterViewMode = interViewMode;
            if(interViewModel.InterViewLocation != null)
            {
                interview.InterViewLocation = interViewModel.InterViewLocation;
            }
          await  dbContext.Interviews.AddAsync(interview);
          await dbContext.SaveChangesAsync();
          string msg = $"Dear Applicant for the job role {applied.Title} you applied an InterView is Scheduled.You Can Check About in your Profile";
            notificationRepository.CreateNotification(msg, applied.userId);
          return interview.Id;
        }

        private async Task CreateNotification(ApplicationUser user,Company company)
        {
            string msg = $"MrorMs {user.FullName} have Registered their {company.Name} Company  to our Website Please Verify the Company in Unverified Section";
            string adminId=await (from ur in dbContext.UserRoles 
                                   join r in dbContext.Roles on ur.RoleId equals r.Id
                                   where r.Name=="admin" select ur.UserId
                                   ).FirstOrDefaultAsync();
            notificationRepository.CreateNotification(adminId, msg);
        }
    }
}
