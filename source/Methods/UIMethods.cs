using Autodesk.Revit.UI;
using Autodesk.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BIMPlugins.ExtStorage.Methods
{
    public static class UIMethods
    {
        public static string GetImagePath(string dllName, string imageName)
        {
#if R2019
            return $@"/{dllName}_2019;component/Resources/{imageName}";
#elif R2020
            return $@"/{dllName}_2020;component/Resources/{imageName}";
#elif R2021
            return $@"/{dllName}_2021;component/Resources/{imageName}";
#elif R2022
            return $@"/{dllName}_2022;component/Resources/{imageName}";
#elif R2023
            return $@"/{dllName}_2023;component/Resources/{imageName}";
#elif R2024
            return $@"/{dllName}_2024;component/Resources/{imageName}";
#endif
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

        public static Autodesk.Windows.RibbonButton CreateAWButton(ICommand command, string name, string text, string imagePath, string toolTip)
        {
            Autodesk.Windows.RibbonButton button = new Autodesk.Windows.RibbonButton
            {
                Id = "SE_Plugins_" + name,
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
                Id = "SE Plugins_Modify",
                Name = "SE Plugins",
                Title = "SE Plugins"
            };
            panel.Source = source;

            return panel;
        }
    }
}