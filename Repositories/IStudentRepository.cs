using JobPortal.Entities;

namespace JobPortal.Repositories
{
    public interface IStudentRepository
    {
        Task<ApplicationUser> InsertStudentDetails();
    }
}
