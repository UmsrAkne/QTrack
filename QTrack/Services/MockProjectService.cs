using QTrack.Models;

namespace QTrack.Services
{
    public class MockProjectService : IProjectService
    {
        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            var list = new List<Project>();
            for (var i = 0; i < 40; i++)
            {
                list.Add(new Project { Name = $"Project {i:D3}", });
            }

            var p1 = list[1];
            p1.IsFavorite = true;
            p1.Name = "Favorite Project";

            var p2 = list[2];
            p2.IsFavorite = true;
            p2.IsArchive = true;
            p2.Name = "Favorite Project (Archived)";

            var p3 = list[3];
            p3.IsArchive = true;
            p3.Name = "Project (Archived)";

            var p4 = list[4];
            p4.Name = "オープンに失敗するプロジェクト (Fail)";

            return await Task.FromResult(list);
        }
    }
}