using BIMPlugins.Nuke;
using Nuke.Common;
using Nuke.Common.ProjectModel;

public class Build : BIMPluginsBuild
{
    protected override int MajorVersion => 0;
    protected override int MinorVersion => 6;
    protected override int MaintenanceVersion => 1;

    [Solution("BIMPlugins.ExtStorage.sln")]
    protected override Solution Solution { get; }

    [Parameter("Name of the project to build")]
    protected override string ProjectName { get; } = "BIMPlugins.ExtStorage";

    [Parameter("Configurations to build")]
    protected override string[] Configurations { get; } = { "R2019", "R2020", "R2021", "R2022", "R2023", "R2024", "R2025" };

    public static int Main() => Execute<Build>(x => x.Compile);
}
