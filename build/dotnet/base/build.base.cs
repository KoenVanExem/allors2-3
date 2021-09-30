using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

partial class Build
{
    Target DotnetBaseResetDatabase => _ => _
        .Executes(() =>
        {
            var database = "Base";
            using var sqlServer = new SqlServer();
            sqlServer.Drop(database);
            sqlServer.Create(database);
        });

    private Target DotnetBaseDatabaseTest => _ => _
         .DependsOn(DotnetBaseDatabaseTestDomain);

    private Target DotnetBaseMerge => _ => _
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.DotnetCoreDatabaseMerge)
                .SetApplicationArguments($"{Paths.DotnetCoreDatabaseResourcesCore} {Paths.DotnetBaseDatabaseResourcesBase} {Paths.DotnetBaseDatabaseResources}"));
        });

    private Target DotnetBaseDatabaseTestDomain => _ => _
         .DependsOn(DotnetBaseGenerate)
         .Executes(() =>
         {
             DotNetTest(s => s
                 .SetProjectFile(Paths.DotnetBaseDatabaseDomainTests)
                 .AddLoggers("trx;LogFileName=BaseDatabaseDomain.trx")
                 .SetResultsDirectory(Paths.ArtifactsTests));
         });

    private Target DotnetBaseGenerate => _ => _
         .After(Clean)
         .DependsOn(DotnetBaseMerge)
         .Executes(() =>
         {
             DotNetRun(s => s
                 .SetProjectFile(Paths.DotnetSystemRepositoryGenerate)
                 .SetApplicationArguments($"{Paths.DotnetBaseRepositoryDomainRepository} {Paths.DotnetSystemRepositoryTemplatesMetaCs} {Paths.DotnetBaseDatabaseMetaGenerated}"));
             DotNetRun(s => s
                 .SetProcessWorkingDirectory(Paths.DotnetBase)
                 .SetProjectFile(Paths.DotnetBaseDatabaseGenerate));
         });

    private Target DotnetBasePublishCommands => _ => _
         .DependsOn(DotnetBaseGenerate)
         .Executes(() =>
         {
             var dotNetPublishSettings = new DotNetPublishSettings()
                 .SetProcessWorkingDirectory(Paths.DotnetBaseDatabaseCommands)
                 .SetOutput(Paths.ArtifactsBaseCommands);
             DotNetPublish(dotNetPublishSettings);
         });

    private Target DotnetBasePublishServer => _ => _
             .DependsOn(DotnetBaseGenerate)
         .Executes(() =>
         {
             var dotNetPublishSettings = new DotNetPublishSettings()
                 .SetProcessWorkingDirectory(Paths.DotnetBaseDatabaseServer)
                 .SetOutput(Paths.ArtifactsBaseServer);
             DotNetPublish(dotNetPublishSettings);
         });

    private Target DotnetBaseInstall => _ => _
        .Executes(() =>
        {
            NpmInstall(s => s
                .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
                .SetProcessWorkingDirectory(Paths.DotnetBaseWorkspaceTypescript));
        });

    private Target DotnetBaseScaffold => _ => _
         .DependsOn(DotnetBaseGenerate)
         .ProceedAfterFailure()
         .Executes(() =>
         {
             try
             {
                 NpmRun(s => s
                     .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
                     .SetProcessWorkingDirectory(Paths.DotnetBaseWorkspaceTypescript)
                     .SetCommand("scaffold"));
             }
             catch
             {
                 // TODO:
             }

             DotNetRun(s => s
                 .SetProcessWorkingDirectory(Paths.DotnetBase)
                 .SetProjectFile(Paths.DotnetBaseWorkspaceScaffoldGenerate));
         });

    private Target DotnetBaseWorkspaceTypescriptDomain => _ => _
         .DependsOn(DotnetBaseGenerate)
         .DependsOn(EnsureDirectories)
         .Executes(() =>
         {
             NpmRun(s => s
                 .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
                 .SetProcessWorkingDirectory(Paths.DotnetBaseWorkspaceTypescript)
                 .SetCommand("domain:test"));
         });

    private async Task DotnetBaseWorkspaceIntranetTest(string category)
    {
        using (var sqlServer = new SqlServer())
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsBaseCommands);
            using (var server = new Server(Paths.ArtifactsBaseServer))
            {
                using (var angular = new Angular(Paths.DotnetBaseWorkspaceTypescript, "intranet:serve"))
                {
                    await server.Ready();
                    await angular.Init();
                    DotNetTest(
                        s => s
                            .SetProjectFile(Paths.DotnetBaseWorkspaceIntranetTests)
                            .AddLoggers($"trx;LogFileName=BaseIntranet{category}Tests.trx")
                            .SetFilter($"Category={category}")
                            .SetResultsDirectory(Paths.ArtifactsTests));
                }
            }
        }
    }

    private Target BaseWorkspaceIntranetGenericTests => _ => _
         .DependsOn(this.DotnetBaseScaffold)
         .DependsOn(this.DotnetBasePublishServer)
         .DependsOn(this.DotnetBasePublishCommands)
         .DependsOn(this.DotnetBaseResetDatabase)
         .Executes(async () =>
         {
             await this.DotnetBaseWorkspaceIntranetTest("Generic");
         });

    private Target BaseWorkspaceIntranetOtherTests => _ => _
        .DependsOn(this.DotnetBaseScaffold)
        .DependsOn(DotnetBasePublishServer)
        .DependsOn(DotnetBasePublishCommands)
        .DependsOn(DotnetBaseResetDatabase)
        .Executes(async () =>
        {
            await this.DotnetBaseWorkspaceIntranetTest("Other");
        });

    private Target BaseWorkspaceIntranetRelationTests => _ => _
        .DependsOn(this.DotnetBaseScaffold)
        .DependsOn(DotnetBasePublishServer)
        .DependsOn(DotnetBasePublishCommands)
        .DependsOn(DotnetBaseResetDatabase)
        .Executes(async () =>
        {
            await this.DotnetBaseWorkspaceIntranetTest("Relation");
        });

    private Target BaseWorkspaceIntranetInvoiceTests => _ => _
    .DependsOn(this.DotnetBaseScaffold)
    .DependsOn(DotnetBasePublishServer)
    .DependsOn(DotnetBasePublishCommands)
    .DependsOn(DotnetBaseResetDatabase)
    .Executes(async () =>
    {
        await this.DotnetBaseWorkspaceIntranetTest("Invoice");
    });

    private Target BaseWorkspaceIntranetOrderTests => _ => _
        .DependsOn(this.DotnetBaseScaffold)
        .DependsOn(DotnetBasePublishServer)
        .DependsOn(DotnetBasePublishCommands)
        .DependsOn(DotnetBaseResetDatabase)
        .Executes(async () =>
        {
            await this.DotnetBaseWorkspaceIntranetTest("Order");
        });

    private Target BaseWorkspaceIntranetProductTests => _ => _
        .DependsOn(this.DotnetBaseScaffold)
        .DependsOn(DotnetBasePublishServer)
        .DependsOn(DotnetBasePublishCommands)
        .DependsOn(DotnetBaseResetDatabase)
        .Executes(async () =>
        {
            await this.DotnetBaseWorkspaceIntranetTest("Product");
        });

    private Target BaseWorkspaceIntranetShipmentTests => _ => _
        .DependsOn(this.DotnetBaseScaffold)
        .DependsOn(DotnetBasePublishServer)
        .DependsOn(DotnetBasePublishCommands)
        .DependsOn(DotnetBaseResetDatabase)
        .Executes(async () =>
        {
            await this.DotnetBaseWorkspaceIntranetTest("Shipment");
        });

    private Target BaseWorkspaceIntranetWorkEffortTests => _ => _
        .DependsOn(this.DotnetBaseScaffold)
        .DependsOn(DotnetBasePublishServer)
        .DependsOn(DotnetBasePublishCommands)
        .DependsOn(DotnetBaseResetDatabase)
        .Executes(async () =>
        {
            await this.DotnetBaseWorkspaceIntranetTest("WorkEffort");
        });

    private Target BaseWorkspaceTypescriptTest => _ => _
        .DependsOn(DotnetBaseWorkspaceTypescriptDomain);

    private Target BaseTest => _ => _
        .DependsOn(DotnetBaseDatabaseTest)
        .DependsOn(BaseWorkspaceTypescriptTest);

    private Target Base => _ => _
        .DependsOn(Clean)
        .DependsOn(BaseTest);
}
