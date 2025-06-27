using Microsoft.EntityFrameworkCore;
using Cefalo.Csharp.Core.Entities;

namespace Cefalo.Csharp.Infrastructure.Data;

public class TaskManagementDbContext : DbContext
{
    public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Task configuration
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.Priority).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Tasks)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Seed data
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
            new User { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com" }
        );

        modelBuilder.Entity<TaskItem>().HasData(
            new TaskItem
            {
                Id = 1,
                Title = "Complete project documentation",
                Description = "Write comprehensive documentation for the new feature",
                Status = Cefalo.Csharp.Core.Entities.TaskStatus.Todo,
                Priority = Cefalo.Csharp.Core.Entities.TaskPriority.High,
                CreatedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(7),
                UserId = 1
            },
            new TaskItem
            {
                Id = 2,
                Title = "Review code changes",
                Description = "Review pull request #123 for the authentication module",
                Status = Cefalo.Csharp.Core.Entities.TaskStatus.InProgress,
                Priority = Cefalo.Csharp.Core.Entities.TaskPriority.Medium,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                DueDate = DateTime.UtcNow.AddDays(2),
                UserId = 2
            }
        );
    }
}