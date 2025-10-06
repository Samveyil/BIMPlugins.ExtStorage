using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using BIMPlugins.ExtStorage.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BIMPlugins.ExtStorage.Methods
{
    public static class SelectionMethods
    {
        public static int GetWorksetId(IEnumerable<Workset> worksets, string name)
        {
            return worksets.FirstOrDefault(ws => ws.Name == name)?.Id.IntegerValue ?? -1;
        }

        private class CategorySelectionFilter : ISelectionFilter
        {
            private readonly BuiltInCategory _category;

            public CategorySelectionFilter(BuiltInCategory category)
            {
                _category = category;
            }

            public bool AllowElement(Element element)
            {
                return (BuiltInCategory)element.Category.Id.GetValue() == _category;
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return true;
            }
        }
        private class ClassSelectionFilter : ISelectionFilter
        {
            private readonly Type _type;

            public ClassSelectionFilter(Type type)
            {
                _type = type;
            }

            public bool AllowElement(Element element)
            {
                return _type.IsInstanceOfType(element);
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return true;
            }
        }

        public static Element PickObject(string statusPrompt)
        {
            Reference reference;
            try
            {
                reference = RevitAPI.UIDocument.Selection.PickObject(ObjectType.Element, statusPrompt);
            }
            catch (Exception)
            {
                return null;
            }

            return RevitAPI.Document.GetElement(reference);
        }
        public static Element PickObject(ISelectionFilter filter, string statusPrompt)
        {
            Reference reference;
            try
            {
                reference = RevitAPI.UIDocument.Selection.PickObject(ObjectType.Element, filter, statusPrompt);
            }
            catch (Exception)
            {
                return null;
            }

            return RevitAPI.Document.GetElement(reference);
        }
        public static Element PickObject(BuiltInCategory category, string statusPrompt)
        {
            CategorySelectionFilter filter = new CategorySelectionFilter(category);

            Reference reference;
            try
            {
                reference = RevitAPI.UIDocument.Selection.PickObject(ObjectType.Element, filter, statusPrompt);
            }
            catch (Exception)
            {
                return null;
            }

            return RevitAPI.Document.GetElement(reference);
        }
        public static Element PickObject(Type type, string statusPrompt)
        {
            ClassSelectionFilter filter = new ClassSelectionFilter(type);

            Reference reference;
            try
            {
                reference = RevitAPI.UIDocument.Selection.PickObject(ObjectType.Element, filter, statusPrompt);
            }
            catch (Exception)
            {
                return null;
            }

            return RevitAPI.Document.GetElement(reference);
        }

        public static List<Element> PickObjects(string statusPrompt)
        {
            IList<Reference> references;
            try
            {
                references = RevitAPI.UIDocument.Selection.PickObjects(ObjectType.Element, statusPrompt);
            }
            catch (Exception)
            {
                return null;
            }

            return references.Select(reference => RevitAPI.Document.GetElement(reference)).ToList();
        }
        public static List<Element> PickObjects(ISelectionFilter filter, string statusPrompt)
        {
            IList<Reference> references;
            try
            {
                references = RevitAPI.UIDocument.Selection.PickObjects(ObjectType.Element, filter, statusPrompt);
            }
            catch (Exception)
            {
                return null;
            }

            return references.Select(reference => RevitAPI.Document.GetElement(reference)).ToList();
        }
        public static List<Element> PickObjects(BuiltInCategory category, string statusPrompt)
        {
            CategorySelectionFilter filter = new CategorySelectionFilter(category);

            IList<Reference> references;
            try
            {
                references = RevitAPI.UIDocument.Selection.PickObjects(ObjectType.Element, filter, statusPrompt);
            }
            catch (Exception)
            {
                return null;
            }

            return references.Select(reference => RevitAPI.Document.GetElement(reference)).ToList();
        }
        public static List<Element> PickObjects(Type type, string statusPrompt)
        {
            ClassSelectionFilter filter = new ClassSelectionFilter(type);

            IList<Reference> references;
            try
            {
                references = RevitAPI.UIDocument.Selection.PickObjects(ObjectType.Element, filter, statusPrompt);
            }
            catch (Exception)
            {
                return null;
            }

            return references.Select(reference => RevitAPI.Document.GetElement(reference)).ToList();
        }

        public static ElementMulticategoryFilter ModelCategoryFilter(Document document)
        {
            var builtInCategories = new List<BuiltInCategory>();
            var excludedCategories = new HashSet<BuiltInCategory>
            {
                BuiltInCategory.OST_PipingSystem,
                BuiltInCategory.OST_DuctSystem,
                BuiltInCategory.OST_Lines,
                BuiltInCategory.OST_ShaftOpening
            };

            var iter = document.Settings.Categories.ForwardIterator();
            while (iter.MoveNext())
            {
                if (iter.Current is Category category && category.CategoryType == CategoryType.Model)
                {
                    var builtInCategory = (BuiltInCategory)category.Id.GetValue();
                    
                    if (!excludedCategories.Contains(builtInCategory))
                    {
                        builtInCategories.Add(builtInCategory);
                    }
                }
            }

            return new ElementMulticategoryFilter(builtInCategories);
        }

        public static List<Element> GetIfcElements(Document doc)
        {
            List<Element> elems = new List<Element>();

            if (doc.Title.Contains("ВК"))
            {
                foreach (string viewName in new List<string>() { "IFC Export_Водоснабжение", "IFC Export_Канализация", "IFC Export_АУПТ" })
                {
                    View3D view = new FilteredElementCollector(doc)
                        .OfClass(typeof(View3D))
                        .Cast<View3D>()
                        .FirstOrDefault(v => !v.IsTemplate && v.Name == viewName);

                    if (view == null) { continue; }

                    foreach (Element element in new FilteredElementCollector(doc, view.Id)
                            .WhereElementIsNotElementType()
                            .WhereElementIsViewIndependent()
                            .WherePasses(new LogicalAndFilter(IFCParameterFilter(doc), IgnoredCategoryFilter())))
                    {
                        elems.Add(element);
                    }
                }
            }
            else if (doc.Title.Contains("ОВ"))
            {
                foreach (string viewName in new List<string>() { "IFC Export_Вентиляция", "IFC Export_Отопление, ХC, ТС" })
                {
                    View3D view = new FilteredElementCollector(doc)
                        .OfClass(typeof(View3D))
                        .Cast<View3D>()
                        .FirstOrDefault(v => !v.IsTemplate && v.Name == viewName);

                    foreach (Element element in new FilteredElementCollector(doc, view.Id)
                            .WhereElementIsNotElementType()
                            .WhereElementIsViewIndependent()
                            .WherePasses(new LogicalAndFilter(IFCParameterFilter(doc), IgnoredCategoryFilter())))
                    {
                        elems.Add(element);
                    }
                }
            }
            else
            {
                View3D view = new FilteredElementCollector(doc)
                    .OfClass(typeof(View3D))
                    .Cast<View3D>()
                    .FirstOrDefault(v => !v.IsTemplate && v.Name == "IFC Export");

                elems = new FilteredElementCollector(doc, view.Id)
                    .WhereElementIsNotElementType()
                    .WhereElementIsViewIndependent()
                    .WherePasses(new LogicalAndFilter(IFCParameterFilter(doc), IgnoredCategoryFilter()))
                    .ToList();
            }

            return elems;
        }
        private static ElementParameterFilter IFCParameterFilter(Document doc)
        {
            var paramId = new FilteredElementCollector(doc).OfClass(typeof(SharedParameterElement)).Cast<SharedParameterElement>()
                .Where(p => p.GuidValue.ToString() == "e9663bf5-cf10-4f72-bb2d-ee885c24b4cb").Select(p => p.Id).First();

            return paramId.CreateContainsFilter("Ifc");
        }
        private static ElementMulticategoryFilter IgnoredCategoryFilter()
        {
            List<BuiltInCategory> builtInCategories = new List<BuiltInCategory>()
            {
                BuiltInCategory.OST_PipingSystem,
                BuiltInCategory.OST_DuctSystem,
                BuiltInCategory.OST_Lines,
                BuiltInCategory.OST_ShaftOpening,
                BuiltInCategory.OST_PipeFittingCenterLine,
                BuiltInCategory.OST_PipeCurvesCenterLine,
                BuiltInCategory.OST_DuctCurvesCenterLine,
                BuiltInCategory.OST_DuctFittingCenterLine,
            };

            ElementMulticategoryFilter elementMulticategoryFilter = new ElementMulticategoryFilter(builtInCategories, true);
            return elementMulticategoryFilter;
        }
    }
}