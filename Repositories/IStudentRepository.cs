using JobPortal.Entities;
using JobPortal.Models;

namespace JobPortal.Repositories
{
    public interface IStudentRepository
    {
        Task<ApplicationUser> InsertStudentDetails(StudentModel studentModel);
    }
}
