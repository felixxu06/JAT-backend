using Jat.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jat.IServices
{
    public interface IApplicantService
    {
        Task<IEnumerable<ApplicantDto>> GetAllApplicantsAsync(int pageNumber, int pageSize);
        Task<ApplicantDto?> GetApplicantByIdAsync(long id);
        Task AddApplicantAsync(ApplicantDto applicantDto);
        Task UpdateApplicantAsync(long id, ApplicantDto applicantDto);
        Task DeleteApplicantAsync(long id);
    }
}