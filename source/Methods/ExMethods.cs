using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.UI;
using System.IO;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.Methods
{
    /// <summary>Miscellaneous helpers: reading files, suppressing Revit dialogs and creating DirectShapes.</summary>
    public static class ExMethods
    {
        /// <summary>Opens a text file, reads all the text in the file, and then closes the file.</summary>
        /// <param name="filePath">The path of the file to read.</param>
        /// <remarks>Note: if the specified file does not exist, the method will return <see langword="null"/>.</remarks>
        /// <returns>A string containing all the text in the file.</returns>
        public static string ReadTextFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return null;
        }

        /// <summary>
        /// A <see cref="Autodesk.Revit.UI.UIApplication.DialogBoxShowing"/> handler that automatically answers dialogs shown while opening or printing documents.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// Handles the missing third-party updater dialog (continues working with the file), the unresolved references dialog
        /// (ignores the references and continues) and the print settings warning (OK). The print settings warning is recognized
        /// by its Russian text only. In the Revit 2025 build only the missing third-party updater dialog is handled.
        /// <code>
        /// RevitAPI.UIApplication.DialogBoxShowing += ExMethods.DialogBoxHide;
        /// // open or print documents
        /// RevitAPI.UIApplication.DialogBoxShowing -= ExMethods.DialogBoxHide;
        /// </code>
        /// </remarks>
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

        /// <summary>
        /// A <see cref="Autodesk.Revit.UI.UIApplication.DialogBoxShowing"/> handler that closes document warning dialogs with OK.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// Closes dialogs with the id <c>Dialog_Revit_DocWarnDialog</c>. Used by <see cref="Extensions.DocumentExtensions.PurgeUnused"/>.
        /// In the Revit 2025 build the handler does nothing.
        /// </remarks>
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


        /// <summary>Creates a DirectShape object and adds it to document.</summary>
        /// <param name="geometryObjects">Shape of this object expressed as a collection of GeometryObjects. The supported types of GeometryObjects are: Solid, Mesh, GeometryInstance, Point and Curve.</param>
        /// <param name="builtInCategory">Id of the category assigned to this DirectShape. Must be a valid category id.</param>
        /// <returns>The created DirectShape object.</returns>
        public static DirectShape CreateDirectShape(List<GeometryObject> geometryObjects, BuiltInCategory builtInCategory=BuiltInCategory.OST_GenericModel)
        {
            var directShape = DirectShape.CreateElement(RevitAPI.Document, new ElementId(builtInCategory));
            directShape.SetShape(geometryObjects);

            return directShape;
        }
    }
}