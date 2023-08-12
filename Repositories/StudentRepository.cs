using JobPortal.Entities;
using JobPortal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace JobPortal.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ISkillRepository skillRespository;
        private readonly ICompanyRepository companyRepository;
        private readonly ILocationRepository locationRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly string uploadsFolder;

        public StudentRepository(IWebHostEnvironment webHostEnvironment, ApplicationDbContext dbContext,INotificationRepository notificationRepository, ISkillRepository skillRespository, ICompanyRepository companyRepository, ILocationRepository locationRepository)
        {
            this.dbContext = dbContext;
            this.skillRespository = skillRespository;
            this.companyRepository = companyRepository;
            this.locationRepository = locationRepository;
            this.notificationRepository = notificationRepository;
            this.uploadsFolder = Path.Combine(webHostEnvironment.ContentRootPath, "uploads");
        }

        public async Task<int> ApplyJob(int jobId, string userId)
        {
            AppliedJobs appliedJob = (from aj in dbContext.AppliedJobs where aj.User.Id == userId select aj).FirstOrDefault();
            if (appliedJob != null)
            {
                ApplicationUser user = dbContext.Users.FirstOrDefault(x => x.Id == userId);
                var job = (from j in dbContext.Jobs
                           where j.Id == jobId
                           select new
                           {
                               job=j,
                               ownerId=j.Company.Owner.Id
                           }).FirstOrDefault();
                appliedJob = new AppliedJobs()
                {
                    User = user,
                    Job = job.job
                };
                dbContext.AppliedJobs.Add(appliedJob);
                await dbContext.SaveChangesAsync();
                string msg = $"An Applicant have Applied for the role {job.job.Title}.You Can check his profile Profile & Proceed Further";
                notificationRepository.CreateNotification(msg, job.ownerId);
                return 1;
            }
            return -1;
        }

        public async Task<List<JobModel>> GetAllJobs(string userid)
        {
            List<JobModel> jobModels = await GetJobsByYourSkills(userid);
            List<JobModel> jobModels1;
            List<string> preferredLocations = await (from pl in dbContext.PreferredLocations
                                                     where pl.User.Id == userid
                                                     select pl.Location.Name).ToListAsync();

            if (preferredLocations != null && preferredLocations.Any())
            {
                jobModels1 = jobModels.Where(jm => jm.Locations.Intersect(preferredLocations).Any()).ToList();
                if (jobModels1.Count>1)
                {
                    return jobModels1;
                }
            }
            

            return jobModels;
        }


        public async Task<List<JobModel>> GetAppliedJobs(string userid)
        {
            List<JobModel> appliedJobs = (from aj in dbContext.AppliedJobs
                                          join c in dbContext.Companies on aj.Job.Company.Id equals c.Id
                                          where aj.User.Id == userid 
                                          && !aj.Job.DeleteStatus && !c.DeleteStatus
                                          select new JobModel
                                          {
                                              JobId=aj.Job.Id,
                                              Title=aj.Job.Title,
                                              Description=aj.Job.Description,
                                              CompanyId=c.Id,
                                              CompanyName=c.Name,
                                              Salary=aj.Job.Salary,
                                              NoOfApplicants = aj.Id,
                                              RequiredSkills = (from rs in dbContext.JobSkills where rs.job.Id == aj.Job.Id select rs.Skill.Name).ToList()

                                          }
                                          ).ToList();
            return appliedJobs;
        }

        public async Task<List<JobModel>> GetInterviewsScheduled(string userid)
        {
            List<JobModel> interviews = (from i in dbContext.Interviews
                                         join c in dbContext.Companies on i.AppliedJob.Job.Company.Id equals c.Id
                                        
                                         where !i.AppliedJob.Job.DeleteStatus && !c.DeleteStatus &&
                                         i.AppliedJob.User.Id == userid && i.InterViewDate.Date >= DateTime.Now.Date
                                         select new JobModel
                                         {
                                             JobId = i.AppliedJob.Job.Id,
                                             InterViewDate = i.InterViewDate,
                                             InterViewMode = i.InterViewMode.ToString(),
                                             CompanyName = c.Name,
                                             CompanyId = c.Id,
                                             Title = i.AppliedJob.Job.Title,
                                             Description = i.AppliedJob.Job.Description,
                                             Salary = i.AppliedJob.Job.Salary,
                                             Locations = (from cl in dbContext.CompanyLocations where cl.Company.Id == c.Id select cl.Location.Name).ToList()
                                         }
                                         ).ToList();
            return interviews;

        }
        private string UploadFile(IFormFile file)
        {
            if(file == null || file.Length==0) {
                return "";
            }
            var fileName=Guid.NewGuid()+Path.GetExtension(file.FileName);
            var filepath = Path.Combine(uploadsFolder, fileName);
            using(var stream=new FileStream(filepath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }
        public async Task<List<JobModel>> GetJobsByLocation(string location, string userid)
        {
            List<JobModel> jobModels = await GetJobsByYourSkills(userid);
            List<JobModel> filteredJobs = jobModels
            .Where(job => job.Locations.Contains(location))
            .ToList();
            return filteredJobs;
        }

        public async Task<List<JobModel>> GetJobsByYourSkills(string userid)
        {
            List<string> studentSkills = (from sk in dbContext.StudentSkills where sk.user.Id == userid select sk.skill.Name).ToList();
            List<int> appliedJobIds = (from aj in dbContext.AppliedJobs where aj.User.Id == userid select aj.Job.Id).ToList();
             List<JobModel> jobModels = (
                            from j in dbContext.Jobs 
                            where !appliedJobIds.Contains(j.Id)
                            && !j.DeleteStatus && !j.Company.DeleteStatus
                            select new JobModel
                            {
                                JobId = j.Id,
                                Title = j.Title,
                                Description = j.Description,
                                CompanyId = j.Company.Id,
                                CompanyName = j.Company.Name,
                                Salary = j.Salary,
                                RequiredSkills = (from js in dbContext.JobSkills where js.job.Id==j.Id select js.Skill.Name).ToList(),
                                NoOfApplicants=(from ap in dbContext.AppliedJobs where ap.Job.Id==j.Id select ap.Id).ToList().Count(),
                                Locations=(from cl in dbContext.CompanyLocations where cl.Company.Id== j.Company.Id select cl.Location.Name).ToList(),
                            }
                        ).ToList();
    
            return jobModels;
        }

        public async Task<int> InsertSkill(string skillName, string userId)
        {
            Skills skill=await skillRespository.GetSkill(skillName);
            if (skill == null)
                skill = await skillRespository.InsertSkill(skillName.ToLower());
            ApplicationUser user=await dbContext.Users.FirstOrDefaultAsync(x=>x.Id==userId);
            StudentSkills studentSkills=dbContext.StudentSkills.FirstOrDefault(x=>x.skill.Name==skillName.ToLower());
            if(studentSkills == null)
            {
                studentSkills = new StudentSkills()
                {
                    user = user,
                    skill = skill
                };
                dbContext.StudentSkills.Add(studentSkills);
                dbContext.SaveChanges();
                return 1;
            }
            return -1;
        }

        public async Task<ApplicationUser> InsertStudentDetails(StudentModel studentModel)
        {
            
            ApplicationUser user = (from u in dbContext.Users where u.Id == studentModel.StudentId select u).FirstOrDefault<ApplicationUser>();
            user.Resume = UploadFile(studentModel.ResumeFile);
            user.Address = studentModel.Address;
            dbContext.Users.Update(user);
            
            // List<Skills> skills = new List<Skills>();
            List<StudentSkills> studentSkills = new List<StudentSkills>();
            StudentSkills studentSkill;
            foreach (string skill in studentModel.studentskills)
            {
                Skills cskill = await skillRespository.GetSkill(skill);
                if (cskill == null)
                {
                    cskill = await skillRespository.InsertSkill(skill);
                }
                studentSkill = new StudentSkills()
                {
                    user = user,
                    skill = cskill
                };
                studentSkills.Add(studentSkill);


                
               
            }
            List<PreferredLocation> PreferredLocations = new List<PreferredLocation>();
            PreferredLocation preferredLocation;
            foreach (string place in studentModel.preferredLocations)
            {
                Location location=await locationRepository.GetLocation(place);
                if(location == null)
                {
                    location=await locationRepository.InsertLocation(place.ToLower());
                }
                preferredLocation = new PreferredLocation() { Location = location,User=user };
                PreferredLocations.Add(preferredLocation);
            }
            dbContext.PreferredLocations.AddRange(PreferredLocations);
            dbContext.StudentSkills.AddRange(studentSkills);
            dbContext.SaveChanges();
            return user;
        }
    }
}
