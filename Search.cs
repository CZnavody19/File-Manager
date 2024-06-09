using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace file_explorer
{
    internal class Search
    {
        ListView listView;
        ObservableCollection<ExplorerItem> items;
        public void init(ListView _listView, ObservableCollection<ExplorerItem> _items)
        {
            this.items = _items;
            this.listView = _listView;
        }

        public void search(String text)
        {
            if (text.StartsWith("<re>"))
            {
                Regex re = new Regex(text.Substring(4));
                BackgroundTask.createTask(String.Format("Searching regex", items.Count()), () =>
                {
                    IEnumerable<ExplorerItem> searched = this.items.Where(f => re.IsMatch(f.name));
                    this.listView.Dispatcher.Invoke(() => this.listView.ItemsSource = searched);
                });
            }
            else
            {
                this.listView.ItemsSource = this.items.Where(f => f.name.ToLower().Contains(text.ToLower()));
            }
        }

        public void cancel()
        {
            this.listView.ItemsSource = items;
        }
    }
}
