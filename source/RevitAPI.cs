using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BIMPlugins.ExtStorage.Interfaces;
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


        /// <summary>Creates an instance of external event for executing the specified action.</summary>
        /// <typeparam name="T">The type of the target object.</typeparam>
        /// <param name="target">The object (typically a ViewModel) passed to <paramref name="action"/> when the event is executed.</param>
        /// <param name="action">The action executed by the external event handler.</param>
        /// <returns>An instance of ExternalEvent class, which will be used to invoke the event</returns>
        public static ExternalEvent CreateExtEvent<T>(T target, Action<T> action) => ExternalEvent.Create(new MyEventHandler<T>(target, action));
    }
}