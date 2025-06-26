using Jat.DTOs;
namespace Jat.IServices
{
    public interface IJobService
    {
        Task<(IEnumerable<JobDto> Jobs, int TotalCount, int TotalPages)> GetAllJobsAsync(int pageNumber, int pageSize);
        Task<JobDto?> GetJobByIdAsync(long id);
        Task AddJobAsync(JobDto jobDto);
        Task UpdateJobAsync(long id, JobDto jobDto);
        Task DeleteJobAsync(long id);
        Task UpdateJobStatusAsync(long id, JobStatus status);
    }
}