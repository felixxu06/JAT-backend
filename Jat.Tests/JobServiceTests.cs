using Moq;
using Jat.Services;
using Jat.IRepositories;
using Jat.DTOs;
using Jat.Entities;

namespace Jat.Tests
{
    public class JobServiceTests
    {
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<IApplicantRepository> _applicantRepositoryMock;
        private readonly JobService _jobService;

        public JobServiceTests()
        {
            _jobRepositoryMock = new Mock<IJobRepository>();
            _applicantRepositoryMock = new Mock<IApplicantRepository>();
            _jobService = new JobService(_jobRepositoryMock.Object, _applicantRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllJobsAsync_ReturnsJobsWithPagination()
        {
            var jobs = new List<Job> { new Job { Id = 1, CompanyName = "Test Company", Position = "Developer" } };
            _jobRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(jobs);
            _jobRepositoryMock.Setup(repo => repo.GetTotalCountAsync()).ReturnsAsync(1);

            var (resultJobs, totalCount, totalPages) = await _jobService.GetAllJobsAsync(1, 10);

            Assert.NotNull(resultJobs);
            Assert.Single(resultJobs);
            Assert.Equal(1, totalCount);
            Assert.Equal(1, totalPages);
        }

        [Fact]
        public async Task GetJobByIdAsync_ReturnsJob()
        {
            var job = new Job { Id = 1, CompanyName = "Test Company", Position = "Developer" };
            _jobRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(job);

            var result = await _jobService.GetJobByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task AddJobAsync_CallsRepository()
        {
            var job = new JobDto { CompanyName = "Test Company", Position = "Developer" };

            await _jobService.AddJobAsync(job);

            _jobRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Job>()), Times.Once);
        }
    }
}