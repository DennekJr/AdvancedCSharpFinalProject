using AdvancedCFinalProject.Models;
using Microsoft.AspNetCore.Identity;
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        SeedUsers(builder);
    }
    private static void SeedRoles(ModelBuilder builder)
    {

    }

    private static void SeedUsers(ModelBuilder builder)
    {
        var adminRoleId = Guid.NewGuid().ToString();
        var normalUserRoleId = Guid.NewGuid().ToString();

        builder.Entity<IdentityRole>().HasData(
            new List<IdentityRole>()
            {
                    new ()
                    {
                        Id =adminRoleId,
                        Name = "Administrator",
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        NormalizedName = "Administrator".ToUpper()
                    },
                    new ()
                    {
                        Id =normalUserRoleId,
                        Name = "User",
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        NormalizedName = "User".ToUpper()
                    }
            }
        );

        var adminId = Guid.NewGuid().ToString();
        var normalUser1Id = Guid.NewGuid().ToString();
        var normalUser2Id = Guid.NewGuid().ToString();

        builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = adminId,
                Email = "admin@email.com",
                NormalizedEmail = "ADMIN@EMAIL.COM",
                UserName = "admin@email.com",
                NormalizedUserName = "ADMIN@EMAIL.COM",
                Reputation = 0,
                PasswordHash = "AQAAAAEAACcQAAAAEDs30VwJyCURbN7HfxfYOXXeqBL/STAHcgtbTNsugTWo2C1EmOgSreoFYg0uDM2w0w==",
                SecurityStamp = "5GUYLKEWMP4GZZ5UXZNO2JAPY5IHQVP3",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
            }
        );

        builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = normalUser1Id,
                Email = "normal1@email.com",
                NormalizedEmail = "NORMAL1@EMAIL.COM",
                UserName = "normal1@email.com",
                NormalizedUserName = "NORMAL1@EMAIL.COM",
                Reputation = 0,
                PasswordHash = "AQAAAAEAACcQAAAAEDs30VwJyCURbN7HfxfYOXXeqBL/STAHcgtbTNsugTWo2C1EmOgSreoFYg0uDM2w0w==",
                SecurityStamp = "5GUYLKEWMP4GZZ5UXZNO2JAPY5IHQVP3",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
            }
        );

        builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = normalUser2Id,
                Email = "normal2@email.com",
                NormalizedEmail = "NORMAL2@EMAIL.COM",
                UserName = "normal2@email.com",
                NormalizedUserName = "NORMAL2@EMAIL.COM",
                Reputation = 0,
                PasswordHash = "AQAAAAEAACcQAAAAEDs30VwJyCURbN7HfxfYOXXeqBL/STAHcgtbTNsugTWo2C1EmOgSreoFYg0uDM2w0w==",
                SecurityStamp = "5GUYLKEWMP4GZZ5UXZNO2JAPY5IHQVP3",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
            }
        );

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminId
            }
        );
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = normalUserRoleId,
                UserId = normalUser1Id
            }
        );
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = normalUserRoleId,
                UserId = normalUser2Id
            }
        );
    }
}

