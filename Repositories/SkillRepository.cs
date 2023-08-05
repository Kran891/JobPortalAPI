using System;
using System.Linq;
using System.Threading.Tasks;
using JobPortal.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly ApplicationDbContext dbcontext;
        public SkillRepository(ApplicationDbContext dbContext) {
            this.dbcontext = dbcontext;
        }

        public async Task<List<string>> GetAllSkills(string userid)
        {
            List<string> skillnames= ( from sk in dbcontext.StudentSkills
                                       where sk.user.Id == userid select sk.skill.Name

                                       ).ToList();
            return skillnames;
        }

        public async Task<Skills> GetSkill(string skillname)
        {
          Skills skill=(from sk in dbcontext.Skills where sk.Name == skillname select sk).FirstOrDefault<Skills>();
            return skill;
        }

        

        

        

        
        public async Task<List<string>> GetAllSkillsAsync()
        {
            List<string> skillNames = await dbcontext.Skills
                                                 .Select(skill => skill.Name)
                                                 .ToListAsync();
            return skillNames;
        }

        public async Task<Skills> InsertSkill(string skillname)
        {
            Skills skill = new Skills()
            {
              Name = skillname
            };
            dbcontext.Skills.Add(skill);
            dbcontext.SaveChanges();
            return skill;
        }
    }

    
}
