using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BIMPlugins.ExtStorage.Extensions
{
    /// <summary>Extension methods for <see cref="Autodesk.Revit.UI.UIDocument"/>: getting the selection and prompting the user to pick elements and points.</summary>
    public static class UIDocumentExtensions
    {
        private class CategorySelectionFilter(BuiltInCategory category) : ISelectionFilter
        {
            public bool AllowElement(Element element) => element.GetBuiltInCategory() == category;
            public bool AllowReference(Reference reference, XYZ position) => true;
        }
        private class ClassSelectionFilter(Type type) : ISelectionFilter
        {
            public bool AllowElement(Element element) => type.IsInstanceOfType(element);
            public bool AllowReference(Reference reference, XYZ position) => true;
        }
        private class ClassCategorySelectionFilter(Type type, BuiltInCategory category) : ISelectionFilter
        {
            public bool AllowElement(Element element) => type.IsInstanceOfType(element) && element.GetBuiltInCategory() == category;
            public bool AllowReference(Reference reference, XYZ position) => true;
        }

        /// <summary>Returns the set of elements that are currently selected.</summary>
        /// <returns>The collection of <see cref="Autodesk.Revit.DB.Element"/> objects that are currently selected.</returns>
        public static ICollection<Element> ToSelectedElements(this UIDocument uiDoc) => uiDoc.Selection.GetElementIds()
            .Select(id => id.ToElement(uiDoc.Document))
            .ToList();


        /// <summary>Prompts the user to pick a point on the active work plane while showing a custom status prompt string.</summary>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>The picked point.</returns>
        public static XYZ PickPoint(this UIDocument uiDoc, string statusPrompt)
        {
            try
            {
                return uiDoc.Selection.PickPoint(statusPrompt);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return null;
            }
        }

        /// <summary>Prompts the user to pick a point on the active work plane using the specified snap settings while showing a custom status prompt string.</summary>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="snapSettings">The snap types available while picking.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>The picked point.</returns>
        public static XYZ PickPoint(this UIDocument uiDoc, ObjectSnapTypes snapSettings, string statusPrompt)
        {
            try
            {
                return uiDoc.Selection.PickPoint(snapSettings, statusPrompt);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return null;
            }
        }


        /// <summary>Prompts the user to select one object while showing a custom status prompt string.</summary>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="objectType">The type of objects to pick: elements, faces, edges or points on elements.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>The reference to the selected object.</returns>
        public static Reference PickObject(this UIDocument uiDoc, ObjectType objectType, string statusPrompt)
        {
            try
            {
                return uiDoc.Selection.PickObject(objectType, statusPrompt);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return null;
            }
        }

        /// <summary>Prompts the user to select one object which passes a custom filter while showing a custom status prompt string.</summary>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="objectType">The type of objects to pick: elements, faces, edges or points on elements.</param>
        /// <param name="filter">The selection filter.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>The reference to the selected object.</returns>
        public static Reference PickObject(this UIDocument uiDoc, ObjectType objectType, ISelectionFilter filter, string statusPrompt)
        {
            try
            {
                return uiDoc.Selection.PickObject(objectType, filter, statusPrompt);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return null;
            }
        }


        /// <summary>Prompts the user to select multiple objects while showing a custom status prompt string.</summary>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="objectType">The type of objects to pick: elements, faces, edges or points on elements.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>The references to the selected objects.</returns>
        public static IList<Reference> PickObjects(this UIDocument uiDoc, ObjectType objectType, string statusPrompt)
        {
            try
            {
                return uiDoc.Selection.PickObjects(objectType, statusPrompt);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return null;
            }
        }

        /// <summary>Prompts the user to select multiple objects which pass a custom filter while showing a custom status prompt string.</summary>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="objectType">The type of objects to pick: elements, faces, edges or points on elements.</param>
        /// <param name="filter">The selection filter.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>The references to the selected objects.</returns>
        public static IList<Reference> PickObjects(this UIDocument uiDoc, ObjectType objectType, ISelectionFilter filter, string statusPrompt)
        {
            try
            {
                return uiDoc.Selection.PickObjects(objectType, filter, statusPrompt);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return null;
            }
        }


        /// <summary>Prompts the user to select one element while showing a custom status prompt string.</summary>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>An element selected by user.</returns>
        public static Element PickElement(this UIDocument uiDoc, string statusPrompt)
        {
            return uiDoc.PickObject(ObjectType.Element, statusPrompt)?
                .ToElement(uiDoc.Document);
        }

        /// <summary>Prompts the user to select one element which passes a custom filter while showing a custom status prompt string.</summary>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="filter">The selection filter.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>An element selected by user.</returns>
        public static Element PickElement(this UIDocument uiDoc, ISelectionFilter filter, string statusPrompt)
        {
            return uiDoc.PickObject(ObjectType.Element, filter, statusPrompt)?
                .ToElement(uiDoc.Document);
        }

        /// <summary>Prompts the user to select one element filtered by the specified category while showing a custom status prompt string.</summary>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="category">The built-in category to filter the selection.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>An element selected by user.</returns>
        public static Element PickElement(this UIDocument uiDoc, BuiltInCategory category, string statusPrompt)
        {
            return uiDoc.PickElement(new CategorySelectionFilter(category), statusPrompt);
        }


        /// <summary>Prompts the user to select one element filtered by the specified element type and a custom filter while showing a custom status prompt string.</summary>
        /// <typeparam name="T">The element type to filter the selection (must inherit from Element).</typeparam>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="filter">The selection filter.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>An element selected by user cast to type <typeparamref name="T"/>.</returns>
        public static T PickElement<T>(this UIDocument uiDoc, ISelectionFilter filter, string statusPrompt) where T : Element
        {
            return uiDoc.PickElement(filter, statusPrompt) as T;
        }

        /// <summary>Prompts the user to select one element filtered by the specified element type while showing a custom status prompt string.</summary>
        /// <typeparam name="T">The element type to filter the selection (must inherit from Element).</typeparam>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>An element selected by user cast to type <typeparamref name="T"/>.</returns>
        public static T PickElement<T>(this UIDocument uiDoc, string statusPrompt) where T : Element
        {
            return uiDoc.PickElement<T>(new ClassSelectionFilter(typeof(T)), statusPrompt);
        }

        /// <summary>Prompts the user to select one element filtered by the specified element type and category while showing a custom status prompt string.</summary>
        /// <typeparam name="T">The element type to filter the selection (must inherit from Element).</typeparam>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="category">The built-in category to filter the selection.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>An element selected by user cast to type <typeparamref name="T"/>.</returns>
        public static T PickElement<T>(this UIDocument uiDoc, BuiltInCategory category, string statusPrompt) where T : Element
        {
            return uiDoc.PickElement<T>(new ClassCategorySelectionFilter(typeof(T), category), statusPrompt);
        }


        /// <summary>Prompts the user to select multiple elements while showing a custom status prompt string.</summary>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>A collection of elements selected by the user.</returns>
        public static IList<Element> PickElements(this UIDocument uiDoc, string statusPrompt)
        {
            var doc = uiDoc.Document;

            return uiDoc.PickObjects(ObjectType.Element, statusPrompt)?
                .Select(r => r.ToElement(doc))
                .ToList();
        }

        /// <summary>Prompts the user to select multiple elements which pass a custom filter while showing a custom status prompt string.</summary>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="filter">The selection filter.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>A collection of elements selected by the user.</returns>
        public static IList<Element> PickElements(this UIDocument uiDoc, ISelectionFilter filter, string statusPrompt)
        {
            var doc = uiDoc.Document;

            return uiDoc.PickObjects(ObjectType.Element, filter, statusPrompt)?
                .Select(r => r.ToElement(doc))
                .ToList();
        }

        /// <summary>Prompts the user to select multiple elements filtered by the specified category while showing a custom status prompt string.</summary>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="category">The built-in category to filter the selection.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>A collection of elements selected by user.</returns>
        public static IList<Element> PickElements(this UIDocument uiDoc, BuiltInCategory category, string statusPrompt)
        {
            return uiDoc.PickElements(new CategorySelectionFilter(category), statusPrompt);
        }


        /// <summary>Prompts the user to select multiple elements filtered by the specified element type and a custom filter while showing a custom status prompt string.</summary>
        /// <typeparam name="T">The element type to filter the selection (must inherit from Element).</typeparam>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="filter">The selection filter.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>A collection of elements of type <typeparamref name="T"/> selected by user.</returns>
        public static IList<T> PickElements<T>(this UIDocument uiDoc, ISelectionFilter filter, string statusPrompt) where T : Element
        {
            return uiDoc.PickElements(filter, statusPrompt)?
                .Cast<T>()
                .ToList();
        }

        /// <summary>Prompts the user to select multiple elements filtered by the specified element type while showing a custom status prompt string.</summary>
        /// <typeparam name="T">The element type to filter the selection (must inherit from Element).</typeparam>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>A collection of elements of type <typeparamref name="T"/> selected by user.</returns>
        public static IList<T> PickElements<T>(this UIDocument uiDoc, string statusPrompt) where T : Element
        {
            return uiDoc.PickElements<T>(new ClassSelectionFilter(typeof(T)), statusPrompt);
        }

        /// <summary>Prompts the user to select multiple elements filtered by the specified element type and category while showing a custom status prompt string.</summary>
        /// <typeparam name="T">The element type to filter the selection (must inherit from Element).</typeparam>
        /// <param name="uiDoc">The UI document.</param>
        /// <param name="category">The built-in category to filter the selection.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>A collection of elements of type <typeparamref name="T"/> selected by user.</returns>
        public static IList<T> PickElements<T>(this UIDocument uiDoc, BuiltInCategory category, string statusPrompt) where T : Element
        {
            return uiDoc.PickElements<T>(new ClassCategorySelectionFilter(typeof(T), category), statusPrompt);
        }
    }
}