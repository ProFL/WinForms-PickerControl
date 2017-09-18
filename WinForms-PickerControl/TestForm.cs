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

namespace PickerControl
{
    class TestForm : Form
    {
        private HorizontalPicker<string> horizontalPicker;
        private TextBox textBox;

        public TestForm() : base()
        {
            InitializeComponent();
        }

        [STAThread]
        public static void Main()
        {
            TestForm t = new TestForm();

            Application.Run(t);
        }

        private void InitializeComponent()
        {
            this.textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Margin = new System.Windows.Forms.Padding(0);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(500, 35);
            this.textBox.TabIndex = 0;
            this.textBox.Text = ": ";
            // 
            // TestForm
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(500, 161);
            this.Controls.Add(this.textBox);
            this.Name = "TestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();
            //
            // horizontalPicker
            //
            horizontalPicker = new HorizontalPicker<string>(0, 25, 500, 500);
            horizontalPicker.Items.ItemAdded += (sender, e) =>
            {
                HorizontalPicker<string>.AllItemCount++;
            };
            horizontalPicker.Items.Add("TAB");
            horizontalPicker.Items.Add("Q");
            horizontalPicker.Items.Add("W");
            horizontalPicker.Items.Add("E");
            horizontalPicker.Items.Add("R");
            horizontalPicker.Items.Add("T");
            horizontalPicker.Items.Add("Y");
            horizontalPicker.ItemPicked += (sender, e) =>
            {
                textBox.Text += horizontalPicker.SelectedItem + " ";
                this.Refresh();
            };
            Controls.Add(horizontalPicker);
            
        }
    }
}
