using System.Windows;
using System.Windows.Controls;

namespace file_explorer
{
    public class BackgroundTask
    {
        static TextBlock statusTextBox;
        static ProgressBar statusProgressBar;
        static Thread backgroundThread = new Thread(bgThread);
        static Queue<BackgroundTask> queue = new Queue<BackgroundTask>();
        static bool shouldBeAlive = true;
        static bool isInitialized = false;
        public static void init(TextBlock _statusTextBox, ProgressBar _statusProgressBar)
        {
            statusTextBox = _statusTextBox;
            statusProgressBar = _statusProgressBar;

            backgroundThread.Start();

            isInitialized = true;
        }
        public static void createTask(String description, Delegate task)
        {
            queue.Enqueue(new BackgroundTask(description, task));
        }
        public static void close()
        {
            queue.Clear();
            shouldBeAlive = false;
        }
        static void bgThread()
        {
            while (shouldBeAlive)
            {
                if (queue.Count > 0 && isInitialized)
                {
                    BackgroundTask task = queue.Dequeue();

                    statusTextBox.Dispatcher.Invoke(() => statusTextBox.Text = task.description);
                    statusProgressBar.Dispatcher.Invoke(() => statusProgressBar.IsIndeterminate = true);

                    try
                    {
                        task.task.DynamicInvoke();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, String.Format("Task {} failed", task.description), MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    statusTextBox.Dispatcher.Invoke(() => statusTextBox.Text = "No tasks");
                    statusProgressBar.Dispatcher.Invoke(() => statusProgressBar.IsIndeterminate = false);
                }
            }
        }

        String description;
        Delegate task;

        BackgroundTask(String _description, Delegate _task)
        {
            this.description = _description;
            this.task = _task;
        }
    }
}
