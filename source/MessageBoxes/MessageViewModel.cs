using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;

namespace BIMPlugins.ExtStorage.MessageBoxes
{
    public partial class MessageViewModel(string message, MessageBoxImage messageIcon, bool useOKButton) : ObservableObject
    {
        [ObservableProperty] private string _messageText = message;
        [ObservableProperty] private string _image = messageIcon switch
        {
            MessageBoxImage.Information => "InformationOutline",
            MessageBoxImage.Question => "Help",
            MessageBoxImage.Warning => "Alert",
            MessageBoxImage.Error => "AlertCircleOutline",
            _ => null
        };
        [ObservableProperty] private bool _useOKButton = useOKButton;

        public MessageBoxResult Result { get; private set; } = MessageBoxResult.Cancel;


        [RelayCommand]
        private void Close(string result)
        {
            Result = result switch
            {
                "Да" => MessageBoxResult.Yes,
                "Нет" => MessageBoxResult.No,
                "ОК" => MessageBoxResult.OK,
                _ => MessageBoxResult.Cancel
            };

            RaiseCloseRequest();
        }


        public event EventHandler CloseRequest;
        public void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}