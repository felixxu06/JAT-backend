using Jat.Entities;

namespace Jat.IRepositories
{
    public interface IApplicantRepository
    {
        Task<IEnumerable<Applicant>> GetAllAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Applicant>> GetAllByJobIdAsync(Int64 jobId);
        Task<Applicant?> GetByIdAsync(long id);
        Task AddAsync(Applicant applicant);
        Task UpdateAsync(long id, Applicant applicant);
        Task DeleteAsync(long id);
        Task<int> GetTotalCountAsync();
    }
}