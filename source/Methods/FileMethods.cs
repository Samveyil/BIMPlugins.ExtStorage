using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.Methods
{
    public static class FileMethods
    {
        public static OpenOptions SetOpenOptions(ModelPath projectPath, DetachFromCentralOption detachOption = DetachFromCentralOption.DetachAndPreserveWorksets)
        {
            OpenOptions opts = new OpenOptions();
            opts.DetachFromCentralOption = detachOption;

            try
            {
                List<WorksetPreview> worksets = (List<WorksetPreview>)WorksharingUtils.GetUserWorksetInfo(projectPath);
                List<WorksetId> worksetIds = [];
                foreach (WorksetPreview workset in worksets)
                {
                    if (!workset.Name.StartsWith("#") && !workset.Name.StartsWith("!"))
                    {
                        worksetIds.Add(workset.Id);
                    }
                }

                WorksetConfiguration worksetConfig = new WorksetConfiguration(WorksetConfigurationOption.CloseAllWorksets);
                worksetConfig.Open(worksetIds);

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