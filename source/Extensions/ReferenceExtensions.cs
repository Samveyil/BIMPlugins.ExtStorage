using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class ReferenceExtensions
    {
        /// <summary>Gets the Element.</summary>
        /// <param name="doc">The owning document. Passing <see langword="null"/> will return the active document</param>
        /// <returns>The element.</returns>
        public static Element ToElement(this Reference reference, Document doc = null)
        {
            doc ??= RevitAPI.Document;
            return doc.GetElement(reference);
        }

        /// <summary>Gets the Element.</summary>
        /// <typeparam name="T">The element type to return (must inherit from Element).</typeparam>
        /// <param name="doc">The owning document. Passing <see langword="null"/> will return the active document</param>
        /// <returns>The element cast to type <typeparamref name="T"/>.</returns>
        public static T ToElement<T>(this Reference reference, Document doc = null) where T : Element
        {
            doc ??= RevitAPI.Document;
            return (T)doc.GetElement(reference);
        }
    }
}
