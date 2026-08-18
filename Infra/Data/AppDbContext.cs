using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Identity.Client;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Company>().HasData(
        new Company { CompanyId = 1, Name = "Cube AI" },
        new Company { CompanyId = 2, Name = "Instabug" },
        new Company { CompanyId = 3, Name = "ITWorx" },
        new Company { CompanyId = 4, Name = "Valeo" }
    );

    modelBuilder.Entity<User>().HasData(
        new User { UserId = 1, Name = "Mahmoud Mostafa", Email = "mahmoud@cube.ai" },
        new User { UserId = 2, Name = "Omar Ali", Email = "omar.ali@example.com" },
        new User { UserId = 3, Name = "Sara Hassan", Email = "sara.h@example.com" }
    );
}
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<Interview> Interviews { get; set; }
}
