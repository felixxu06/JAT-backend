using System.Collections.Concurrent;
using Jat.Entities;

namespace Jat.Repositories
{
    public class InMemoryDatabase
    {
        public ConcurrentDictionary<long, Job> Jobs { get; } = new();
        public ConcurrentDictionary<long, Applicant> Applicants { get; } = new();
        // Add more entity sets as needed
    }
}