using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BIMPlugins.ExtStorage.Extensions
{
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
        public static IEnumerable<Element> ToSelectedElements(this UIDocument uiDoc) => uiDoc.Selection.GetElementIds().Select(id => id.ToElement());

        /// <summary>Prompts the user to select one object while showing a custom status prompt string.</summary>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>An element selected by user.</returns>
        public static Element PickObject(this UIDocument uiDoc, string statusPrompt)
        {
            try
            {
                return uiDoc.Selection.PickObject(ObjectType.Element, statusPrompt).ToElement();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Prompts the user to select one object which passes a custom filter while showing a custom status prompt string.</summary>
        /// <param name="filter">The selection filter.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>An element selected by user.</returns>
        public static Element PickObject(this UIDocument uiDoc, ISelectionFilter filter, string statusPrompt)
        {
            try
            {
                return uiDoc.Selection.PickObject(ObjectType.Element, filter, statusPrompt).ToElement();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Prompts the user to select one object filtered by the specified category while showing a custom status prompt string.</summary>
        /// <param name="category">The built-in category to filter the selection.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>An element selected by user.</returns>
        public static Element PickObject(this UIDocument uiDoc, BuiltInCategory category, string statusPrompt)
        {
            try
            {
                return uiDoc.Selection.PickObject(ObjectType.Element, new CategorySelectionFilter(category), statusPrompt).ToElement();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Prompts the user to select one object filtered by the specified element type while showing a custom status prompt string.</summary>
        /// <typeparam name="T">The element type to filter the selection (must inherit from Element).</typeparam>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>An element selected by user cast to type <typeparamref name="T"/>.</returns>
        public static T PickObject<T>(this UIDocument uiDoc, string statusPrompt) where T : Element
        {
            try
            {
                return uiDoc.Selection.PickObject(ObjectType.Element, new ClassSelectionFilter(typeof(T)), statusPrompt).ToElement<T>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Prompts the user to select one object filtered by the specified element type and category while showing a custom status prompt string.</summary>
        /// <typeparam name="T">The element type to filter the selection (must inherit from Element).</typeparam>
        /// <param name="category">The built-in category to filter the selection.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>An element selected by user cast to type <typeparamref name="T"/>.</returns>
        public static T PickObject<T>(this UIDocument uiDoc, BuiltInCategory category, string statusPrompt) where T : Element
        {
            try
            {
                return uiDoc.Selection.PickObject(ObjectType.Element, new ClassCategorySelectionFilter(typeof(T), category), statusPrompt).ToElement<T>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Prompts the user to select one object filtered by the specified element type and a custom filter while showing a custom status prompt string.</summary>
        /// <typeparam name="T">The element type to filter the selection (must inherit from Element).</typeparam>
        /// <param name="filter">The selection filter.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>An element selected by user cast to type <typeparamref name="T"/>.</returns>
        public static T PickObject<T>(this UIDocument uiDoc, ISelectionFilter filter, string statusPrompt) where T : Element
        {
            try
            {
                return uiDoc.Selection.PickObject(ObjectType.Element, filter, statusPrompt).ToElement() as T;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>Prompts the user to select multiple objects while showing a custom status prompt string.</summary>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>A collection of elements selected by the user.</returns>
        public static IEnumerable<Element> PickObjects(this UIDocument uiDoc, string statusPrompt)
        {
            try
            {
                return uiDoc.Selection.PickObjects(ObjectType.Element, statusPrompt).Select(r => r.ToElement());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Prompts the user to select multiple objects which pass a custom filter while showing a custom status prompt string.</summary>
        /// <param name="filter">The selection filter.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>A collection of elements selected by the user.</returns>
        public static IEnumerable<Element> PickObjects(this UIDocument uiDoc, ISelectionFilter filter, string statusPrompt)
        {
            try
            {
                return uiDoc.Selection.PickObjects(ObjectType.Element, filter, statusPrompt).Select(r => r.ToElement());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Prompts the user to select multiple objects filtered by the specified category while showing a custom status prompt string.</summary>
        /// <param name="category">The built-in category to filter the selection.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>A collection of elements selected by user.</returns>
        public static IEnumerable<Element> PickObjects(this UIDocument uiDoc, BuiltInCategory category, string statusPrompt)
        {
            try
            {
                return uiDoc.Selection.PickObjects(ObjectType.Element, new CategorySelectionFilter(category), statusPrompt).Select(r => r.ToElement());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Prompts the user to select multiple objects filtered by the specified element type while showing a custom status prompt string.</summary>
        /// <typeparam name="T">The element type to filter the selection (must inherit from Element).</typeparam>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>A collection of elements of type <typeparamref name="T"/> selected by user.</returns>
        public static IEnumerable<T> PickObjects<T>(this UIDocument uiDoc, string statusPrompt) where T : Element
        {
            try
            {
                return uiDoc.Selection.PickObjects(ObjectType.Element, new ClassSelectionFilter(typeof(T)), statusPrompt).Select(r => r.ToElement<T>());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Prompts the user to select multiple objects filtered by the specified element type and category while showing a custom status prompt string.</summary>
        /// <typeparam name="T">The element type to filter the selection (must inherit from Element).</typeparam>
        /// <param name="category">The built-in category to filter the selection.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>A collection of elements of type <typeparamref name="T"/> selected by user.</returns>
        public static IEnumerable<T> PickObjects<T>(this UIDocument uiDoc, BuiltInCategory category, string statusPrompt) where T : Element
        {
            try
            {
                return uiDoc.Selection.PickObjects(ObjectType.Element, new ClassCategorySelectionFilter(typeof(T), category), statusPrompt).Select(r => r.ToElement<T>());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Prompts the user to select multiple objects filtered by the specified element type and a custom filter while showing a custom status prompt string.</summary>
        /// <typeparam name="T">The element type to filter the selection (must inherit from Element).</typeparam>
        /// <param name="filter">The selection filter.</param>
        /// <param name="statusPrompt">The message shown on the status bar.</param>
        /// <remarks>Note: if the user cancels the operation (for example, through ESC), the method will return <see langword="null"/>.</remarks>
        /// <returns>A collection of elements of type <typeparamref name="T"/> selected by user.</returns>
        public static IEnumerable<T> PickObjects<T>(this UIDocument uiDoc, ISelectionFilter filter, string statusPrompt) where T : Element
        {
            try
            {
                return uiDoc.Selection.PickObjects(ObjectType.Element, filter, statusPrompt).Select(r => r.ToElement<T>());
            }
            catch
            {
                return null;
            }
        }
    }
}