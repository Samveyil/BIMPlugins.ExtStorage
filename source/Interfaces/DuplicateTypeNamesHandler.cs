using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.Interfaces
{
    /// <summary>
    /// An <see cref="Autodesk.Revit.DB.IDuplicateTypeNamesHandler"/> that keeps the types of the destination document
    /// when copied elements use types with the same names.
    /// </summary>
    /// <remarks>Pass it to <see cref="Autodesk.Revit.DB.CopyPasteOptions.SetDuplicateTypeNamesHandler"/>.</remarks>
    public class DuplicateTypeNamesHandler : IDuplicateTypeNamesHandler
    {
        /// <summary>Called when copied elements use types whose names already exist in the destination document.</summary>
        /// <param name="args">Information about the duplicate types.</param>
        /// <returns>Always <see cref="Autodesk.Revit.DB.DuplicateTypeAction.UseDestinationTypes"/>.</returns>
        public DuplicateTypeAction OnDuplicateTypeNamesFound(DuplicateTypeNamesHandlerArgs args)
        {
            return DuplicateTypeAction.UseDestinationTypes;
        }
    }
}

