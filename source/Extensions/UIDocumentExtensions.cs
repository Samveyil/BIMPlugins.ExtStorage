using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class UIDocumentExtensions
    {
        public static List<Element> ToSelectedElements(this UIDocument uiDoc)
        {
            return uiDoc.Selection.GetElementIds()
                .Select(id => id.ToElement())
                .ToList();
        }
    }
}