using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskTracker.Core.Extensions;
using TaskTracker.Core.Exceptions;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Repositories;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository<UserRepository>, IUserRepository
    {
        public UserRepository(TaskDbContext context, ILogger<UserRepository> logger) : base(context, logger)
        {
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await GetQueryable<User>()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await GetQueryable<User>()
                .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await GetQueryable<User>()
                .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await GetQueryable<User>()
                .AnyAsync(x => x.Username == username, cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await GetQueryable<User>()
                .AnyAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            _context.Users.Add(user);
            await SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            var existingUser = await GetQueryable<User>(asNoTracking: false)
                .FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);

            if (existingUser == null)
            {
                throw new NotFoundException($"Kullanıcı (ID: {user.Id}) bulunamadı");
            }

            _context.Users.Update(user);
            await SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await GetQueryable<User>(asNoTracking: false)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException($"Kullanıcı (ID: {id}) bulunamadı");
            }

            _context.Users.Remove(user);
            await SaveChangesAsync(cancellationToken);
        }
    }
} 