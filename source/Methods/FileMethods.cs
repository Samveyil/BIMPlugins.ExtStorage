using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.Methods
{
    /// <summary>Factory methods for options used to open and save Revit documents.</summary>
    public static class FileMethods
    {
        /// <summary>Constructs a new instance of the options class configured with the specified detach option and workset configuration.</summary>
        /// <param name="projectPath">The path to the workshared model.</param>
        /// <param name="detachOption">An option that specifies whether or not a workset-enabled document is detached from its central document.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> to close all user-created worksets; otherwise, <see langword="false"/>.</param>
        /// <remarks>Note: if <paramref name="closeAllWorksets"/> is <see langword="false"/>, only worksets whose names start with <c>#</c> or <c>!</c> will be closed.</remarks>
        /// <returns>A new <see cref="Autodesk.Revit.DB.OpenOptions"/> instance configured with the specified detach and workset options.</returns>
        public static OpenOptions SetOpenOptions(ModelPath projectPath, DetachFromCentralOption detachOption = DetachFromCentralOption.DetachAndPreserveWorksets, bool closeAllWorksets=true)
        {
            OpenOptions opts = new OpenOptions() { DetachFromCentralOption = detachOption};

            try
            {
                var worksetConfig = new WorksetConfiguration(WorksetConfigurationOption.CloseAllWorksets);
                
                if (!closeAllWorksets)
                {
                    IList<WorksetPreview> worksets = WorksharingUtils.GetUserWorksetInfo(projectPath);
                    List<WorksetId> worksetIds = [];
                    foreach (WorksetPreview workset in worksets)
                    {
                        if (!workset.Name.StartsWith("#") && !workset.Name.StartsWith("!"))
                            worksetIds.Add(workset.Id);
                    }

                    worksetConfig.Open(worksetIds);
                }

                opts.SetOpenWorksetsConfiguration(worksetConfig);
            }
            catch (CentralModelException) { }
            catch (RevitServerCommunicationException) { }

            return opts;
        }


        /// <summary>Creates a new <see cref="Autodesk.Revit.DB.SaveAsOptions"/> instance that overwrites any existing file.</summary>
        /// <param name="saveAsCentral">
        /// <see langword="true"/> to save a workshared document as a central file. Pass <see langword="false"/> for a document
        /// without worksharing, such as <c>doc.IsWorkshared</c> gives: saving it with worksharing options fails.
        /// </param>
        /// <returns>A <see cref="Autodesk.Revit.DB.SaveAsOptions"/> instance with <see cref="Autodesk.Revit.DB.SaveAsOptions.OverwriteExistingFile"/> set to <see langword="true"/>.</returns>
        public static SaveAsOptions SetSaveAsOptions(bool saveAsCentral = true)
        {
            var saveOptions = new SaveAsOptions { OverwriteExistingFile = true };

            if (saveAsCentral)
                saveOptions.SetWorksharingOptions(new WorksharingSaveAsOptions { SaveAsCentral = true });

            return saveOptions;
        }
    }
}