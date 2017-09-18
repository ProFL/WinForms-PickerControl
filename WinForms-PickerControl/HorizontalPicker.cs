//MIT License

//Copyright (c) 2017 Pedro Linhares

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Diagnostics;

namespace PickerControl
{
    [Designer(typeof(Design.HorizontalPickerDesigner), typeof(IRootDesigner))]
    public class HorizontalPicker<T> : Picker<T>
    {
        public static event EventHandler AllItemCountChanged;
        protected static int _AllItemCount = 0;
        public static int AllItemCount
        {
            get
            {
                return _AllItemCount;
            }
            set
            {
                _AllItemCount = value;
                AllItemCountChanged?.Invoke(null, null);
            }
        }

        public static event EventHandler LargestWordLengthChanged;
        protected static int _LargestWordLength = 0;
        public static int LargestWordLength
        {
            get
            {
                return _LargestWordLength;
            }
            set
            {
                _LargestWordLength = value;
                LargestWordLengthChanged?.Invoke(null, null);
            }
        }

        public override event EventHandler ItemPicked;

        public Size ItemBoxSize
        {
            get
            {
                return new Size((int)(LargestWordLength * Font.Size), (int)Font.Size);
            }
        }

        [Flags]
        protected enum MouseHoverDirection
        {
            None = 0x00,
            Left = 1 << 1,
            Right = 1 << 2,
            Far = 1 << 3,
        }
        protected MouseHoverDirection _CurrentHoverDirection;
        protected MouseHoverDirection CurrentHoverDirection
        {
            get
            {
                return _CurrentHoverDirection;
            }
            set
            {
                _CurrentHoverDirection = value;
                if ((_CurrentHoverDirection & MouseHoverDirection.Far) != 0 && timerHover.Interval != 500)
                {
                    timerHover.Interval = 500;
                }
                else if ((_CurrentHoverDirection & MouseHoverDirection.Far) == 0 && timerHover.Interval != 1000)
                {
                    timerHover.Interval = 1000;
                }

                if (timerHover.Enabled == false)
                {
                    timerHover.Start();
                }
            }
        }
        protected Timer timerHover;

        protected void BaseInit()
        {
            Items = new PickerList();
            DisplayItemCount = 5;
            DisplayItemSpacing = 2;
            _SelectedIndex = 0;
            FontDetails = new FontBuilder(DefaultFont.FontFamily.ToString(), FontStyle.Regular);
            timerHover = new Timer();

            Items.ItemAdded += OnItemAdded;
            AllItemCountChanged += OnAllItemCountChanged;
            LargestWordLengthChanged += OnLargestWordLengthChanged;
            SelectedIndexChanged += OnSelectedIndexChanged;
            timerHover.Tick += OnTimerHoverTick;
            MouseHover += OnMouseHover;
            MouseClick += OnMouseClick;
            //MouseWheel += OnMouseWheel;
        }
        public HorizontalPicker() : base()
        {
            BaseInit();
        }
        public HorizontalPicker(int left, int top, int width, int height) : base("", left, top, width, height)
        {
            BaseInit();
        }

        protected bool CursorIsInRight()
        {
            return (Cursor.Position.X >= Width / 2 + ItemBoxSize.Width);
        }

        protected bool CursorIsInLeft()
        {
            return (Cursor.Position.X <= Width / 2 - ItemBoxSize.Width);
        }

        protected void OnItemAdded(object sender, EventArgs e)
        {
            if (Items.Count == 1)
            {
                SelectedIndex = 0;
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (FontDetails != null)
            {
                Font = FontDetails.Build(this, LargestWordLength);
            }
        }
        protected void OnAllItemCountChanged(object sender, EventArgs e)
        {
            foreach (var item in Items)
            {
                if (item.ToString().Length > LargestWordLength)
                {
                    LargestWordLength = item.ToString().Length;
                }
            }
        }
        protected void OnLargestWordLengthChanged(object sender, EventArgs e)
        {
            Font = FontDetails.Build(this, LargestWordLength);
            Refresh();
        }
        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndex < 0)
            {
                _SelectedIndex = Items.Count + _SelectedIndex;
            }
        }
        protected void OnTimerHoverTick(object sender, EventArgs e)
        {
            if (CurrentHoverDirection != MouseHoverDirection.None)
            {
                if ((CurrentHoverDirection & MouseHoverDirection.Right) != 0)
                {
                    if (CursorIsInRight())
                    {
                        SelectedIndex++;
                    }
                }
                else
                {
                    if (CursorIsInLeft())
                    {
                        SelectedIndex--;
                    }
                }
                Refresh();

                CurrentHoverDirection = MouseHoverDirection.None;
                timerHover.Stop();
            }
        }
        protected void OnMouseHover(object sender, EventArgs e)
        {
            var pos = Cursor.Position;
            if (CursorIsInRight())
            {
                if (timerHover.Enabled && ((CurrentHoverDirection & (MouseHoverDirection.Right | MouseHoverDirection.Far)) != 0))
                {
                    return;
                }
                else
                {
                    if (timerHover.Enabled && (CurrentHoverDirection & MouseHoverDirection.Right) == 0)
                    {
                        timerHover.Stop();
                    }

                    CurrentHoverDirection |= MouseHoverDirection.Right;
                    if (pos.X >= Width / 2 + 2 * ItemBoxSize.Width)
                    {
                        CurrentHoverDirection |= MouseHoverDirection.Far;
                    }

                    if (!timerHover.Enabled)
                    {
                        timerHover.Start();
                    }
                }
            }
            else if (CursorIsInLeft())
            {
                if (timerHover.Enabled && ((CurrentHoverDirection & (MouseHoverDirection.Left | MouseHoverDirection.Far)) != 0))
                {
                    return;
                }
                else
                {
                    if (timerHover.Enabled && (CurrentHoverDirection & MouseHoverDirection.Left) == 0)
                    {
                        timerHover.Stop();
                    }

                    CurrentHoverDirection |= MouseHoverDirection.Left;
                    if (pos.X <= Width / 2 + 2 * ItemBoxSize.Width)
                    {
                        CurrentHoverDirection |= MouseHoverDirection.Far;
                    }

                    if (!timerHover.Enabled)
                    {
                        timerHover.Start();
                    }
                }
            }
        }
        protected void OnMouseClick(object sender, EventArgs e)
        {
            var cursorPos = Cursor.Position;
            var centerBoxLeft = Width / 2 - ItemBoxSize.Width / 2;

            if (cursorPos.X >= centerBoxLeft && cursorPos.Y <= centerBoxLeft + ItemBoxSize.Width)
            {
                ItemPicked?.Invoke(this, null);
            }
            else if (cursorPos.X < centerBoxLeft)
            {
                SelectedIndex--;
            }
            else if (cursorPos.X > centerBoxLeft + ItemBoxSize.Width)
            {
                SelectedIndex++;
            }
        }

        [Conditional("DEBUG")]
        protected void PaintBorders(Color color, Rectangle rectangle, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(color), rectangle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            PaintBorders(Color.FromArgb(127, Color.Blue), new Rectangle(new Point(Location.X + 1, Location.Y + 1), new Size(Width - 2, Height - 2)), e);

            if (Items.Count >= 1)
            {
                StringFormat stringFormat = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.Character,
                };
                RectangleF drawingRectangle = new RectangleF()
                {
                    X = Location.X,
                    Y = Location.Y,
                    Width = LargestWordLength * Font.Size,
                    Height = Font.Height
                };

                float alphaVariationStep = ((100.0f - 50.0f) / (DisplayItemCount / 2)) / 100;
                byte lastTransparency = 0x7F;
                for (int i = 0; i < DisplayItemCount; i++)
                {
                    Color drawColor = Color.FromArgb(lastTransparency, Color.White);

                    if (i < DisplayItemCount / 2)
                    {
                        lastTransparency += (byte)(0xFF * alphaVariationStep);
                    }
                    else
                    {
                        lastTransparency -= (byte)(0xFF * alphaVariationStep);
                    }

                    int curIndex = 0;

                    if (i < DisplayItemCount / 2)
                    {
                        curIndex = SelectedIndex - (DisplayItemCount / 2) + i;
                        if (curIndex < 0)
                        {
                            curIndex = Items.Count + curIndex;
                        }
                    }
                    else if (i == DisplayItemCount / 2)
                    {
                        curIndex = SelectedIndex;
                    }
                    else
                    {
                        curIndex = SelectedIndex + (i / 2);
                    }

                    RectangleF rectangleToDraw = drawingRectangle;
                    rectangleToDraw.X += (i * drawingRectangle.Width);

                    PaintBorders(Color.FromArgb(127, Color.Red), Rectangle.Round(rectangleToDraw), e);

                    e.Graphics.DrawString(Items[curIndex].ToString(), Font, new SolidBrush(drawColor), rectangleToDraw, stringFormat);
                }
            }
        }
    }
}
