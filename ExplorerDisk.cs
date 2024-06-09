using System.IO;
using System.Windows.Media.Imaging;

namespace file_explorer
{
    public class ExplorerDisk
    {
        public String name { get; private set; }
        public BitmapImage icon { get; private set; }
        public String path { get; private set; }

        public ExplorerDisk(DriveInfo info)
        {
            this.name = String.Format("({0}) {1}", info.Name, info.VolumeLabel);
            this.path = info.RootDirectory.FullName;

            switch (info.DriveType)
            {
                case DriveType.Fixed:
                    this.icon = DefaultIcons.FixedDriveLarge.ToBitmap().ToBitmapImage();
                    break;

                case DriveType.Network:
                    this.icon = DefaultIcons.NetworkDriveLarge.ToBitmap().ToBitmapImage();
                    break;

                default:
                    this.icon = new BitmapImage();
                    break;
            }
        }
    }
}
