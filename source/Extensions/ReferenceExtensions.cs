using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class ReferenceExtensions
    {
        public static Element ToElement(this Reference reference, Document doc = null)
        {
            doc ??= RevitAPI.Document;
            return doc.GetElement(reference);
        }
        public static T ToElement<T>(this Reference reference, Document doc = null) where T : Element
        {
            doc ??= RevitAPI.Document;
            return (T)doc.GetElement(reference);
        }
    }
}
