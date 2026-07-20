using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace BIMPlugins.ExtStorage
{
    /// <summary>
    /// Provides static access to the current Revit application, document, and UI components.
    /// Also contains a generic <see cref="MyEventHandler{T}"/> for executing custom actions in the Revit API context.
    /// </summary>
    public static class RevitAPI
    {
        /// <summary>The current Revit UIApplication/>.</summary>
        public static UIApplication UIApplication { get; } = new Autodesk.Revit.UI.Events.RibbonItemEventArgs().Application;

        /// <summary>Returns the database level Application represented by this UI level Application.</summary>
        public static Autodesk.Revit.ApplicationServices.Application Application { get => UIApplication.Application; }

        /// <summary>Provides access to an object that represents the currently active project.</summary>
        public static UIDocument UIDocument { get => UIApplication.ActiveUIDocument; }

        /// <summary>The active document.</summary>
        public static Document Document { get => UIDocument.Document; }

        /// <summary>The current document's active view.</summary>
        /// <value>The active view is the view that last had focus in the UI. <see langword="null"/> if no view is considered active.</value>
        public static View ActiveView { get => Document.ActiveView; }


        /// <summary>
        /// A generic <see cref="Autodesk.Revit.UI.IExternalEventHandler"/> implementation that executes a custom action 
        /// on a target object using <see cref="Autodesk.Revit.UI.ExternalEvent"/>.
        /// </summary>
        /// <typeparam name="T">The type of the target object (typically a ViewModel).</typeparam>
        /// <remarks>
        /// This handler is designed to be used with <see cref="Autodesk.Revit.UI.ExternalEvent"/> to execute code 
        /// safely within the Revit API context without violating the API context boundaries.
        /// <br/>
        /// Any exceptions thrown during execution are caught and displayed in a <see cref="Autodesk.Revit.UI.TaskDialog"/>.
        /// </remarks>
        public class MyEventHandler<T>(T target, Action<T> action) : IExternalEventHandler
        {
            private readonly T _target = target;
            private readonly Action<T> _action = action;

            public void Execute(UIApplication app)
            {
                try
                {
                    _action(_target);
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", ex.Message + ex.StackTrace);
                }
            }

            public string GetName() => "GenericEventHandler";
        }
    }
}