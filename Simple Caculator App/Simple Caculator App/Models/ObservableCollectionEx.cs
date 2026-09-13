using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Simple_Caculator_App.Models
{
    public class ObservableCollectionEx : ObservableCollection<StringToken>
    {
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            Unsubscribe(e.OldItems);
            Subscribe(e.NewItems);
            base.OnCollectionChanged(e);
        }

        protected override void ClearItems()
        {
            foreach (StringToken element in this)
                element.PropertyChanged -= ContainedElementChanged;

            base.ClearItems();
        }

        private void Subscribe(IList iList)
        {
            if (iList != null)
            {
                foreach (StringToken element in iList)
                    element.PropertyChanged += ContainedElementChanged;
            }
        }

        private void Unsubscribe(IList iList)
        {
            if (iList != null)
            {
                foreach (StringToken element in iList)
                    element.PropertyChanged -= ContainedElementChanged;
            }
        }

        private void ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }

        public void Add(string a)
        {
            Add(new StringToken { Text = a });
        }

        public override string ToString()
        {
            return string.Join(" ", this);
        }
    }

    public class StringToken : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string text;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
            }
        }

        public int Length => Text.Length;

        public StringToken Substring(int startIndex, int length)
        {
            Text = Text.Substring(startIndex, length);
            return this;
        }

        public static StringToken operator +(StringToken b, string a)
        {
            b.Text = b.Text + a;
            return b;
        }

        public override string ToString() => Text;
    }
}
