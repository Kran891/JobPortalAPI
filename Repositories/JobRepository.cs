using JobPortal.Entities;
using JobPortal.Models;

namespace JobPortal.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext dbContext;

        public JobRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task InsertStudentDetails(StudentModel studentModel)
        {
            ApplicationUser user = dbContext.Users.FirstOrDefault(u => u.Id == studentModel.CompanyName);

            if (user != null)
            {
                Job job = new Job
                {
                    Description = user
                };
                Company company = new Company();
                dbContext.Companies.Add(company);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    internal class Job
    {
        public ApplicationUser Description { get; set; }
    }
}
