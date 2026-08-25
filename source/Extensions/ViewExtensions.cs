using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Linq;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class ViewExtensions
    {
        /// <summary>Returns the UI view that corresponds to the specified view.</summary>
        /// <returns>A <see cref="Autodesk.Revit.UI.UIView"/> with the same ViewId, or <see langword="null"/> if the view is not open.</returns>
        public static UIView ToUIView(this View view)
        {
            return RevitAPI.UIDocument.GetOpenUIViews()
                .FirstOrDefault(v => v.ViewId.ToString() == view.Id.ToString());
        }
    }
}
