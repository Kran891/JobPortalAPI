using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Entities
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyLocation> CompanyLocations { get; set; }
        public DbSet<AppliedJobs> AppliedJobs { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Jobs> Jobs { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<JobSkills> JobSkills { get; set; }
        public DbSet<UserEducation> UserEducations { get; set;}
        public DbSet<UserSkills> UserSkills { get; set; }
        public DbSet<Location> Location { get; set; }

        public DbSet<StudentSkills> StudentSkills { get; set; }

    }
}
