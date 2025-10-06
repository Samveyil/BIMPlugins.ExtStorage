using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Media;

namespace BIMPlugins.ExtStorage
{
    public static class RevitAPI
    {
        public static UIApplication UIApplication { get; set; } = new Autodesk.Revit.UI.Events.RibbonItemEventArgs().Application;
        public static Autodesk.Revit.ApplicationServices.Application Application { get => UIApplication.Application; }
        public static UIDocument UIDocument { get => UIApplication.ActiveUIDocument; }
        public static Document Document { get => UIDocument.Document; }
        public static Autodesk.Revit.DB.View ActiveView { get => Document.ActiveView; }

        public static void InitializeMaterialDesign()
        {
            var card = new Card();
            var hue = new Hue("Dammy", Colors.Black, Colors.White);
        }

        public class MyEventHandler<T> : IExternalEventHandler where T : ObservableObject
        {
            private readonly T _target;
            private readonly Action<T> _action;

            public MyEventHandler(T target, Action<T> action)
            {
                _target = target;
                _action = action;
            }

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

            public string GetName()
            {
                return "GenericEventHandler";
            }
        }
    }
}