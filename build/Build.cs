using System.Collections.Generic;
using System.IO;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.MSBuild;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Solution] readonly Solution Solution;

    [Parameter("Name of the plugin to compile")]
    readonly string PluginName = "BIMPlugins.ExtStorage";

    Target Compile => _ => _
        .Executes(() =>
        {
            var project = Solution.GetProject(PluginName);
            if (project == null)
                throw new FileNotFoundException("Not found!");

            var build = new List<string>();
            foreach (var (_, c) in project.Configurations)
            {
                var configuration = c.Split("|")[0];
                var platform = c.Split("|")[1];

                if (configuration.Contains("Debug") || configuration == "Release" || build.Contains(configuration))
                    continue;

                build.Add(configuration);

                Serilog.Log.Debug($"Configuration: {configuration}");

                string msbuildPath = @"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe";

                MSBuild(_ => _
                    .SetProcessToolPath(msbuildPath)
                    .SetProjectFile(project.Path)
                    .SetConfiguration(configuration)
                    .SetTargets("Restore"));

                MSBuild(_ => _
                    .SetProcessToolPath(msbuildPath)
                    .SetProjectFile(project.Path)
                    .SetConfiguration(configuration)
                    .SetTargets("Rebuild"));
            }
        });
}