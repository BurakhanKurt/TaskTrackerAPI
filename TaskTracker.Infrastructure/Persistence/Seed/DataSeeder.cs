using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Helpers;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.Persistence.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(TaskDbContext context)
        {
            // Seed Users
            if (!await context.Users.AnyAsync())
            {
                var users = new List<(User User, string Password)>
                {
                    (new User
                    {
                        Username = "admin",
                        Email = "admin@tasktracker.com",
                        FirstName = "Admin",
                        LastName = "User",
                        CreatedAt = DateTime.UtcNow
                    }, "Admin123!"),
                };

                var passwordHelper = new PasswordHelper();
                foreach (var (user, password) in users)
                {
                    var (passwordHash, passwordSalt) = passwordHelper.HashPassword(password);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    context.Users.Add(user);
                }
                await context.SaveChangesAsync();
            }

            // Seed Tasks
            if (!await context.Tasks.AnyAsync())
            {
                var tasks = new List<TaskItem>
                {
                    new TaskItem
                    {
                        IsCompleted = false,
                        UserId = 1,
                        CreatedAt = DateTime.UtcNow
                    },
                    new TaskItem
                    {
                        IsCompleted = true,
                        UserId = 1,
                        CreatedAt = DateTime.UtcNow,
                        DueDate = DateTime.UtcNow.AddDays(7)
                    }
                };
                tasks[0].SetTitle("Proje planlaması yap");
                tasks[1].SetTitle("API dokümantasyonu hazırla");
                context.Tasks.AddRange(tasks);
                await context.SaveChangesAsync();
            }
        }
    }
}