using BIMPlugins.ExtStorage.RevitServer.Classes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BIMPlugins.ExtStorage.RevitServer.ViewModels
{
    public partial class MultiRSNViewModel : RSNViewModel
    {
        [ObservableProperty] private ObservableCollection<string> _projects = [];

        [RelayCommand]
        private void ProjectChecked(FileItem fileItem)
        {
            if (Projects.Contains(fileItem.Name))
                Projects.Remove(fileItem.Name);
            else
                Projects.Add(fileItem.Name);
        }
    }
}