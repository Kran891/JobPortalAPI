using JobPortal.Entities;
using JobPortal.Models;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace JobPortal.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ISkillRespository skillRespository;
        private readonly ICompanyRepository companyRepository;

        public StudentRepository(ApplicationDbContext dbContext, ISkillRespository skillRespository, ICompanyRepository companyRepository)
        {
            this.dbContext = dbContext;
            this.skillRespository = skillRespository;
            this.companyRepository = companyRepository;
        }

        public Task<int> ApplyJob(int jobId)
        {
            throw new NotImplementedException();
        }

        public Task<List<JobModel>> GetAllJobs(string userid)
        {
            throw new NotImplementedException();
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

        public Task<List<JobModel>> GetJobsByYourSkills(string userid)
        {
            throw new NotImplementedException();
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
                Skills cskill = await skillRespository.GetSKill(skill);
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
            
            foreach (string place in studentModel.preferredLocations)
            {

            }
            dbContext.StudentSkills.AddRange(studentSkills);
            dbContext.SaveChanges();
            return user;
        }
    }
}
