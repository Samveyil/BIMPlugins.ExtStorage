using System.ComponentModel;

namespace BIMPlugins.ExtStorage.RevitServer.Classes
{
    public partial class FolderItem(string name) : FileItem(name)
    {
        public ICollectionView Items { get; set; }
        public bool IsServer { get; set; } = false;
    }
}