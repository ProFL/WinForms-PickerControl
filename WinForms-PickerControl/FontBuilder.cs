using System.Drawing;

namespace PickerControl
{
    public abstract partial class Picker<T>
    {
        public class FontBuilder
        {
            protected string Family;
            protected int _Size = 1;
            public int Size
            {
                get
                {
                    return _Size;
                }
            }
            protected FontStyle Style;

            public int GetSize()
            {
                return Size;
            }

            public FontBuilder(string family, FontStyle style)
            {
                Family = family;
                Style = style;
            }

            public Font Build(Picker<T> picker, int largestWordSize)
            {
                if (picker is HorizontalPicker<T>)
                {
                    _Size = picker.Width / (largestWordSize * picker.DisplayItemCount);
                    if (Size > picker.Height)
                    {
                        _Size = picker.Height;
                    }
                }
                ////TODO
                //else if (picker is VerticalPicker)
                //{
                //    //TODO
                //    Size = picker.Width;
                //}

                if (Size <= 0 || Size > System.Single.MaxValue)
                {
                    _Size = 1;
                }

                return new Font(Family, Size, Style, GraphicsUnit.Pixel);
            }
        }
    }
}
