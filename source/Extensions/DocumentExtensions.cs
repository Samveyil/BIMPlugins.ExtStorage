using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using BIMPlugins.ExtStorage.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        public static IEnumerable<T> ToElements<T>(this Document document, BuiltInCategory category) where T : Element
        {
            return new FilteredElementCollector(document)
                .OfClass(typeof(T))
                .OfCategory(category)
                .Cast<T>();
        }
        public static IEnumerable<T> ToElements<T>(this Document document, BuiltInCategory category, ElementFilter filter) where T : Element
        {
            return new FilteredElementCollector(document)
                .OfClass(typeof(T))
                .OfCategory(category)
                .WherePasses(filter)
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
        public static IEnumerable<T> ToElements<T>(this Document document, ElementId viewId, BuiltInCategory category) where T : Element
        {
            return new FilteredElementCollector(document, viewId)
                .OfClass(typeof(T))
                .OfCategory(category)
                .Cast<T>();
        }
        public static IEnumerable<T> ToElements<T>(this Document document, ElementId viewId, BuiltInCategory category, ElementFilter filter) where T : Element
        {
            return new FilteredElementCollector(document, viewId)
                .OfClass(typeof(T))
                .OfCategory(category)
                .WherePasses(filter)
                .Cast<T>();
        }
        public static ICollection<ElementId> ToElementIds<T>(this Document document)
        {
            return new FilteredElementCollector(document)
                .OfClass(typeof(T))
                .ToElementIds();
        }
        public static ICollection<ElementId> ToElementIds<T>(this Document document, BuiltInCategory category)
        {
            return new FilteredElementCollector(document)
                .OfClass(typeof(T))
                .OfCategory(category)
                .ToElementIds();
        }
        public static ICollection<ElementId> ToElementIds<T>(this Document document, BuiltInCategory category, ElementFilter filter)
        {
            return new FilteredElementCollector(document)
                .OfClass(typeof(T))
                .OfCategory(category)
                .WherePasses(filter)
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
        public static ICollection<ElementId> ToElementIds<T>(this Document document, ElementId viewId, BuiltInCategory category)
        {
            return new FilteredElementCollector(document, viewId)
                .OfClass(typeof(T))
                .OfCategory(category)
                .ToElementIds();
        }
        public static ICollection<ElementId> ToElementIds<T>(this Document document, ElementId viewId, BuiltInCategory category, ElementFilter filter)
        {
            return new FilteredElementCollector(document, viewId)
                .OfClass(typeof(T))
                .OfCategory(category)
                .WherePasses(filter)
                .ToElementIds();
        }

        public static IList<Element> ToElements(this Document document, ElementFilter filter, bool toTypes=false)
        {
            var collector = toTypes
                ? new FilteredElementCollector(document).WhereElementIsElementType()
                : new FilteredElementCollector(document).WhereElementIsNotElementType();

            return collector
                .WherePasses(filter)
                .ToElements();
        }
        public static IList<Element> ToElements(this Document document, BuiltInCategory category, bool toTypes = false)
        {
            return toTypes
                ? new FilteredElementCollector(document)
                    .OfCategory(category)
                    .WhereElementIsElementType()
                    .ToElements()
                : new FilteredElementCollector(document)
                    .OfCategory(category)
                    .WhereElementIsNotElementType()
                    .ToElements();
        }
        public static IList<Element> ToElements(this Document document, BuiltInCategory category, ElementFilter filter, bool toTypes = false)
        {
            return toTypes
                ? new FilteredElementCollector(document)
                    .OfCategory(category)
                    .WhereElementIsElementType()
                    .WherePasses(filter)
                    .ToElements()
                : new FilteredElementCollector(document)
                    .OfCategory(category)
                    .WhereElementIsNotElementType()
                    .WherePasses(filter)
                    .ToElements();
        }
        public static IList<Element> ToElements(this Document document, ElementId viewId, BuiltInCategory category, bool toTypes = false)
        {
            return toTypes
                ? new FilteredElementCollector(document, viewId)
                    .OfCategory(category)
                    .WhereElementIsElementType()
                    .ToElements()
                : new FilteredElementCollector(document, viewId)
                    .OfCategory(category)
                    .WhereElementIsNotElementType()
                    .ToElements();
        }
        public static IList<Element> ToElements(this Document document, ElementId viewId, BuiltInCategory category, ElementFilter filter, bool toTypes = false)
        {
            return toTypes
                ? new FilteredElementCollector(document, viewId)
                    .OfCategory(category)
                    .WhereElementIsElementType()
                    .WherePasses(filter)
                    .ToElements()
                : new FilteredElementCollector(document, viewId)
                    .OfCategory(category)
                    .WhereElementIsNotElementType()
                    .WherePasses(filter)
                    .ToElements();
        }
        
        public static ICollection<ElementId> ToElementIds(this Document document, ElementFilter filter, bool toTypes = false)
        {
            var collector = toTypes
                ? new FilteredElementCollector(document).WhereElementIsElementType()
                : new FilteredElementCollector(document).WhereElementIsNotElementType();

            return collector
                .WherePasses(filter)
                .ToElementIds();
        }
        public static ICollection<ElementId> ToElementIds(this Document document, BuiltInCategory category, bool toTypes = false)
        {
            return toTypes
                ? new FilteredElementCollector(document)
                    .OfCategory(category)
                    .WhereElementIsElementType()
                    .ToElementIds()
                : new FilteredElementCollector(document)
                    .OfCategory(category)
                    .WhereElementIsNotElementType()
                    .ToElementIds();
        }
        public static ICollection<ElementId> ToElementIds(this Document document, BuiltInCategory category, ElementFilter filter, bool toTypes = false)
        {
            return toTypes
                ? new FilteredElementCollector(document)
                    .OfCategory(category)
                    .WhereElementIsElementType()
                    .WherePasses(filter)
                    .ToElementIds()
                : new FilteredElementCollector(document)
                    .OfCategory(category)
                    .WhereElementIsNotElementType()
                    .WherePasses(filter)
                    .ToElementIds();
        }
        public static ICollection<ElementId> ToElementIds(this Document document, ElementId viewId, BuiltInCategory category, bool toTypes = false)
        {
            return toTypes
                ? new FilteredElementCollector(document, viewId)
                    .OfCategory(category)
                    .WhereElementIsElementType()
                    .ToElementIds()
                : new FilteredElementCollector(document, viewId)
                    .OfCategory(category)
                    .WhereElementIsNotElementType()
                    .ToElementIds();
        }
        public static ICollection<ElementId> ToElementIds(this Document document, ElementId viewId, BuiltInCategory category, ElementFilter filter, bool toTypes = false)
        {
            return toTypes
                ? new FilteredElementCollector(document, viewId)
                    .OfCategory(category)
                    .WhereElementIsElementType()
                    .WherePasses(filter)
                    .ToElementIds()
                : new FilteredElementCollector(document, viewId)
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

        public static void PurgeUnused(this Document document)
        {
            RevitAPI.UIApplication.DialogBoxShowing += new EventHandler<DialogBoxShowingEventArgs>(ExMethods.WarningDialogHide);

            string desiredRule = "Проект содержит неиспользуемые семейства и типоразмеры";

            PerformanceAdviser perfAdviser = PerformanceAdviser.GetPerformanceAdviser();

            IList<PerformanceAdviserRuleId> allRulesList = perfAdviser.GetAllRuleIds();
            IList<PerformanceAdviserRuleId> rulesToExecute = new List<PerformanceAdviserRuleId>();
            foreach (PerformanceAdviserRuleId r in allRulesList)
            {
                if (perfAdviser.GetRuleName(r).Equals(desiredRule))
                    rulesToExecute.Add(r);
            }

            for (int i = 0; i < 3; i++)
            {
                IList<FailureMessage> failureMessages = perfAdviser.ExecuteRules(document, rulesToExecute);
                if (failureMessages.Count() != 0)
                {
                    ICollection<ElementId> failingElementsIds = failureMessages[0].GetFailingElements();
                    using (Transaction t = new Transaction(document, "Удалить неиспользуемое"))
                    {
                        t.Start();

                        foreach (ElementId eid in failingElementsIds)
                        {
                            try
                            {
                                document.Delete(eid);
                            }
                            catch { }
                        }

                        t.Commit();
                    }
                }

                List<ElementId> unusedAssetIds = [];

                try
                {
                    AddUnusedAssets(document, GetUnusedAssets(document, "GetUnusedMaterials"), unusedAssetIds);
                    AddUnusedAssets(document, GetUnusedAssets(document, "GetUnusedAppearances"), unusedAssetIds);
                    AddUnusedAssets(document, GetUnusedAssets(document, "GetUnusedStructures"), unusedAssetIds);
                    AddUnusedAssets(document, GetUnusedAssets(document, "GetUnusedThermals"), unusedAssetIds);

                    using (Transaction t = new Transaction(document, "Удалить неиспользуемое"))
                    {
                        t.Start();

                        document.Delete(unusedAssetIds);

                        t.Commit();
                    }
                }
                catch { }
            }

            RevitAPI.UIApplication.DialogBoxShowing -= new EventHandler<DialogBoxShowingEventArgs>(ExMethods.WarningDialogHide);
        }
        private static ICollection<ElementId> GetUnusedAssets(Document doc, string methodName)
        {
            MethodInfo method = typeof(Document).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
                return (ICollection<ElementId>)method.Invoke(doc, null);
            return new List<ElementId>();
        }
        private static void AddUnusedAssets(Document doc, ICollection<ElementId> elementIds, List<ElementId> ids)
        {
            foreach (var id in elementIds)
            {
                Element elem = doc.GetElement(id);
                if (elem != null)
                    ids.Add(id);
            }
        }
    }
}