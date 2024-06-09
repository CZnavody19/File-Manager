using System.Windows;

namespace file_explorer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                BackgroundTask.init(UI_status_bar_text, UI_status_bar_progress);
                Clipboard.init(UI_status_bar_text_clipboard);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "App crashed, enjoy", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BackgroundTask.close();
        }
    }
}