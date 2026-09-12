using System.IO;
using System.Windows;
using DotNetEnv;
using LiteDB;
using QTrack.Services;
using QTrack.Utils;
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
        var credentials = new ApiCredentials
        {
            YoutrackApiKey = Env.GetString("YOUTRACK_API_KEY"),
            YoutrackProjectsEndpoint = Env.GetString("YOUTRACK_PROJECTS_ENDPOINT"),
            YoutrackIssuesEndpoint = Env.GetString("YOUTRACK_ISSUES_ENDPOINT"),
        };

        containerRegistry.RegisterInstance(credentials);

        #if RELEASE
        containerRegistry.Register<IProjectService, ProjectService>();
        containerRegistry.Register<IIssueService, IssueService>();
        #elif DEBUG
        containerRegistry.Register<IProjectService, MockProjectService>();
        containerRegistry.Register<IIssueService, MockIssueService>();
        #endif

        containerRegistry.RegisterSingleton<ProjectsViewModel>();
        containerRegistry.Register<IssuesViewModel>();

        var settings = AppSettings.Load();
        settings.Save();
        containerRegistry.RegisterInstance(settings);

        // LiteDB の初期設定
        var dbPath = Path.Combine(AppContext.BaseDirectory, "AppData.db");
        var connectionString = $"Filename={dbPath};Connection=shared";
        var litedb = new LiteDatabase(connectionString);
        containerRegistry.RegisterInstance<ILiteDbService>(new LiteDbService(litedb));
    }

    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        Env.Load();
        base.OnStartup(e);
    }
}