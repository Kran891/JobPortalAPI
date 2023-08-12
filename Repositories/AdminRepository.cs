

using JobPortal.Entities;
using JobPortal.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Buffers.Text;

namespace JobPortal.Repositories
{
    public class AdminRepository : IAdminRepository
    {

        private readonly ApplicationDbContext dbContext;

        public AdminRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public async Task<List<CompanyModel>> GetAllUnverifiedCompanies()
        {
            List<CompanyModel> companyModels = (from c in dbContext.Companies
                                               where c.Status == false
                                               select new CompanyModel
                                               {

                                                 CompanyId = c.Id,
                                                   Name = c.Name,
                                                   Description = c.Description,
                                                   OwnerId= c.Owner.Id,
                                                   OwnerName=c.Owner.FullName,

                                                   CompanyLocations=(from cl in dbContext.CompanyLocations
                                                                     where cl.Company.Id == c.Id 
                                                                     select cl.Location.Name).ToList()
                                               }).ToList() ;

            
            return companyModels;
           // throw new NotImplementedException();
        }

        public async Task<List<CompanyModel>> GetCompanies()
        {
            List<CompanyModel> companyModels = (from c in dbContext.Companies
                                                where c.Status 
                                                && !c.DeleteStatus
                                                select new CompanyModel
                                                {

                                                    CompanyId = c.Id,
                                                    Name = c.Name,
                                                    Description = c.Description,
                                                    OwnerId = c.Owner.Id,
                                                    OwnerName = c.Owner.FullName,

                                                    CompanyLocations = (from cl in dbContext.CompanyLocations
                                                                        where cl.Company.Id == c.Id
                                                                        select cl.Location.Name).ToList()
                                                }).ToList();


            return companyModels;
            throw new NotImplementedException();
        }

        public async Task<List<JobModel>> GetJobPostedToday()
        {
            DateTime today = DateTime.Today;

            List<JobModel> jobsPostedToday = await(
                from job in dbContext.Jobs
                where job.PostedDate.Date == today.Date && !job.DeleteStatus
                && !job.DeleteStatus && !job.Company.DeleteStatus
                select new JobModel
                {
                    JobId=job.Id,
                    Title = job.Title,
                    Description = job.Description,
                    CompanyName = job.Company.Name,
                    Salary = job.Salary,
                    RequiredSkills = (from rs in dbContext.JobSkills where job.Id == rs.job.Id select rs.Skill.Name).ToList(),
                    //RequiredSkills = job.RequiredSkills, 
                   // DeleteStatus = job.DeleteStatus,
                    PostedDate = job.PostedDate
                }
            ).ToListAsync();

            return jobsPostedToday;
        }
    
  

public async Task<int> VerifyCompany(int CompanyId)
        {
            Company company = (from c in dbContext.Companies
                               where c.Id == CompanyId
                               select c).FirstOrDefault();
            if (company != null)
            {
                company.Status = true;
                dbContext.Companies.Update(company);
                dbContext.SaveChanges();
                return 1;

            }
            return -1;
            
        }
    }
}
