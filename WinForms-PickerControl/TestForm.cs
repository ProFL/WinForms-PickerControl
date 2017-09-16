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
using System.Windows.Forms;
using System.Drawing;

namespace PickerControl
{
    class TestForm : Form
    {
        private HorizontalPicker<string> p;

        public void InitializeComponents()
        {
            p = new HorizontalPicker<string>(0, 0, 500, 500);

            p.Items.ItemAdded += (sender, e) =>
            {
                HorizontalPicker<string>.AllItemCount++;
            };

            p.Items.Add("TAB");
            p.Items.Add("Q");
            p.Items.Add("W");
            p.Items.Add("E");
            p.Items.Add("R");
            p.Items.Add("T");
            p.Items.Add("Y");

            Controls.Add(p);
        }

        public TestForm() : base()
        {
            Size = new Size(1000, 1000);

            BackColor = Color.Black;
            InitializeComponents();
        }

        [STAThread]
        public static void Main()
        {
            TestForm t = new TestForm();

            Application.Run(t);
        }
    }
}
