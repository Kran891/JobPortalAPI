using JobPortal.Entities;

namespace JobPortal.Repositories
{
    public interface ISkillRespository
    {
        Task<Skills> InsertSkill(string skillname);
        Task<Skills> GetSKill(string skillname);
    }
}
