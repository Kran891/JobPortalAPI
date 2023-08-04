using JobPortal.Entities;

namespace JobPortal.Repositories
{
    public interface ISkillRepository
    {
        Task<Skills> InsertSkill(string skillname);
        Task<Skills> GetSkill(string skillname);
        Task<List<string>> GetAllSkillsAsync();
    }
}
