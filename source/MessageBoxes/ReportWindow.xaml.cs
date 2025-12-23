using System.Collections;
using System.Windows;

namespace BIMPlugins.ExtStorage.MessageBoxes
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ReportWindow(IEnumerable collection)
        {
            InitializeComponent();
            Box.ItemsSource = collection;
        }
    }
}
