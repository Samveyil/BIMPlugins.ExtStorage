using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.Methods
{
    public static class FileMethods
    {
        /// <summary>Constructs a new instance of the options class configured with the specified detach option and workset configuration.</summary>
        /// <param name="projectPath">The path to the workshared model.</param>
        /// <param name="detachOption">An option that specifies whether or not a workset-enabled document is detached from its central document.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> if to close all user-created worksets; otherwise, <see langword="false"/>.</param>
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
                    List<WorksetPreview> worksets = (List<WorksetPreview>)WorksharingUtils.GetUserWorksetInfo(projectPath);
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


        /// <summary>Creates a new <see cref="Autodesk.Revit.DB.SaveAsOptions"/> instance configured to save the document as a central worksharing file, overwriting any existing file.</summary>
        /// <returns>A <see cref="Autodesk.Revit.DB.SaveAsOptions"/> instance with <see cref="Autodesk.Revit.DB.SaveAsOptions.OverwriteExistingFile"/> and <see cref="Autodesk.Revit.DB.WorksharingSaveAsOptions.SaveAsCentral"/> set to <see langword="true"/>.</returns>
        public static SaveAsOptions SetSaveAsOptions()
        {
            WorksharingSaveAsOptions worksharingOptions = new WorksharingSaveAsOptions();
            worksharingOptions.SaveAsCentral = true;
            
            SaveAsOptions saveOptions = new SaveAsOptions();
            saveOptions.OverwriteExistingFile = true;
            saveOptions.SetWorksharingOptions(worksharingOptions);
            
            return saveOptions;
        }
    }
}