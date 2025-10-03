using System.Windows;

namespace BIMPlugins.ExtStorage.MessageBoxes
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow()
        {
            InitializeComponent();
        }

        public static MessageBoxResult ShowMessage(string message, MessageBoxImage messageIcon = MessageBoxImage.None, bool useOKButton = true)
        {
            var messageWindow = new MessageWindow();

            return messageWindow.Show(message, messageIcon, useOKButton);
        }

        private MessageBoxResult Show(string message, MessageBoxImage messageIcon, bool useOKButton)
        {
            var viewModel = new MessageViewModel(message, messageIcon, useOKButton);
            DataContext = viewModel;
            
            viewModel.CloseRequest += (s, e) => this.Close();

            this.ShowDialog();

            return viewModel.Result;
        }
    }
}
