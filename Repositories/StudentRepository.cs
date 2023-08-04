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

        public StudentRepository(ApplicationDbContext dbContext, ISkillRepository skillRespository, ICompanyRepository companyRepository, ILocationRepository locationRepository)
        {
            this.dbContext = dbContext;
            this.skillRespository = skillRespository;
            this.companyRepository = companyRepository;
            this.locationRepository = locationRepository;
        }

        public async Task<int> ApplyJob(int jobId, string userId)
        {
            AppliedJobs appliedJob = (from aj in dbContext.AppliedJobs where aj.User.Id == userId select aj).FirstOrDefault();
            if (appliedJob != null)
            {
                ApplicationUser user = dbContext.Users.FirstOrDefault(x => x.Id == userId);
                Jobs job = dbContext.Jobs.FirstOrDefault(x => x.Id == jobId);
                appliedJob = new AppliedJobs()
                {
                    User = user,
                    Job = job
                };
                dbContext.AppliedJobs.Add(appliedJob);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            return -1;
        }

        public async Task<List<JobModel>> GetAllJobs(string userid)
        {
            List<JobModel> jobModels = await GetJobsByYourSkills(userid);

            List<string> preferredLocations = await (from pl in dbContext.PreferredLocations
                                                     where pl.User.Id == userid
                                                     select pl.Location.Name).ToListAsync();

            if (preferredLocations != null && preferredLocations.Any())
            {
                jobModels = jobModels.Where(jm => jm.Locations.Intersect(preferredLocations).Any()).ToList();
            }

            if (jobModels == null || jobModels.Count == 0) 
            {
                jobModels = (
                    from j in dbContext.Jobs
                    join c in dbContext.Companies on j.Company.Id equals c.Id
                    select new JobModel
                    {
                        JobId = j.Id,
                        Title = j.Title,
                        Description = j.Description,
                        CompanyId = c.Id,
                        CompanyName = c.Name,
                        Salary = j.Salary,
                        RequiredSkills = (
                            from js in dbContext.JobSkills
                            where js.job.Id == j.Id
                            select js.Skill.Name
                        ).ToList()
                    }
                ).ToList();
            }

            return jobModels;
        }


        public async Task<List<JobModel>> GetAppliedJobs(string userid)
        {
            List<JobModel> appliedJobs = (from aj in dbContext.AppliedJobs
                                          join c in dbContext.Companies on aj.Job.Company.Id equals c.Id
                                          where aj.User.Id == userid 
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
                                         where i.AppliedJob.User.Id == userid && i.InterViewDate.Date >= DateTime.Now.Date
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

        public async Task<List<JobModel>> GetJobsByLocation(string location, string userid)
        {
            List<JobModel> Location = (from l in dbContext.Location
                                       select new JobModel
                                       {
                                           CompanyId = l.Id,
                                           CompanyName=l.Name,
                                           JobId = l.Id,
                                       }
                                       ).ToList();
            return Location;
        }

        public async Task<List<JobModel>> GetJobsByYourSkills(string userid)
        {
            List<string> studentSkills = (from sk in dbContext.StudentSkills where sk.user.Id == userid select sk.skill.Name).ToList();

            List<JobModel> jobModels = (
                                             from jk in dbContext.JobSkills
                                             where studentSkills.Contains(jk.Skill.Name)
                                             group jk by jk.job into jobSkillGroup
                                             where jobSkillGroup.Count() >= 2
                                             select new JobModel
                                             {
                                                 JobId = jobSkillGroup.Key.Id,
                                                 Title = jobSkillGroup.Key.Title,
                                                 Description = jobSkillGroup.Key.Description,
                                                 CompanyId = jobSkillGroup.Key.Company.Id,
                                                 CompanyName = jobSkillGroup.Key.Company.Name,
                                                 Salary = jobSkillGroup.Key.Salary,
                                                 RequiredSkills = jobSkillGroup.Select(js => js.Skill.Name).ToList()
                                             }
                                         ).ToList();
            return jobModels;
        }

        public async Task<ApplicationUser> InsertStudentDetails(StudentModel studentModel)
        {
            ApplicationUser user = (from u in dbContext.Users where u.Id == studentModel.StudentId select u).FirstOrDefault<ApplicationUser>();
            user.Resume = studentModel.Resume;
            dbContext.Users.Add(user);
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
            foreach (string place in studentModel.preferredLocations)
            {
                
            }
            dbContext.StudentSkills.AddRange(studentSkills);
            dbContext.SaveChanges();
            return user;
        }
    }
}
