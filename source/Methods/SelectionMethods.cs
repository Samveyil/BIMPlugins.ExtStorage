using Autodesk.Revit.DB;
using BIMPlugins.ExtStorage.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace BIMPlugins.ExtStorage.Methods
{
    /// <summary>Helpers for selecting worksets and categories.</summary>
    public static class SelectionMethods
    {
        /// <summary>Finds a workset by name and returns its id.</summary>
        /// <param name="worksets">The worksets to search, for example from a <see cref="Autodesk.Revit.DB.FilteredWorksetCollector"/>.</param>
        /// <param name="name">The workset name. The comparison is case-sensitive.</param>
        /// <returns>The integer value of the workset id, or <c>-1</c> if no workset with this name exists.</returns>
        public static int GetWorksetId(IEnumerable<Workset> worksets, string name)
        {
            return worksets.FirstOrDefault(ws => ws.Name == name)?.Id.IntegerValue ?? -1;
        }

        /// <summary>Creates a filter that passes elements of all model categories of the document.</summary>
        /// <param name="document">The document whose categories are used.</param>
        /// <remarks>Piping systems, duct systems, model lines and shaft openings are excluded.</remarks>
        /// <returns>An <see cref="Autodesk.Revit.DB.ElementMulticategoryFilter"/> with the model categories.</returns>
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
    }
}