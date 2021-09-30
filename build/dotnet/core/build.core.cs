using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

partial class Build
{
    Target DotnetCoreResetDatabase => _ => _
        .Executes(() =>
        {
            var database = "Core";
            using var sqlServer = new SqlServer();
            sqlServer.Drop(database);
            sqlServer.Create(database);
        });

    private Target DotnetCoreMerge => _ => _
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.DotnetCoreDatabaseMerge)
                .SetApplicationArguments($"{Paths.DotnetCoreDatabaseResourcesCore} {Paths.DotnetCoreDatabaseResourcesCustom} {Paths.DotnetCoreDatabaseResources}"));
        });

    Target DotnetCoreGenerate => _ => _
        .After(Clean)
        .DependsOn(DotnetCoreMerge)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.DotnetSystemRepositoryGenerate)
                .SetApplicationArguments($"{Paths.DotnetCoreRepositoryDomainRepository} {Paths.DotnetSystemRepositoryTemplatesMetaCs} {Paths.DotnetCoreDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.DotnetCore)
                .SetProjectFile(Paths.DotnetCoreDatabaseGenerate));
        });

    Target DotnetCoreDatabaseTestDomain => _ => _
        .DependsOn(DotnetCoreGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.DotnetDatabaseDomainTests)
                .AddLoggers("trx;LogFileName=CoreDatabaseDomain.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target DotnetCorePublishCommands => _ => _
        .DependsOn(DotnetCoreGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetProcessWorkingDirectory(Paths.DotnetCoreDatabaseCommands)
                .SetOutput(Paths.ArtifactsCoreCommands);
            DotNetPublish(dotNetPublishSettings);
        });

    Target DotnetCorePublishServer => _ => _
        .DependsOn(DotnetCoreGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetProcessWorkingDirectory(Paths.DotnetCoreDatabaseServer)
                .SetOutput(Paths.ArtifactsCoreServer);
            DotNetPublish(dotNetPublishSettings);
        });

    Target DotnetCoreDatabaseTestServer => _ => _
        .DependsOn(DotnetCoreGenerate)
        .DependsOn(DotnetCorePublishServer)
        .DependsOn(DotnetCorePublishCommands)
        .DependsOn(DotnetCoreResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);
            using var server = new Server(Paths.ArtifactsCoreServer);
            await server.Ready();
            DotNetTest(s => s
                .SetProjectFile(Paths.DotnetCoreDatabaseServerTests)
                .AddLoggers("trx;LogFileName=CoreDatabaseServer.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target DotnetCoreInstall => _ => _
        .Executes(() =>
        {
            NpmInstall(s => s
                .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
                .SetProcessWorkingDirectory(Paths.DotnetCoreWorkspaceTypescript));
        });

    Target DotnetCoreScaffold => _ => _
        .DependsOn(DotnetCoreGenerate)
        .ProceedAfterFailure()
        .Executes(() =>
        {
            try
            {
                NpmRun(s => s
                    .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
                    .SetProcessWorkingDirectory(Paths.DotnetCoreWorkspaceTypescript)
                    .SetCommand("scaffold"));
            }
            catch
            {
                // TODO:
            }


            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.DotnetCore)
                .SetProjectFile(Paths.DotnetCoreWorkspaceScaffoldGenerate));
        });

    Target DotnetCoreWorkspaceTypescriptDomain => _ => _
        .DependsOn(DotnetCoreGenerate)
        .DependsOn(EnsureDirectories)
        .Executes(() =>
        {
            NpmRun(s => s
                .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
                .SetProcessWorkingDirectory(Paths.DotnetCoreWorkspaceTypescript)
                .SetCommand("domain:test"));
        });

    Target DotnetCoreWorkspaceTypescriptPromise => _ => _
        .DependsOn(DotnetCoreGenerate)
        .DependsOn(DotnetCorePublishServer)
        .DependsOn(DotnetCorePublishCommands)
        .DependsOn(EnsureDirectories)
        .DependsOn(DotnetCoreResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);

            using (var sqlServer = new SqlServer())
            {
                using (var server = new Server(Paths.ArtifactsCoreServer))
                {
                    await server.Ready();
                    NpmRun(s => s
                        .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
                        .SetProcessWorkingDirectory(Paths.DotnetCoreWorkspaceTypescript)
                        .SetCommand("promise:test"));
                }
            }
        });

    Target DotnetCoreWorkspaceTypescriptAngular => _ => _
        .DependsOn(DotnetCoreGenerate)
        .DependsOn(DotnetCorePublishServer)
        .DependsOn(DotnetCorePublishCommands)
        .DependsOn(EnsureDirectories)
        .DependsOn(DotnetCoreResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);

            using (var sqlServer = new SqlServer())
            {
                using (var server = new Server(Paths.ArtifactsCoreServer))
                {
                    await server.Ready();
                    NpmRun(s => s
                        .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
                        .SetProcessWorkingDirectory(Paths.DotnetCoreWorkspaceTypescript)
                        .SetCommand("angular:test"));
                }
            }
        });

    Target DotnetCoreWorkspaceTypescriptMaterialTests => _ => _
        .DependsOn(DotnetCoreScaffold)
        .DependsOn(DotnetCorePublishServer)
        .DependsOn(DotnetCorePublishCommands)
        .DependsOn(DotnetCoreResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);

            using (var sqlServer = new SqlServer())
            {
                using (var server = new Server(Paths.ArtifactsCoreServer))
                {
                    using (var angular = new Angular(Paths.DotnetCoreWorkspaceTypescript, "angular-material:serve"))
                    {
                        await server.Ready();
                        await angular.Init();
                        DotNetTest(s => s
                            .SetProjectFile(Paths.DotnetCoreWorkspaceScaffoldAngularMaterialTests)
                            .AddLoggers("trx;LogFileName=CoreWorkspaceTypescriptMaterialTests.trx")
                            .SetResultsDirectory(Paths.ArtifactsTests));
                    }
                }
            }
        });

    Target CoreWorkspaceCSharpDomainTests => _ => _
        .DependsOn(DotnetCorePublishServer)
        .DependsOn(DotnetCorePublishCommands)
        .DependsOn(DotnetCoreResetDatabase)
        .Executes(async () =>
        {
            DotNet("Commands.dll Populate", Paths.ArtifactsCoreCommands);

            using (var sqlServer = new SqlServer())
            {
                using (var server = new Server(Paths.ArtifactsCoreServer))
                {
                    await server.Ready();
                    DotNetTest(s => s
                        .SetProjectFile(Paths.CoreWorkspaceCSharpDomainTests)
                        .AddLoggers("trx;LogFileName=CoreWorkspaceCSharpDomainTests.trx")
                        .SetResultsDirectory(Paths.ArtifactsTests));
                }
            }
        });

    Target CoreDatabaseTest => _ => _
        .DependsOn(DotnetCoreDatabaseTestDomain)
        .DependsOn(DotnetCoreDatabaseTestServer);

    Target CoreWorkspaceTypescriptTest => _ => _
        .DependsOn(DotnetCoreWorkspaceTypescriptDomain)
        .DependsOn(DotnetCoreWorkspaceTypescriptPromise)
        // TODO: Koen
        //.DependsOn(CoreWorkspaceTypescriptAngular)
        .DependsOn(DotnetCoreWorkspaceTypescriptMaterialTests);

    Target CoreWorkspaceCSharpTest => _ => _
        .DependsOn(CoreWorkspaceCSharpDomainTests);

    Target CoreWorkspaceTest => _ => _
        .DependsOn(CoreWorkspaceCSharpTest)
        .DependsOn(CoreWorkspaceTypescriptTest);

    Target CoreTest => _ => _
        .DependsOn(CoreDatabaseTest)
        .DependsOn(CoreWorkspaceTest);

    Target Core => _ => _
        .DependsOn(Clean)
        .DependsOn(CoreTest);
}
