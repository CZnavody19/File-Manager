using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace file_explorer
{
    public class ExplorerItem
    {
        public String name { get; private set; }
        public explorerItemType type { get; private set; }
        public String path { get; private set; }
        public BitmapImage icon { get; private set; }
        public String size { get; private set; }

        public ExplorerItem(String _path, explorerItemType _type)
        {
            this.path = _path;
            this.type = _type;

            if (_type == explorerItemType.File)
            {
                this.icon = Icon.ExtractAssociatedIcon(_path).ToBitmap().ToBitmapImage();
                this.name = Path.GetFileName(_path);
                this.size = Utils.readableSize(new FileInfo(_path).Length);
            }

            if (_type == explorerItemType.Direcory)
            {
                this.icon = DefaultIcons.FolderLarge.ToBitmap().ToBitmapImage();
                this.name = new DirectoryInfo(_path).Name;
            }
        }
    }
    public enum explorerItemType
    {
        File,
        Direcory
    }
}
