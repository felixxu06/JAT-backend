using Microsoft.AspNetCore.Mvc;
using Jat.IServices;
using Jat.DTOs;

namespace Jat.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _applicantService;

        public ApplicantController(IApplicantService applicantService)
        {
            _applicantService = applicantService;
        }

        [HttpGet("page/{pageNumber}/size/{pageSize}")]
        public async Task<IActionResult> GetAllApplicants(int pageNumber = 1, int pageSize = 10)
        {
            var (applicants, totalCount, totalPages) = await _applicantService.GetAllApplicantsAsync(pageNumber, pageSize);
            var response = new
            {
                Data = applicants,
                Pagination = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                }
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicantById(long id)
        {
            var applicant = await _applicantService.GetApplicantByIdAsync(id);
            if (applicant == null) return NotFound();
            return Ok(applicant);
        }

        [HttpPost]
        public async Task<IActionResult> AddApplicant([FromBody] ApplicantDto applicantDto)
        {
            await _applicantService.AddApplicantAsync(applicantDto);
            return CreatedAtAction(nameof(GetApplicantById), new { id = applicantDto.Id }, applicantDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApplicant(long id, [FromBody] ApplicantDto applicantDto)
        {
            await _applicantService.UpdateApplicantAsync(id, applicantDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicant(long id)
        {
            await _applicantService.DeleteApplicantAsync(id);
            return NoContent();
        }
    }
}
