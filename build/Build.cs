using BIMPlugins.Nuke;
using Nuke.Common;
using Nuke.Common.ProjectModel;

public class Build : BIMPluginsBuild
{
    protected override int MajorVersion => 0;
    protected override int MinorVersion => 3;
    protected override int MaintenanceVersion => 8;


    [Solution("BIMPlugins.ExtStorage.sln")]
    protected override Solution Solution { get; }

    [Parameter("Имя проекта для сборки")]
    protected override string ProjectName { get; } = "BIMPlugins.ExtStorage";

    public static int Main() => Execute<Build>(x => x.Compile);
}