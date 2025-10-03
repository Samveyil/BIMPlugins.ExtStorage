using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.UI;
using System.IO;

namespace BIMPlugins.ExtStorage.Methods
{
    public static class ExMethods
    {
        public static string ReadTextFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return null;
        }

        public static void DialogBoxHide(object sender, DialogBoxShowingEventArgs e)
        {
            if (e is TaskDialogShowingEventArgs e2)
            {
                bool isConfirm = e2.DialogId.Contains("TaskDialog_Missing_Third_Party_Updater");
                bool isConfirm2 = e2.DialogId.Contains("TaskDialog_Unresolved_Reference");
                bool isConfirm3 = e2.Message.Contains("не может быть использовано с настройками печати");

                try
                {
                    if (isConfirm)
                    {
                        e2.OverrideResult((int)TaskDialogCommandLinkId.CommandLink1);
                    }
#if !R2025
                    if (isConfirm2)
                    {
                        e2.OverrideResult((int)System.Windows.Forms.DialogResult.Yes);
                    }

                    if (isConfirm3)
                    {
                        e2.OverrideResult((int)System.Windows.Forms.DialogResult.OK);
                    }
#endif
                }
                catch { }
            }
        }
        public static void WarningDialogHide(object sender, DialogBoxShowingEventArgs e)
        {
            if (e != null)
            {
                bool isConfirm = e.DialogId.Contains("Dialog_Revit_DocWarnDialog");
#if !R2025
                try
                {
                    if (isConfirm)
                    {
                        e.OverrideResult((int)System.Windows.Forms.DialogResult.OK);
                    }
                }
                catch { }
#endif
            }
        }

        public static DirectShape CreateDirectShape(List<GeometryObject> geometryObjects, BuiltInCategory builtInCategory=BuiltInCategory.OST_GenericModel)
        {
            var directShape = DirectShape.CreateElement(RevitAPI.Document, new ElementId(builtInCategory));
            directShape.SetShape(geometryObjects);

            return directShape;
        }
    }
}