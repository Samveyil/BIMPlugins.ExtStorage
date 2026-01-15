using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.Methods
{
    public static class FileMethods
    {
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