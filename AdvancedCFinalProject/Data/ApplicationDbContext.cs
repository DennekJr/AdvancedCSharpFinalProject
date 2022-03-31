using AdvancedCFinalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCFinalProject.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Company> Company { get; set; }
    public DbSet<Project> Project { get; set; }
    public DbSet<Developer> Developer { get; set; }
    public DbSet<DeveloperTask> Tasks { get; set; }
}

