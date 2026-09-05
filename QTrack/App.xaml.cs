using System.Windows;
using QTrack.Services;
using QTrack.ViewModels;
using QTrack.Views;

namespace QTrack;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        #if RELEASE
        containerRegistry.Register<IProjectService, ProjectService>();
        containerRegistry.Register<IIssueService, IssueService>();
        #elif DEBUG
        containerRegistry.Register<IProjectService, MockProjectService>();
        containerRegistry.Register<IIssueService, MockIssueService>();
        #endif

        containerRegistry.RegisterSingleton<ProjectsViewModel>();
        containerRegistry.RegisterSingleton<IssuesViewModel>();
    }

    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }
}