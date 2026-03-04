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

        public static IEnumerable<Element> ToSelectedElements(this UIDocument uiDoc) => uiDoc.Selection.GetElementIds().Select(id => id.ToElement());

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