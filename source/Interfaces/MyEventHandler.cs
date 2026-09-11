using Autodesk.Revit.UI;
using System;

namespace BIMPlugins.ExtStorage.Interfaces
{
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
    internal class MyEventHandler<T>(T target, Action<T> action) : IExternalEventHandler
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