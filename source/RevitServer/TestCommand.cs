using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BIMPlugins.ExtStorage.RevitServer.ViewModels;
using BIMPlugins.ExtStorage.RevitServer.Windows;

namespace BIMPlugins.ExtStorage.RevitServer
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class TestCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var viewModel = new MultiRSNViewModel();
            var window = new MultiRSNWindow(viewModel);
            viewModel.CloseRequest += (s, e) => window.Close();

            window.ShowDialog();

            return Result.Succeeded;
        }
    }
}