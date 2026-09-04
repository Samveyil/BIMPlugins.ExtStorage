using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.Extensions.UtilsExtensions
{
    public static class ModelPathUtilsExtensions
    {
        /// <inheritdoc cref="Autodesk.Revit.DB.ModelPathUtils.ConvertModelPathToUserVisiblePath(Autodesk.Revit.DB.ModelPath)" />
        public static string ConvertToUserVisiblePath(this ModelPath modelPath)
        {
            return ModelPathUtils.ConvertModelPathToUserVisiblePath(modelPath);
        }
    }
}
