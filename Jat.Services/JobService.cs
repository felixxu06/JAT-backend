using Jat.DTOs;
using Jat.IServices;
using Jat.IRepositories;
using Jat.Entities;
using DTOJobStatus = Jat.DTOs.JobStatus;
using EntityJobStatus = Jat.Entities.JobStatus;

namespace Jat.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _repository;
        private readonly IApplicantRepository _applicantRepository;

        public JobService(IJobRepository repository, IApplicantRepository applicantRepository)
        {
            _repository = repository;
            _applicantRepository = applicantRepository;
        }

        public async Task<IEnumerable<JobDto>> GetAllJobsAsync(int pageNumber, int pageSize)
        {
            var jobs = await _repository.GetAllAsync(pageNumber, pageSize);
            return jobs.Select(job => new JobDto
            {
                Id = job.Id,
                CompanyName = job.CompanyName,
                Position = job.Position,
                Status = Enum.TryParse<DTOJobStatus>(job.Status.ToString(), out var status) ? status : null,
                DateApplied = job.DateApplied
            });
        }

        public async Task<JobDto?> GetJobByIdAsync(long id)
        {
            var job = await _repository.GetByIdAsync(id);
            if (job == null) return null;
            return new JobDto
            {
                Id = job.Id,
                CompanyName = job.CompanyName,
                Position = job.Position,
                Status = Enum.TryParse<DTOJobStatus>(job.Status.ToString(), out var status) ? status : null,
                DateApplied = job.DateApplied
            };
        }

        public async Task AddJobAsync(JobDto jobDto)
        {
            var job = new Job
            {
                CompanyName = jobDto.CompanyName,
                Position = jobDto.Position,
                Status = jobDto.Status.HasValue && Enum.TryParse<EntityJobStatus>(jobDto.Status.ToString(), out var status) ? status : EntityJobStatus.Open,
                DateApplied = jobDto.DateApplied
            };
            await _repository.AddAsync(job);
        }

        public async Task UpdateJobAsync(long id, JobDto jobDto)
        {
            var job = new Job
            {
                Id = id,
                CompanyName = jobDto.CompanyName,
                Position = jobDto.Position,
                Status = jobDto.Status.HasValue && Enum.TryParse<EntityJobStatus>(jobDto.Status.ToString(), out var status) ? status : EntityJobStatus.Open,
                DateApplied = jobDto.DateApplied
            };
            await _repository.UpdateAsync(id, job);
        }

        public async Task DeleteJobAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task UpdateJobStatusAsync(long id, DTOJobStatus status)
        {
            var job = await _repository.GetByIdAsync(id);
            if (job == null)
            {
                throw new KeyNotFoundException($"Job with ID {id} not found.");
            }
            job.Status = Enum.TryParse<EntityJobStatus>(status.ToString(), out var entityStatus) ? entityStatus : EntityJobStatus.Open;
            job.UpdateTimestamp();
            // enable fake transaction
            try
            {
                if (status == DTOJobStatus.Closed)
                {
                    // If the job is closed, we might want to update the status of all applicants
                    var applicants = await _applicantRepository.GetAllByJobIdAsync(id);
                    foreach (var applicant in applicants)
                    {
                        applicant.Status = Entities.ApplicantStatus.Rejected; // Assuming you have a Closed status in your ApplicantStatus enum
                        await _applicantRepository.UpdateAsync(applicant.Id, applicant);
                    }
                }
                await _repository.UpdateAsync(id, job); 
            }
            catch (Exception ex)
            {
                // TODO
                // rollback transaction
                throw new InvalidOperationException($"Failed to update job status for job ID {id}.", ex);
            }
        }
    }
}