using Microsoft.AspNetCore.Mvc;
using Jat.IServices;
using Jat.DTOs;

namespace Jat.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var jobs = await _jobService.GetAllJobsAsync(pageNumber, pageSize);
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(long id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound();
            return Ok(job);
        }

        [HttpPost]
        public async Task<IActionResult> AddJob([FromBody] JobDto jobDto)
        {
            await _jobService.AddJobAsync(jobDto);
            return CreatedAtAction(nameof(GetJobById), new { id = jobDto.Id }, jobDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(long id, [FromBody] JobDto jobDto)
        {
            await _jobService.UpdateJobAsync(id, jobDto);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateJobStatus(long id, [FromBody] JobStatus status)
        {
            await _jobService.UpdateJobStatusAsync(id, status);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(long id)
        {
            await _jobService.DeleteJobAsync(id);
            return NoContent();
        }
    }
}
