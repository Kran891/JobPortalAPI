using JobPortal.Models;
using System.Threading.Tasks;

namespace JobPortal.Repositories
{
    public interface IJobRepository
    {
        Task InsertStudentDetails(StudentModel studentModel);

    }
}
