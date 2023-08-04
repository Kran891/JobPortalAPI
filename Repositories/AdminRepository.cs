

using JobPortal.Entities;
using JobPortal.Models;

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
                                                where c.Status == true
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

        public Task<List<CompanyModel>> GetJobPostedToday()
        {
            throw new NotImplementedException();
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
