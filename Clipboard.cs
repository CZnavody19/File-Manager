using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace file_explorer
{
    internal static class Clipboard
    {
        static List<(ExplorerItem, bool)> items = new List<(ExplorerItem, bool)>();
        static TextBlock statusBarText;
        public static void init(TextBlock _statusText)
        {
            statusBarText = _statusText;
        }
        static void updateStatus()
        {
            statusBarText.Text = String.Format("{0} items in clipboard", items.Count());
        }
        
        static void copyOrMove(string _from, string _to, bool _file, bool _move)
        {
            try
            {   // least scuffed piece of code
                switch ((Convert.ToUInt32(_file) << 1) | Convert.ToUInt32(_move)) {
                    case 0b00:
                        Utils.CopyDirectory(_from, _to);
                        break;
                    case 0b01:
                        Directory.Move(_from, _to);
                        break;
                    case 0b10:
                        File.Copy(_from, _to);
                        break;
                    case 0b11:
                        File.Move(_from, _to);
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void copy(List<ExplorerItem> _items, bool _owerwrite, bool _cut)
        {
            if (_owerwrite)
            {
                items.Clear();
            }
            items.AddRange(_items.Select(item => (item, _cut)));
            updateStatus();
        }
        public static void paste(String path)
        {
            BackgroundTask.createTask(String.Format("Copying {0} items", items.Count()), () =>
            {
                foreach ((ExplorerItem item, bool cut) in items)
                {
                    copyOrMove(item.path, Path.Combine(path, item.name), item.type == explorerItemType.File, cut); 
                }
            });
        }
    }
}
