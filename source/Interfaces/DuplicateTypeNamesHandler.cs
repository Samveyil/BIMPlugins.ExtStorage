using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.Interfaces
{
    public class DuplicateTypeNamesHandler : IDuplicateTypeNamesHandler
    {
        public DuplicateTypeAction OnDuplicateTypeNamesFound(DuplicateTypeNamesHandlerArgs args)
        {
            return DuplicateTypeAction.UseDestinationTypes;
        }
    }
}

