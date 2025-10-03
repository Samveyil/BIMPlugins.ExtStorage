using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class DocumentExtensions
    {
        public static void SynchronizeWithCentral(this Document document, string comment)
        {
            TransactWithCentralOptions transact = new TransactWithCentralOptions();
            SynchronizeWithCentralOptions synchronize = new SynchronizeWithCentralOptions();
            RelinquishOptions relinquishOptions = new RelinquishOptions(true);
            synchronize.SetRelinquishOptions(relinquishOptions);
            synchronize.Comment = comment;

            document.SynchronizeWithCentral(transact, synchronize);
        }

        public static View3D GetView3D(this Document document, string viewName, bool setAsActive=true)
        {
            var view3D = new FilteredElementCollector(document)
                .OfClass(typeof(View3D))
                .Where(e => e.Name == viewName)
                .FirstOrDefault() as View3D;

            view3D ??= document.CreateView3D(viewName);

            if (setAsActive)
                new UIDocument(document).ActiveView = view3D;

            return view3D;
        }
        private static View3D CreateView3D(this Document document, string viewName)
        {
            var viewTypeId = new FilteredElementCollector(document)
                .OfClass(typeof(ViewFamilyType))
                .Cast<ViewFamilyType>()
                .FirstOrDefault(v => v.ViewFamily == ViewFamily.ThreeDimensional)
                .Id;

            View3D view3D;
            using (Transaction t = new Transaction(document, "Создать 3D вид"))
            {
                t.Start();

                view3D = View3D.CreateIsometric(document, viewTypeId);
                view3D.Name = viewName;

                t.Commit();
            }

            return view3D;
        }

        public static IEnumerable<T> ToElements<T>(this Document document) where T : Element
        {
            return new FilteredElementCollector(document)
                .OfClass(typeof(T))
                .Cast<T>();
        }
        public static IEnumerable<T> ToElements<T>(this Document document, ElementFilter filter) where T : Element
        {
            return new FilteredElementCollector(document)
                .OfClass(typeof(T))
                .WherePasses(filter)
                .Cast<T>();
        }
        public static IEnumerable<T> ToElements<T>(this Document document, ElementId viewId) where T : Element
        {
            return new FilteredElementCollector(document, viewId)
                .OfClass(typeof(T))
                .Cast<T>();
        }
        public static IEnumerable<T> ToElements<T>(this Document document, ElementId viewId, ElementFilter filter) where T : Element
        {
            return new FilteredElementCollector(document, viewId)
                .OfClass(typeof(T))
                .WherePasses(filter)
                .Cast<T>();
        }
        public static ICollection<ElementId> ToElementIds<T>(this Document document)
        {
            return new FilteredElementCollector(document)
                .OfClass(typeof(T))
                .ToElementIds();
        }
        public static ICollection<ElementId> ToElementIds<T>(this Document document, ElementFilter filter)
        {
            return new FilteredElementCollector(document)
                .OfClass(typeof(T))
                .WherePasses(filter)
                .ToElementIds();
        }
        public static ICollection<ElementId> ToElementIds<T>(this Document document, ElementId viewId)
        {
            return new FilteredElementCollector(document, viewId)
                .OfClass(typeof(T))
                .ToElementIds();
        }
        public static ICollection<ElementId> ToElementIds<T>(this Document document, ElementId viewId, ElementFilter filter)
        {
            return new FilteredElementCollector(document, viewId)
                .OfClass(typeof(T))
                .WherePasses(filter)
                .ToElementIds();
        }

        public static IList<Element> ToElements(this Document document, ElementFilter filter)
        {
            return new FilteredElementCollector(document)
                .WhereElementIsNotElementType()
                .WherePasses(filter)
                .ToElements();
        }
        public static IList<Element> ToElements(this Document document, BuiltInCategory category)
        {
            return new FilteredElementCollector(document)
                .OfCategory(category)
                .WhereElementIsNotElementType()
                .ToElements();
        }
        public static IList<Element> ToElements(this Document document, BuiltInCategory category, ElementFilter filter)
        {
            return new FilteredElementCollector(document)
                .OfCategory(category)
                .WhereElementIsNotElementType()
                .WherePasses(filter)
                .ToElements();
        }
        public static IList<Element> ToElements(this Document document, BuiltInCategory category, ElementId viewId)
        {
            return new FilteredElementCollector(document, viewId)
                .OfCategory(category)
                .WhereElementIsNotElementType()
                .ToElements();
        }
        public static IList<Element> ToElements(this Document document, BuiltInCategory category, ElementId viewId, ElementFilter filter)
        {
            return new FilteredElementCollector(document, viewId)
                .OfCategory(category)
                .WhereElementIsNotElementType()
                .WherePasses(filter)
                .ToElements();
        }
        public static ICollection<ElementId> ToElementIds(this Document document, BuiltInCategory category)
        {
            return new FilteredElementCollector(document)
                .OfCategory(category)
                .WhereElementIsNotElementType()
                .ToElementIds();
        }
        public static ICollection<ElementId> ToElementIds(this Document document, BuiltInCategory category, ElementFilter filter)
        {
            return new FilteredElementCollector(document)
                .OfCategory(category)
                .WhereElementIsNotElementType()
                .WherePasses(filter)
                .ToElementIds();
        }
        public static ICollection<ElementId> ToElementIds(this Document document, BuiltInCategory category, ElementId viewId)
        {
            return new FilteredElementCollector(document, viewId)
                .OfCategory(category)
                .WhereElementIsNotElementType()
                .ToElementIds();
        }
        public static ICollection<ElementId> ToElementIds(this Document document, BuiltInCategory category, ElementId viewId, ElementFilter filter)
        {
            return new FilteredElementCollector(document, viewId)
                .OfCategory(category)
                .WhereElementIsNotElementType()
                .WherePasses(filter)
                .ToElementIds();
        }

        public static IEnumerable<Element> ToModelElements(this Document document)
        {
            return new FilteredElementCollector(document)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .Where(e => e.Category?.CategoryType == CategoryType.Model && e.Category.CanAddSubcategory)
                .Where(e => e.GetBuiltInCategory() != BuiltInCategory.OST_Parts);
        }
        public static IEnumerable<Element> ToModelElements(this Document document, ElementFilter filter)
        {
            return new FilteredElementCollector(document)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .WherePasses(filter)
                .Where(e => e.Category?.CategoryType == CategoryType.Model && e.Category.CanAddSubcategory)
                .Where(e => e.GetBuiltInCategory() != BuiltInCategory.OST_Parts);
        }
        public static IEnumerable<Element> ToModelElements(this Document document, ElementId viewId)
        {
            return new FilteredElementCollector(document, viewId)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .Where(e => e.Category?.CategoryType == CategoryType.Model && e.Category.CanAddSubcategory)
                .Where(e => e.GetBuiltInCategory() != BuiltInCategory.OST_Parts);
        }
        public static IEnumerable<Element> ToModelElements(this Document document, ElementId viewId, ElementFilter filter)
        {
            return new FilteredElementCollector(document, viewId)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .WherePasses(filter)
                .Where(e => e.Category?.CategoryType == CategoryType.Model && e.Category.CanAddSubcategory)
                .Where(e => e.GetBuiltInCategory() != BuiltInCategory.OST_Parts);
        }
        public static ICollection<ElementId> ToModelElementIds(this Document document)
        {
            return new FilteredElementCollector(document)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .Where(e => e.Category?.CategoryType == CategoryType.Model && e.Category.CanAddSubcategory)
                .Where(e => e.GetBuiltInCategory() != BuiltInCategory.OST_Parts)
                .Select(e => e.Id)
                .ToList();
        }
        public static ICollection<ElementId> ToModelElementIds(this Document document, ElementFilter filter)
        {
            return new FilteredElementCollector(document)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .WherePasses(filter)
                .Where(e => e.Category?.CategoryType == CategoryType.Model && e.Category.CanAddSubcategory)
                .Where(e => e.GetBuiltInCategory() != BuiltInCategory.OST_Parts)
                .Select(e => e.Id)
                .ToList();
        }
        public static ICollection<ElementId> ToModelElementIds(this Document document, ElementId viewId)
        {
            return new FilteredElementCollector(document, viewId)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .Where(e => e.Category?.CategoryType == CategoryType.Model && e.Category.CanAddSubcategory)
                .Where(e => e.GetBuiltInCategory() != BuiltInCategory.OST_Parts)
                .Select(e => e.Id)
                .ToList();
        }
        public static ICollection<ElementId> ToModelElementIds(this Document document, ElementId viewId, ElementFilter filter)
        {
            return new FilteredElementCollector(document, viewId)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .WherePasses(filter)
                .Where(e => e.Category?.CategoryType == CategoryType.Model && e.Category.CanAddSubcategory)
                .Where(e => e.GetBuiltInCategory() != BuiltInCategory.OST_Parts)
                .Select(e => e.Id)
                .ToList();
        }
    }
}