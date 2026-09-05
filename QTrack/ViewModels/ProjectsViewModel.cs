using QTrack.Services;
using QTrack.Utils;

namespace QTrack.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProjectsViewModel : BindableBase
    {
        private readonly IProjectService projectService;

        public ProjectsViewModel(IProjectService projectService)
        {
            AppLogger.Info("IssuesViewModel created");
            AppLogger.Info(projectService.ToString());

            this.projectService = projectService;
        }
    }
}