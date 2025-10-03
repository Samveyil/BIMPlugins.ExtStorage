using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.Interfaces
{
    public class FamilyLoadOptions : IFamilyLoadOptions
    {
        public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
        {
            overwriteParameterValues = false;
            return true;
        }

        public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParametersValues)
        {
            source = FamilySource.Family;
            overwriteParametersValues = false;
            return true;
        }
    }
}