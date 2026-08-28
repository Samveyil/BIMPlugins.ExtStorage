using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.Interfaces
{
    /// <summary>
    /// An <see cref="Autodesk.Revit.DB.IFamilyLoadOptions"/> implementation that defines how Revit
    /// behaves when loading a family that already exists in the target document.
    /// </summary>
    public class FamilyLoadOptions : IFamilyLoadOptions
    {
        private readonly bool _overwrite;
        private readonly FamilySource _familySource = FamilySource.Family;

        /// <summary>Creates an instance with a parameter-overwrite setting.</summary>
        /// <param name="overwriteParameters">
        /// This determines whether or not to overwrite the parameter values of existing types. The default value is <see langword="false"/>.
        /// </param>
        public FamilyLoadOptions(bool overwriteParameters=false) => _overwrite = overwriteParameters;

        /// <summary>Creates an instance with a shared-family source and parameter-overwrite settings.</summary>
        /// <param name="familySource">This indicates if the family will load from the project or the current family.</param>
        /// <param name="overwriteParameters">
        /// This determines whether or not to overwrite the parameter values of existing types.
        /// </param>
        public FamilyLoadOptions(FamilySource familySource, bool overwriteParameters)
        {
            _familySource = familySource;
            _overwrite = overwriteParameters;
        }
 
        public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
        {
            overwriteParameterValues = _overwrite;
            return true;
        }

        public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParametersValues)
        {
            source = _familySource;
            overwriteParametersValues = _overwrite;
            return true;
        }
    }
}