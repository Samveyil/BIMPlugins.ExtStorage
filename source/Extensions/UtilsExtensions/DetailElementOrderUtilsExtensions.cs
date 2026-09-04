using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.Extensions.UtilsExtensions
{
    public static class DetailElementOrderUtilsExtensions
    {
        /// <inheritdoc cref="Autodesk.Revit.DB.DetailElementOrderUtils.BringForward(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.View,Autodesk.Revit.DB.ElementId)" />
        public static void BringForward(this Element detailElement, View view)
        {
            DetailElementOrderUtils.BringForward(detailElement.Document, view, detailElement.Id);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.DetailElementOrderUtils.BringToFront(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.View,Autodesk.Revit.DB.ElementId)" />
        public static void BringToFront(this Element detailElement, View view)
        {
            DetailElementOrderUtils.BringToFront(detailElement.Document, view, detailElement.Id);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.DetailElementOrderUtils.SendBackward(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.View,Autodesk.Revit.DB.ElementId)" />
        public static void SendBackward(this Element detailElement, View view)
        {
            DetailElementOrderUtils.SendBackward(detailElement.Document, view, detailElement.Id);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.DetailElementOrderUtils.SendToBack(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.View,Autodesk.Revit.DB.ElementId)" />
        public static void SendToBack(this Element detailElement, View view)
        {
            DetailElementOrderUtils.SendToBack(detailElement.Document, view, detailElement.Id);
        }


        /// <inheritdoc cref="Autodesk.Revit.DB.DetailElementOrderUtils.BringForward(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.View,Autodesk.Revit.DB.ElementId)" />
        public static void BringForward(this ElementId detailElementId, Document doc, View view)
        {
            DetailElementOrderUtils.BringForward(doc, view, detailElementId);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.DetailElementOrderUtils.BringToFront(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.View,Autodesk.Revit.DB.ElementId)" />
        public static void BringToFront(this ElementId detailElementId, Document doc, View view)
        {
            DetailElementOrderUtils.BringToFront(doc, view, detailElementId);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.DetailElementOrderUtils.SendBackward(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.View,Autodesk.Revit.DB.ElementId)" />
        public static void SendBackward(this ElementId detailElementId, Document doc, View view)
        {
            DetailElementOrderUtils.SendBackward(doc, view, detailElementId);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.DetailElementOrderUtils.SendToBack(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.View,Autodesk.Revit.DB.ElementId)" />
        public static void SendToBack(this ElementId detailElementId, Document doc, View view)
        {
            DetailElementOrderUtils.SendToBack(doc, view, detailElementId);
        }


        /// <inheritdoc cref="Autodesk.Revit.DB.DetailElementOrderUtils.BringForward(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.View,Autodesk.Revit.DB.ElementId)" />
        public static void BringForward(this ICollection<ElementId> detailElementIds, Document doc, View view)
        {
            DetailElementOrderUtils.BringForward(doc, view, detailElementIds);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.DetailElementOrderUtils.BringToFront(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.View,Autodesk.Revit.DB.ElementId)" />
        public static void BringToFront(this ICollection<ElementId> detailElementIds, Document doc, View view)
        {
            DetailElementOrderUtils.BringToFront(doc, view, detailElementIds);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.DetailElementOrderUtils.SendBackward(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.View,Autodesk.Revit.DB.ElementId)" />
        public static void SendBackward(this ICollection<ElementId> detailElementIds, Document doc, View view)
        {
            DetailElementOrderUtils.SendBackward(doc, view, detailElementIds);
        }

        /// <inheritdoc cref="Autodesk.Revit.DB.DetailElementOrderUtils.SendToBack(Autodesk.Revit.DB.Document,Autodesk.Revit.DB.View,Autodesk.Revit.DB.ElementId)" />
        public static void SendToBack(this ICollection<ElementId> detailElementIds, Document doc, View view)
        {
            DetailElementOrderUtils.SendToBack(doc, view, detailElementIds);
        }
    }
}