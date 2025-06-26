using Xunit;
using Moq;
using Jat.Services;
using Jat.IRepositories;
using Jat.DTOs;
using Jat.Entities;

namespace Jat.Tests
{
    public class ApplicantServiceTests
    {
        private readonly Mock<IApplicantRepository> _applicantRepositoryMock;
        private readonly ApplicantService _applicantService;

        public ApplicantServiceTests()
        {
            _applicantRepositoryMock = new Mock<IApplicantRepository>();
            _applicantService = new ApplicantService(_applicantRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllApplicantsAsync_ReturnsApplicantsWithPagination()
        {
            var applicants = new List<Applicant> { new Applicant { Id = 1, FirstName = "John", LastName = "Doe" } };
            _applicantRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(applicants);
            _applicantRepositoryMock.Setup(repo => repo.GetTotalCountAsync()).ReturnsAsync(1);

            var (resultApplicants, totalCount, totalPages) = await _applicantService.GetAllApplicantsAsync(1, 10);

            Assert.NotNull(resultApplicants);
            Assert.Single(resultApplicants);
            Assert.Equal(1, totalCount);
            Assert.Equal(1, totalPages);
        }

        [Fact]
        public async Task GetApplicantByIdAsync_ReturnsApplicant()
        {
            var applicant = new Applicant { Id = 1, FirstName = "John", LastName = "Doe" };
            _applicantRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(applicant);

            var result = await _applicantService.GetApplicantByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task AddApplicantAsync_CallsRepository()
        {
            var applicant = new ApplicantDto { FirstName = "John", LastName = "Doe" };

            await _applicantService.AddApplicantAsync(applicant);

            _applicantRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Applicant>()), Times.Once);
        }
    }
}