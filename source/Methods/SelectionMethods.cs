using Autodesk.Revit.DB;
using BIMPlugins.ExtStorage.Extensions;
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