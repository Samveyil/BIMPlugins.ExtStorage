using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.Extensions.UtilsExtensions
{
    public static class SolidUtilsExtensions
    {
        /// <inheritdoc cref="Autodesk.Revit.DB.SolidUtils.Clone(Autodesk.Revit.DB.Solid)" />
        public static Solid Clone(this Solid solid) => SolidUtils.Clone(solid);

        /// <inheritdoc cref="Autodesk.Revit.DB.SolidUtils.CreateTransformed(Autodesk.Revit.DB.Solid,Autodesk.Revit.DB.Transform)" />
        public static Solid CreateTransformed(this Solid solid, Transform transform) => SolidUtils.CreateTransformed(solid, transform);

        /// <inheritdoc cref="Autodesk.Revit.DB.SolidUtils.SplitVolumes(Autodesk.Revit.DB.Solid)" />
        public static IList<Solid> SplitVolumes(this Solid solid) => SolidUtils.SplitVolumes(solid);
    }
}