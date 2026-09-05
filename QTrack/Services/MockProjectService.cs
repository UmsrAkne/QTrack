using QTrack.Models;

namespace QTrack.Services
{
    public class MockProjectService : IProjectService
    {
        public IEnumerable<Project> GetAllProjects()
        {
            return new List<Project>();
        }
    }
}