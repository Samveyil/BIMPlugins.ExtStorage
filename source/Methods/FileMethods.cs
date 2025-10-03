using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;

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
                    if (!workset.Name.Contains("#"))
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
    }
}