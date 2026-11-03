using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.Extensions.UtilsExtensions
{
    /// <summary>Extension methods wrapping <see cref="Autodesk.Revit.DB.ModelPathUtils"/>.</summary>
    public static class ModelPathUtilsExtensions
    {
        /// <inheritdoc cref="Autodesk.Revit.DB.ModelPathUtils.ConvertModelPathToUserVisiblePath(Autodesk.Revit.DB.ModelPath)" />
        public static string ConvertToUserVisiblePath(this ModelPath modelPath)
        {
            return ModelPathUtils.ConvertModelPathToUserVisiblePath(modelPath);
        }
    }
}
