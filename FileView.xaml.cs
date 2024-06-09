using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace file_explorer
{
    public partial class FileView : UserControl
    {
        ObservableCollection<ExplorerItem> files = new ObservableCollection<ExplorerItem>();
        ObservableCollection<ExplorerDisk> disks = new ObservableCollection<ExplorerDisk>();
        FileSystemWatcher fsWatcher = new FileSystemWatcher();
        History<String> history = new History<String>();
        Search search = new Search();
        bool progChangingPath = false;
        public FileView()
        {
            InitializeComponent();

            setUIPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            UI_listview_files.ItemsSource = files;
            UI_listview_disks.ItemsSource = disks;

            history.init(UI_button_back, UI_button_forward);
            search.init(UI_listview_files, files);

            fsWatcher.NotifyFilter = NotifyFilters.Attributes
                                   | NotifyFilters.DirectoryName
                                   | NotifyFilters.FileName
                                   | NotifyFilters.Size;
            fsWatcher.Changed += directoryChanged;
            fsWatcher.Created += directoryChanged;
            fsWatcher.Deleted += directoryChanged;
            fsWatcher.Renamed += directoryChanged;
            fsWatcher.IncludeSubdirectories = false;
            fsWatcher.EnableRaisingEvents = false;

            loadDisks();
            loadDirectory(UI_textbox_path.Text, true);
        }
        void directoryChanged(object sender, FileSystemEventArgs e)
        {
            this.Dispatcher.Invoke(() => loadDirectory(UI_textbox_path.Text, false));
        }
        public void loadDirectory(String _path, bool _addhistory)
        {
            fsWatcher.EnableRaisingEvents = false;
            search.cancel();
            BackgroundTask.createTask(String.Format("Loading directory {0}", _path), () =>
            {
                try
                {
                    int count_files = 0;
                    int count_dirs = 0;
                    long size_files = 0;

                    if (_addhistory)
                    {
                        this.Dispatcher.Invoke(() => history.add(_path));
                    }
                    this.Dispatcher.Invoke(() => files.Clear());
                    foreach (String directory in Directory.GetDirectories(_path))
                    {
                        this.Dispatcher.Invoke(() => files.Add(new ExplorerItem(directory, explorerItemType.Direcory)));
                        count_dirs += 1;
                    }

                    foreach (String file in Directory.GetFiles(_path))
                    {
                        this.Dispatcher.Invoke(() => files.Add(new ExplorerItem(file, explorerItemType.File)));
                        count_files += 1;
                        size_files += new FileInfo(file).Length;
                    }

                    foreach (ExplorerDisk disk in disks)
                    {
                        if (disk.path == new DirectoryInfo(_path).Root.FullName)
                        {
                            this.Dispatcher.Invoke(() => UI_listview_disks.SelectedItem = disk);
                        }
                    }
                    this.Dispatcher.Invoke(() => setUIPath(_path));
                    this.Dispatcher.Invoke(() => UI_status_bar_info.Text = String.Format("{0} items ({1} files, {2} directories); {3}", files.Count, count_files, count_dirs, Utils.readableSize(size_files)));
                    this.Dispatcher.Invoke(() => fsWatcher.Path = _path);
                    this.Dispatcher.Invoke(() => fsWatcher.EnableRaisingEvents = true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Dispatcher.Invoke(() => loadDirectory(history.previous(), false));
                }
            });
        }
        void loadDisks()
        {
            foreach (DriveInfo di in DriveInfo.GetDrives())
            {
                BackgroundTask.createTask(String.Format("Loading disk {0}", di.Name), () =>
                {
                    if (di.IsReady)
                    {
                        this.Dispatcher.Invoke(() => disks.Add(new ExplorerDisk(di)));
                    }
                });
            }
        }
        void setUIPath(String _path)
        {
            progChangingPath = true;
            UI_textbox_path.Text = _path;
            progChangingPath = false;
        }

        private void UI_textbox_path_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!progChangingPath && Directory.Exists(UI_textbox_path.Text))
            {
                loadDirectory(UI_textbox_path.Text, true);
            }
        }

        private void UI_button_up_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo? parent = Directory.GetParent(UI_textbox_path.Text);
            if (parent != null)
            {
                loadDirectory(parent.FullName, true);
            }
        }

        private void UI_listview_files_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (UI_listview_files.SelectedItem != null)
            {
                ExplorerItem item = UI_listview_files.SelectedItem as ExplorerItem;
                switch (item.type)
                {
                    case explorerItemType.File:
                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = item.path;
                        psi.UseShellExecute = true;
                        Process.Start(psi);
                        break;
                    case explorerItemType.Direcory:
                        loadDirectory(item.path, true);
                        break;
                }
            }
        }

        private void UI_listview_disks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            loadDirectory((UI_listview_disks.SelectedItem as ExplorerDisk).path, true);
        }

        private void UI_button_back_Click(object sender, RoutedEventArgs e)
        {
            loadDirectory(history.previous(), false);
        }

        private void UI_button_forward_Click(object sender, RoutedEventArgs e)
        {
            loadDirectory(history.next(), false);
        }

        private void UI_button_search_Click(object sender, RoutedEventArgs e)
        {
            if (UI_textbox_search.Text == "")
            {
                search.cancel();
            }
            else
            {
                search.search(UI_textbox_search.Text);
            }
        }

        private void UI_CM_Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.copy(UI_listview_files.SelectedItems.OfType<ExplorerItem>().ToList(), true, false);
        }

        private void UI_CM_CopyDO_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.copy(UI_listview_files.SelectedItems.OfType<ExplorerItem>().ToList(), false, false);
        }
        private void UI_CM_Cut_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.copy(UI_listview_files.SelectedItems.OfType<ExplorerItem>().ToList(), true, true);
        }
        private void UI_CM_CutDO_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.copy(UI_listview_files.SelectedItems.OfType<ExplorerItem>().ToList(), false, true);
        }
        private void UI_CM_Paste_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.paste(UI_textbox_path.Text);
        }
        private void UI_CM_Delete_Click(object sender, RoutedEventArgs e)
        {
            foreach (ExplorerItem item in UI_listview_files.SelectedItems)
            {
                if (item.type == explorerItemType.File)
                {
                    File.Delete(item.path);
                }
                else
                {
                    Directory.Delete(item.path, true);
                }
            }
        }
        private void UI_CM_CopyPath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(((ExplorerItem)UI_listview_files.SelectedItem).path);
        }

        private void UI_CM_Rename_Click(object sender, RoutedEventArgs e)
        {
            Rename renameDialog = new Rename((ExplorerItem)UI_listview_files.SelectedItem);
            renameDialog.ShowDialog();
        }
    }
}
