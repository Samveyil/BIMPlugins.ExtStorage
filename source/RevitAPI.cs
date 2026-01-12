using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace BIMPlugins.ExtStorage
{
    public static class RevitAPI
    {
        public static UIApplication UIApplication { get; set; } = new Autodesk.Revit.UI.Events.RibbonItemEventArgs().Application;
        public static Autodesk.Revit.ApplicationServices.Application Application { get => UIApplication.Application; }
        public static UIDocument UIDocument { get => UIApplication.ActiveUIDocument; }
        public static Document Document { get => UIDocument.Document; }
        public static View ActiveView { get => Document.ActiveView; }

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