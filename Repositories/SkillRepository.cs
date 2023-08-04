using System;
using System.Linq;
using System.Threading.Tasks;
using JobPortal.Entities;
using Microsoft.AspNetCore.Mvc;

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
            List<string> skillnames= ( from sk in dbcontext.Skills
                                       where sk.user.Id == userid
                                       ).ToList();
        }

        public async Task<Skills> GetSKill(string skillname)
        {
          Skills skill=(from sk in dbcontext.Skills where sk.Name == skillname select sk).FirstOrDefault<Skills>();
            return skill;
        }

        public Task<Skills> GetSkill(string skillname)
        {
            throw new NotImplementedException();
        }

        public Task<Skills> InsertSkill(string skillname)
        {
            throw new NotImplementedException();
        }

        

        
        public async Task<List<string>> GetAllSkillsAsync()
        {
            List<string> skillNames = await dbcontext.Skills
                                                 .Select(skill => skill.Name)
                                                 .ToListAsync();
            return skillNames;
        }



    }

    
}
