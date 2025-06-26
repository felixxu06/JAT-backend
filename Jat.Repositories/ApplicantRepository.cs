using System.Linq;
using Jat.Entities;
using Jat.IRepositories;

namespace Jat.Repositories
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly UserContext _userContext;
        private readonly InMemoryDatabase _db;

        public ApplicantRepository(UserContext userContext, InMemoryDatabase db)
        {
            _userContext = userContext;
            _db = db;
        }

        public Task<IEnumerable<Applicant>> GetAllAsync(int pageNumber, int pageSize)
        {
            var result = _db.Applicants.Values.Where(a => !a.Deleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();
            return Task.FromResult(result);
        }

        public Task<Applicant?> GetByIdAsync(long id)
        {
            _db.Applicants.TryGetValue(id, out var applicant);
            return Task.FromResult(applicant != null && !applicant.Deleted ? applicant : null);
        }

        public Task AddAsync(Applicant applicant)
        {
            applicant.Id = _db.Applicants.Count == 0 ? 1 : _db.Applicants.Keys.Max() + 1;
            applicant.CreatedBy = _userContext.CurrentUser?.Identity?.Name ?? "Unknown";
            applicant.CreatedAt = DateTime.UtcNow;
            _db.Applicants[applicant.Id] = applicant;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(long id, Applicant applicant)
        {
            if (_db.Applicants.ContainsKey(id))
            {
                applicant.Id = id;
                applicant.UpdatedBy = _userContext.CurrentUser?.Identity?.Name ?? "Unknown";
                applicant.UpdatedAt = DateTime.UtcNow;
                _db.Applicants[id] = applicant;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(long id)
        {
            if (_db.Applicants.TryGetValue(id, out var applicant))
            {
                applicant.Deleted = true;
                applicant.UpdatedBy = _userContext.CurrentUser?.Identity?.Name ?? "Unknown";
                applicant.UpdatedAt = DateTime.UtcNow;
                _db.Applicants[id] = applicant;
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Applicant>> GetAllByJobIdAsync(long jobId)
        {
            return Task.FromResult(_db.Applicants.Values.Where(app=>app.JobId == jobId));
        }

        public Task<int> GetTotalCountAsync()
        {
            var totalCount = _db.Applicants.Values.Count(a => !a.Deleted);
            return Task.FromResult(totalCount);
        }
    }
}