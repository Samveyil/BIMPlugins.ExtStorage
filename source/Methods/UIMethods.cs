using Autodesk.Revit.UI;
using Autodesk.Windows;
using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BIMPlugins.ExtStorage.Methods
{
    public static class UIMethods
    {
        public static string GetImagePath(string dllName, string imageName)
        {
            return $@"/{dllName};component/Resources/{imageName}";
        }

        public static void FindTab(UIControlledApplication application, string tabName)
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
        public static Autodesk.Revit.UI.RibbonPanel GetRibbonPanel(UIControlledApplication application, string tabName, string panelName)
        {
            return application.GetRibbonPanels(tabName).FirstOrDefault(p => p.Name == panelName)
                ?? application.CreateRibbonPanel(tabName, panelName);
        }

        public static Autodesk.Windows.RibbonButton CreateAWButton(ICommand command, string name, string text, string imagePath, string toolTip)
        {
            Autodesk.Windows.RibbonButton button = new Autodesk.Windows.RibbonButton
            {
                Id = "BIMPlugins_" + name,
                AllowInStatusBar = true,
                AllowInToolBar = true,
                GroupLocation = Autodesk.Private.Windows.RibbonItemGroupLocation.Single,
                IsEnabled = true,
                IsToolTipEnabled = true,
                IsVisible = false,
                LargeImage = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute)),
                ShowImage = true,
                ShowText = true,
                ShowToolTipOnDisabled = true,
                Text = text,
                ToolTip = toolTip,
                MinHeight = 0,
                MinWidth = 0,
                Size = RibbonItemSize.Large,
                ResizeStyle = RibbonItemResizeStyles.HideText,
                IsCheckable = false,
                Orientation = System.Windows.Controls.Orientation.Vertical,
                CommandHandler = command
            };

            return button;
        }
        public static Autodesk.Windows.RibbonPanel CreateAWPanel()
        {
            var panel = new Autodesk.Windows.RibbonPanel();
            panel.IsVisible = false;
            panel.FloatingOrientation = System.Windows.Controls.Orientation.Vertical;

            RibbonPanelSource source = new RibbonPanelSource()
            {
                Id = "BIMPlugins_Modify",
                Name = "BIMPlugins",
                Title = "BIMPlugins"
            };
            panel.Source = source;

            return panel;
        }
    }
}