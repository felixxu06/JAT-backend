using System.Linq;
using Jat.Entities;
using Jat.IRepositories;

namespace Jat.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly UserContext _userContext;
        private readonly InMemoryDatabase _db;

        public JobRepository(UserContext userContext, InMemoryDatabase db)
        {
            _userContext = userContext;
            _db = db;
        }

        public Task<IEnumerable<Job>> GetAllAsync(int pageNumber, int pageSize)
        {
            var result = _db.Jobs.Values.Where(j => !j.Deleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();
            return Task.FromResult(result);
        }

        public Task<Job?> GetByIdAsync(long id)
        {
            _db.Jobs.TryGetValue(id, out var job);
            return Task.FromResult(job != null && !job.Deleted ? job : null);
        }

        public Task AddAsync(Job job)
        {
            job.Id = _db.Jobs.Count == 0 ? 1 : _db.Jobs.Keys.Max() + 1;
            job.CreatedBy = _userContext.CurrentUser?.Identity?.Name ?? "Unknown";
            job.CreatedAt = DateTime.UtcNow;
            _db.Jobs[job.Id] = job;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(long id, Job job)
        {
            if (_db.Jobs.ContainsKey(id))
            {
                job.Id = id;
                job.UpdatedBy = _userContext.CurrentUser?.Identity?.Name ?? "Unknown";
                job.UpdatedAt = DateTime.UtcNow;
                _db.Jobs[id] = job;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(long id)
        {
            if (_db.Jobs.TryGetValue(id, out var job))
            {
                job.Deleted = true;
                job.UpdatedBy = _userContext.CurrentUser?.Identity?.Name ?? "Unknown";
                job.UpdatedAt = DateTime.UtcNow;
                _db.Jobs[id] = job;
            }
            return Task.CompletedTask;
        }

        public Task<int> GetTotalCountAsync()
        {
            var totalCount = _db.Jobs.Values.Count(j => !j.Deleted);
            return Task.FromResult(totalCount);
        }
    }
}