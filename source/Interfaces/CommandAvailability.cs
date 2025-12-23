using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BIMPlugins.ExtStorage.Interfaces
{
    public class AlwaysAvailable : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories) => applicationData != null;
    }

    public class NotAvailableInFamilyEditor : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            var doc = applicationData.ActiveUIDocument?.Document;
            return doc != null && !doc.IsFamilyDocument;
        }
    }
}

