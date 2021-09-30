using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath DotnetBase => Dotnet / "base";
    public AbsolutePath DotnetBaseRepositoryDomainRepository => DotnetBase / "Repository/Domain/Repository.csproj";

    public AbsolutePath DotnetBaseDatabase => DotnetBase / "Database";
    public AbsolutePath DotnetBaseDatabaseMetaGenerated => DotnetBaseDatabase / "Meta/generated";
    public AbsolutePath DotnetBaseDatabaseGenerate => DotnetBaseDatabase / "Generate/Generate.csproj";
    public AbsolutePath DotnetBaseDatabaseMerge => DotnetBaseDatabase / "Merge/Merge.csproj";
    public AbsolutePath DotnetBaseDatabaseCommands => DotnetBaseDatabase / "Commands";
    public AbsolutePath DotnetBaseDatabaseServer => DotnetBaseDatabase / "Server";
    public AbsolutePath DotnetBaseDatabaseDomainTests => DotnetBaseDatabase / "Domain.Tests/Domain.Tests.csproj";
    public AbsolutePath DotnetBaseDatabaseResources => DotnetBaseDatabase / "Resources";
    public AbsolutePath DotnetBaseDatabaseResourcesBase => DotnetBaseDatabaseResources / "Base";

    public AbsolutePath DotnetBaseWorkspaceCSharp => DotnetBase / "Workspace/CSharp";

    public AbsolutePath DotnetBaseWorkspaceTypescript => DotnetBase / "Workspace/Typescript";
    public AbsolutePath DotnetBaseWorkspaceIntranetTests => DotnetBase / "Workspace/Scaffold/Intranet.Tests";
    public AbsolutePath DotnetBaseWorkspaceScaffoldGenerate => DotnetBase / "Workspace/Scaffold/Generate/Generate.csproj";
}
