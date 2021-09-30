using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath DotnetCore => Dotnet / "core";
    public AbsolutePath DotnetCoreRepositoryDomainRepository => DotnetCore / "Repository/Domain/Repository.csproj";

    public AbsolutePath DotnetCoreDatabase => DotnetCore / "Database";
    public AbsolutePath DotnetCoreDatabaseMetaGenerated => DotnetCoreDatabase / "Meta/generated";
    public AbsolutePath DotnetCoreDatabaseGenerate => DotnetCoreDatabase / "Generate/Generate.csproj";
    public AbsolutePath DotnetCoreDatabaseMerge => DotnetCoreDatabase / "Merge/Merge.csproj";
    public AbsolutePath DotnetCoreDatabaseServer => DotnetCoreDatabase / "Server";
    public AbsolutePath DotnetCoreDatabaseCommands => DotnetCoreDatabase / "Commands";
    public AbsolutePath DotnetDatabaseDomainTests => DotnetCoreDatabase / "Domain.Tests/Domain.Tests.csproj";
    public AbsolutePath DotnetCoreDatabaseServerTests => DotnetCoreDatabase / "Server.Tests/Server.Tests.csproj";
    public AbsolutePath DotnetCoreDatabaseResources => DotnetCoreDatabase / "Resources";
    public AbsolutePath DotnetCoreDatabaseResourcesCore => DotnetCoreDatabaseResources / "Core";
    public AbsolutePath DotnetCoreDatabaseResourcesCustom => DotnetCoreDatabaseResources / "Custom";

    public AbsolutePath DotnetCoreWorkspaceTypescript=> DotnetCore / "Workspace/Typescript";
    public AbsolutePath DotnetCoreWorkspaceScaffold => DotnetCore / "Workspace/Scaffold";
    public AbsolutePath DotnetCoreWorkspaceScaffoldGenerate => DotnetCoreWorkspaceScaffold / "Generate/Generate.csproj";
    public AbsolutePath DotnetCoreWorkspaceScaffoldAngularMaterialTests => DotnetCoreWorkspaceScaffold / "Angular.Material.Tests/Angular.Material.Tests.csproj";

    public AbsolutePath CoreWorkspaceCSharpDomainTests => DotnetCore / "Workspace/CSharp/Domain.Tests";
}
