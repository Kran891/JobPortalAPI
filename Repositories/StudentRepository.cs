using JobPortal.Entities;
using JobPortal.Models;
using System.Collections.Generic;

namespace JobPortal.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ISkillRespository skillRespository;
        public StudentRepository(ApplicationDbContext dbContext, ISkillRespository skillRespository)
        {
            this.dbContext = dbContext;
            this.skillRespository = skillRespository;
        }
        public async Task<ApplicationUser> InsertStudentDetails(StudentModel studentModel)
        {
            ApplicationUser user = (from u in dbContext.Users where u.Id == studentModel.Id select u).FirstOrDefault<ApplicationUser>();
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
