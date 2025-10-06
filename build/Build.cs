using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Solution] readonly Solution Solution;

    [Parameter("Name of the plugin to compile")] readonly string PluginName = "BIMPlugins.ExtStorage";

    [Parameter("URL of the NuGet feed")] readonly string NugetSource = @"https://nuget.pkg.github.com/Samveyil/index.json";
    [Parameter("API Key for the NuGet feed")] readonly string NugetApiKey = "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";

    Target Compile => _ => _
        .Executes(() =>
        {
            var project = Solution.GetProject(PluginName);
            if (project == null)
                throw new FileNotFoundException("Not found!");

            var matchedNupkgs = Directory
                .GetFiles(RootDirectory, "*.nupkg", SearchOption.AllDirectories)
                .ToList();

            foreach (var nupkg in matchedNupkgs)
            {
                File.Delete(nupkg);
            }

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

    Target PushToGitHubNugetRepository => _ => _ 
    .Executes(() =>
        {
            var matchedNupkgs = Directory
                .GetFiles(RootDirectory, "*.nupkg", SearchOption.AllDirectories)
                .ToList();

            foreach (var nupkg in matchedNupkgs)
            {
                DotNetNuGetPush(s => s
                    .SetTargetPath(nupkg)
                    .SetApiKey(NugetApiKey)
                    .SetSource(NugetSource)
                );
            }
        });
}