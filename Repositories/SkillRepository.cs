using System;
using System.Linq;
using System.Threading.Tasks;
using JobPortal.Entities;

namespace JobPortal.Repositories
{
    public class SkillRepository : ISkillRespository
    {
        private readonly ApplicationDbContext dbcontext;
        public SkillRepository(ApplicationDbContext dbContext) {
            this.dbcontext = dbcontext;
        }

        public async Task<Skills> GetSKill(string skillname)
        {
          Skills skill=(from sk in dbcontext.Skills where sk.Name == skillname select sk).FirstOrDefault<Skills>();
            return skill;
        }

        public async Task<Skills> InsertSkill(string skillname)
        {
            Skills skill = new Skills();
            skill.Name = skillname;
            dbcontext.Skills.Add(skill);
            dbcontext.SaveChanges();
            return skill;

        }
        
    }
}
