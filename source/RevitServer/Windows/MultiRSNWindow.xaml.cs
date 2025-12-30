using BIMPlugins.ExtStorage.RevitServer.ViewModels;
using System.Windows;

namespace BIMPlugins.ExtStorage.RevitServer.Windows
{
    /// <summary>
    /// Логика взаимодействия для MultiRSNWindow.xaml
    /// </summary>
    public partial class MultiRSNWindow : Window
    {
        public MultiRSNWindow(MultiRSNViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
