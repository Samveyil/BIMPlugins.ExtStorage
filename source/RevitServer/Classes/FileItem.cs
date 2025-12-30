using CommunityToolkit.Mvvm.ComponentModel;

namespace BIMPlugins.ExtStorage.RevitServer.Classes
{
    public partial class FileItem(string name) : ObservableObject
    {
        [ObservableProperty] private bool _isSelected = false;
        [ObservableProperty] private bool _isExpanded = false;

        public string Name { get; } = name;
        public FileItem Parent { get; set; }
    }
}