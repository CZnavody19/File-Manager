using System.IO;
using System.Windows;

namespace file_explorer
{
    public partial class Rename : Window
    {
        ExplorerItem item;
        public Rename(ExplorerItem _item)
        {
            InitializeComponent();
            UI_textbox_name.Text = _item.name;
            this.item = _item;
        }

        private void UI_button_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UI_button_submit_Click(object sender, RoutedEventArgs e)
        {
            string newPath = Path.Combine(Path.GetDirectoryName(this.item.path), UI_textbox_name.Text);
            if (this.item.type == explorerItemType.File)
            {
                File.Move(this.item.path, newPath);
            }
            else
            {
                Directory.Move(this.item.path, newPath);
            }
            this.Close();
        }
    }
}
