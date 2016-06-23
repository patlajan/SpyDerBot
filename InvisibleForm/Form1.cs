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

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs args)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;

            //var timer = new System.Timers.Timer(50);
            //timer.Elapsed += (s, e) => this.HandleTimer();
            //timer.Start();
        }

        private void HandleTimer()
        {
            Stopwatch _watch = Stopwatch.StartNew();
            var bmp = TakeScreen("Skype");
            //this.BackgroundImage = bmp;

            _watch.Stop();
            Debug.WriteLine(_watch.Elapsed.TotalMilliseconds);
        }

        private Bitmap TakeScreen(string procName)
        {
            var proc = Process.GetProcessesByName(procName);
            Bitmap bmp = null;
            RECT rc;

            foreach (var p in proc) {
                var hwnd = p.MainWindowHandle;
                GetWindowRect(hwnd, out rc);

                if (rc.Height == 0 || rc.Width == 0) continue;

                this.Size = rc.Size;
                this.Location = rc.Location;
                
                bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
                Graphics gfxBmp = Graphics.FromImage(bmp);
                IntPtr hdcBitmap = gfxBmp.GetHdc();

                PrintWindow(hwnd, hdcBitmap, 0);

                gfxBmp.ReleaseHdc(hdcBitmap);
                gfxBmp.Dispose();
            }
            return bmp;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.HandleTimer();

            System.Drawing.Graphics graphics = this.CreateGraphics();
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(1, 1, this.Width-1, this.Height-1);
            graphics.DrawRectangle(System.Drawing.Pens.Red, rectangle);
        }
    }
}
