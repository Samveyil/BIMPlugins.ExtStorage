using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.Extensions.UtilsExtensions
{
    public static class ElementTransformUtilsExtensions
    {
        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.CopyElement(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.ElementId,Autodesk.Revit.DB.XYZ)" />
        public static ICollection<ElementId> Copy(this Element element, XYZ translation)
        {
            return ElementTransformUtils.CopyElement(element.Document, element.Id, translation);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.MirrorElement(Document, Autodesk.Revit.DB.ElementId, Autodesk.Revit.DB.Plane)" />
        public static Element Mirror(this Element element, Plane plane)
        {
            ElementTransformUtils.MirrorElement(element.Document, element.Id, plane);
            return element;
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.MoveElement(Document, Autodesk.Revit.DB.ElementId, Autodesk.Revit.DB.XYZ)" />
        public static Element Move(this Element element, XYZ translation)
        {
            ElementTransformUtils.MoveElement(element.Document, element.Id, translation);
            return element;
        }
 
        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.RotateElement(Document, Autodesk.Revit.DB.ElementId, Line, double)" />
        public static Element Rotate(this Element element, Line axis, double angle)
        {
            ElementTransformUtils.RotateElement(element.Document, element.Id, axis, angle);
            return element;
        }


        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.CopyElement(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.ElementId,Autodesk.Revit.DB.XYZ)" />
        public static ICollection<ElementId> Copy(this ElementId elementId, Document doc, XYZ translation)
        {
            return ElementTransformUtils.CopyElement(doc, elementId, translation);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.MirrorElement(Document, Autodesk.Revit.DB.ElementId, Autodesk.Revit.DB.Plane)" />
        public static ElementId Mirror(this ElementId elementId, Document doc, Plane plane)
        {
            ElementTransformUtils.MirrorElement(doc, elementId, plane);
            return elementId;
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.MoveElement(Document, Autodesk.Revit.DB.ElementId, Autodesk.Revit.DB.XYZ)" />
        public static ElementId Move(this ElementId elementId, Document doc, XYZ translation)
        {
            ElementTransformUtils.MoveElement(doc, elementId, translation);
            return elementId;
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.RotateElement(Document, Autodesk.Revit.DB.ElementId, Line, double)" />
        public static ElementId Rotate(this ElementId elementId, Document doc, Line axis, double angle)
        {
            ElementTransformUtils.RotateElement(doc, elementId, axis, angle);
            return elementId;
        }


        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.CopyElements(Document, ICollection{Autodesk.Revit.DB.ElementId}, Document, Transform, Autodesk.Revit.DB.CopyPasteOptions)" />
        public static ICollection<ElementId> Copy(this ElementId elementId,
            Document sourceDoc,
            Document destinationDoc,
            Transform transform,
            CopyPasteOptions options)
        {
            return ElementTransformUtils.CopyElements(sourceDoc, [elementId], destinationDoc, transform, options);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.CopyElements(Document, ICollection{Autodesk.Revit.DB.ElementId}, Document, Transform, Autodesk.Revit.DB.CopyPasteOptions)" />
        public static ICollection<ElementId> Copy(this ElementId elementId, Document sourceDoc, Document destinationDoc)
        {
            return ElementTransformUtils.CopyElements(sourceDoc, [elementId], destinationDoc, null, null);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.CopyElements(View, ICollection{Autodesk.Revit.DB.ElementId}, View, Transform, Autodesk.Revit.DB.CopyPasteOptions)" />
        public static ICollection<ElementId> Copy(this ElementId elementId,
            View sourceView,
            View destinationView,
            Transform transform,
            CopyPasteOptions options)
        {
            return ElementTransformUtils.CopyElements(sourceView, [elementId], destinationView, transform, options);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.CopyElements(View, ICollection{Autodesk.Revit.DB.ElementId}, View, Transform, Autodesk.Revit.DB.CopyPasteOptions)" />
        public static ICollection<ElementId> Copy(this ElementId elementId, View sourceView, View destinationView)
        {
            return ElementTransformUtils.CopyElements(sourceView, [elementId], destinationView, null, null);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.MirrorElements(Document, ICollection{Autodesk.Revit.DB.ElementId}, Autodesk.Revit.DB.Plane, bool)" />
        public static ICollection<ElementId> Mirror(this ElementId elementId, Document doc, Plane plane, bool mirrorCopies)
        {
            return ElementTransformUtils.MirrorElements(doc, [elementId], plane, mirrorCopies);
        }


        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.CopyElements(Document, ICollection{Autodesk.Revit.DB.ElementId}, Autodesk.Revit.DB.XYZ)" />
        public static ICollection<ElementId> CopyElements(this ICollection<ElementId> elements, Document doc, XYZ translation)
        {
            return ElementTransformUtils.CopyElements(doc, elements, translation);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.CopyElements(Document, ICollection{Autodesk.Revit.DB.ElementId}, Document, Transform, Autodesk.Revit.DB.CopyPasteOptions)" />
        public static ICollection<ElementId> CopyElements(this ICollection<ElementId> elements,
            Document sourceDoc,
            Document destinationDoc,
            Transform transform,
            CopyPasteOptions options)
        {
            return ElementTransformUtils.CopyElements(sourceDoc, elements, destinationDoc, transform, options);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.CopyElements(Document, ICollection{Autodesk.Revit.DB.ElementId}, Document, Transform, Autodesk.Revit.DB.CopyPasteOptions)" />
        public static ICollection<ElementId> CopyElements(this ICollection<ElementId> elements, Document sourceDoc, Document destinationDoc)
        {
            return ElementTransformUtils.CopyElements(sourceDoc, elements, destinationDoc, null, null);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.CopyElements(View, ICollection{Autodesk.Revit.DB.ElementId}, View, Transform, Autodesk.Revit.DB.CopyPasteOptions)" />
        public static ICollection<ElementId> CopyElements(this ICollection<ElementId> elements,
            View sourceView,
            View destinationView,
            Transform transform,
            CopyPasteOptions options)
        {
            return ElementTransformUtils.CopyElements(sourceView, elements, destinationView, transform, options);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.CopyElements(View, ICollection{Autodesk.Revit.DB.ElementId}, View, Transform, Autodesk.Revit.DB.CopyPasteOptions)" />
        public static ICollection<ElementId> CopyElements(this ICollection<ElementId> elements, View sourceView, View destinationView)
        {
            return ElementTransformUtils.CopyElements(sourceView, elements, destinationView, null, null);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.MirrorElements(Document, ICollection{Autodesk.Revit.DB.ElementId}, Autodesk.Revit.DB.Plane, bool)" />
        public static ICollection<ElementId> MirrorElements(this ICollection<ElementId> elements, Document doc, Plane plane, bool mirrorCopies)
        {
            return ElementTransformUtils.MirrorElements(doc, elements, plane, mirrorCopies);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.MoveElements(Document, ICollection{Autodesk.Revit.DB.ElementId}, Autodesk.Revit.DB.XYZ)" />
        public static ICollection<ElementId> MoveElements(this ICollection<ElementId> elements, Document doc, XYZ translation)
        {
            ElementTransformUtils.MoveElements(doc, elements, translation);
            return elements;
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.ElementTransformUtils.RotateElements(Document, ICollection{Autodesk.Revit.DB.ElementId}, Line, double)" />
        public static ICollection<ElementId> RotateElements(this ICollection<ElementId> elements, Document doc, Line axis, double angle)
        {
            ElementTransformUtils.RotateElements(doc, elements, axis, angle);
            return elements;
        }
    }
}