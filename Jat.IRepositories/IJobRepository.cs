using Jat.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jat.IRepositories
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetAllAsync(int pageNumber, int pageSize);
        Task<Job?> GetByIdAsync(long id);
        Task AddAsync(Job job);
        Task UpdateAsync(long id, Job job);
        Task DeleteAsync(long id);
    }
}