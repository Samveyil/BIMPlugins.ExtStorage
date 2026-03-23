using Autodesk.Revit.UI;
using Autodesk.Windows;
using System.Linq;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class UIControlledAppExtensions
    {
        public static void FindTab(this UIControlledApplication application, string tabName)
        {
            RibbonControl ribon = ComponentManager.Ribbon;
            bool checkExsistTab = false;
            foreach (RibbonTab tab in ribon.Tabs)
            {
                if (tab.AutomationName == tabName)
                {
                    checkExsistTab = true;
                    break;
                }
            }
            if (!checkExsistTab)
            {
                application.CreateRibbonTab(tabName);
            }
        }
        public static Autodesk.Revit.UI.RibbonPanel GetRibbonPanel(this UIControlledApplication application, string tabName, string panelName)
        {
            return application.GetRibbonPanels(tabName).FirstOrDefault(p => p.Name == panelName)
                ?? application.CreateRibbonPanel(tabName, panelName);
        }
    }
}