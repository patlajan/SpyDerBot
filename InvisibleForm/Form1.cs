using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace InvisibleForm
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs args)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;

            var timer = new System.Timers.Timer(50);
            timer.Elapsed += (s, e) => this.HandleTimer();
            timer.Start();
        }

        private void HandleTimer()
        {
            Stopwatch _watch = Stopwatch.StartNew();
            var bmp = TakeScreen("WindowsForms10.Window.20008.app.0.3e799b_r13_ad1");
            this.BackgroundImage = bmp;

            _watch.Stop();
            Debug.WriteLine(_watch.Elapsed.TotalMilliseconds);
        }

        private Bitmap TakeScreen(string procName)
        {
            Bitmap bmp = null;
            RECT rc;
            var hwnd = FindWindowByCaption(IntPtr.Zero, "Bluestacks App Player");
            GetWindowRect(hwnd, out rc);

            bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(hwnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();
            return bmp;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RECT rc;
            var hwnd = FindWindowByCaption(IntPtr.Zero, "Bluestacks App Player");
            GetWindowRect(hwnd, out rc);
            this.Size = rc.Size;
            this.Location = rc.Location;
        }
    }
}
