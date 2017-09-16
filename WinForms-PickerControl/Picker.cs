using System;
using System.Windows.Forms;

namespace PickerControl
{
    public abstract partial class Picker<T> : Control
    {
        public event EventHandler SelectedIndexChanged;

        public PickerList Items { get; set; }

        protected int _SelectedIndex = new int();
        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                _SelectedIndex = value;
                SelectedIndexChanged?.Invoke(this, null);
            }
        }

        public int DisplayItemCount { get; set; }
        public int DisplayItemSpacing { get; set; }
        public FontBuilder FontDetails { get; set; }

        public Picker() : base() {}
        public Picker(string text) : base(text) {}
        public Picker(Control parent, string text) : base(parent, text) {}
        public Picker(string text, int left, int top, int width, int height) : base(text, left, top, width, height) {}
        public Picker(Control parent, string text, int left, int top, int width, int height) : base(parent, text, left, top, width, height) { }
    }
}
