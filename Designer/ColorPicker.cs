using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Designer
{
    public partial class ColorPicker : Form
    {
        public ColorPicker()
        {
            InitializeComponent();
        }

        MouseHook _hook;
        private void ColorPicker_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _hook = new MouseHook(hookproc);
            this.Width = 70;
            IntPtr cur = Win32API.LoadCursor(IntPtr.Zero, 32512);
            Win32API.SetCursor(cur);
        }
        private void hookproc(int ncode, IntPtr wparam, IntPtr lparam)
        {
            if (ncode >= 0 &&
                Win32API.MouseMessages.WM_MOUSEMOVE == (Win32API.MouseMessages)wparam)
            {
                Win32API.MSLLHOOKSTRUCT hookStruct = (Win32API.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lparam, typeof(Win32API.MSLLHOOKSTRUCT));
                Point pt = hookStruct.pt;
                this.Location = new Point(pt.X+30, pt.Y);
                Bitmap img = ScreenCapture.captureDesktopRegion(new Point(pt.X, pt.Y), new Point(pt.X+1, pt.Y+1));
                Graphics e = Graphics.FromImage(pictureBox1.Image);
                Color pick = img.GetPixel(0, 0);
                e.FillRectangle(new SolidBrush(pick), new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
                pictureBox1.Invalidate();
                label1.Text = pick.R.ToString();
                label2.Text = pick.G.ToString();
                label3.Text = pick.B.ToString();
            }
        }
    }
}
