using System.Windows.Controls;

namespace file_explorer
{
    public class History<T>
    {
        Button back, forward;
        List<T> history = new List<T>();
        int current = 0;

        public void init(Button _back, Button _forward)
        {
            this.back = _back;
            this.forward = _forward;
            updateButtons();
        }
        public void add(T item)
        {
            if (this.current < this.history.Count - 1)
            {
                this.history.RemoveRange(this.current + 1, this.history.Count - this.current - 1);
            }
            this.history.Add(item);
            this.current = this.history.Count - 1;
            updateButtons();
        }
        public T previous()
        {
            if (this.current > 0)
            {
                this.current -= 1;
                updateButtons();
                return this.history[this.current];
            }
            return this.history.First();
        }
        public T next()
        {
            if (this.current < this.history.Count - 1)
            {
                this.current += 1;
                updateButtons();
                return this.history[this.current];
            }
            return this.history.Last();
        }

        void updateButtons()
        {
            this.back.IsEnabled = this.current > 0;
            this.forward.IsEnabled = this.current < this.history.Count - 1;
        }
    }
}
