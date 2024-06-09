using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace file_explorer
{
    public static class Utils
    {
        //promptly stolen from http://www.java2s.com/example/csharp/system.windows.media.imaging/bitmap-to-bitmapimage.html and modified, thanks!
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }
        public static void CopyDirectory(string sourceDirName, string destDirName)
        {
            Directory.CreateDirectory(destDirName);

            foreach (string dir in Directory.GetDirectories(sourceDirName, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(Path.Combine(destDirName, dir.Substring(sourceDirName.Length + 1)));
            }
            foreach (string file_name in Directory.GetFiles(sourceDirName, "*", SearchOption.AllDirectories))
            {
                File.Copy(file_name, Path.Combine(destDirName, file_name.Substring(sourceDirName.Length + 1)));
            }
        }
        public static String readableSize(long size)
        {
            if (size < 1024)
            {
                return String.Format("{0:0.##} B", size);
            }
            else if (size <  (1024 * 1024))
            {
                return String.Format("{0:0.##} kB", size / 1024f);
            }
            else if (size < (1024 * 1024 * 1024))
            {
                return String.Format("{0:0.##} MB", size / (1024f * 1024f));
            }
            else if (size < (1024l * 1024l * 1024l * 1024l))
            {
                return String.Format("{0:0.##} GB", size / (1024f * 1024f * 1024f));
            }
            else
            {
                return "kys";
            }
        }
    }
}
