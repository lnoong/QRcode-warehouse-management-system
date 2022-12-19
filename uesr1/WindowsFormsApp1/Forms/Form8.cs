using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp1.Forms
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        [DllImport(".\\camera.dll")]
        public static extern int CameraSet(double exposure, double gain, double gamma);
        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^\d+(\.\d+)?$");
            if (regex.IsMatch(uiTextBox1.Text) && regex.IsMatch(uiTextBox2.Text) && regex.IsMatch(uiTextBox4.Text))
            {
                double exposure = double.Parse(uiTextBox1.Text);
                double gain = double.Parse(uiTextBox2.Text);
                double gamma = double.Parse(uiTextBox4.Text);
                CameraSet(exposure, gain, gamma);
            }
            else
                MessageBox.Show("请输入正确的值");
        }
        [DllImport(".\\camera.dll")]

        public static extern void ImageToCs(IntPtr a);
        [DllImport(".\\camera.dll")]
        public static extern void DetectQR(int a);
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            DetectQR(0);
            Bitmap bmp = new Bitmap(1440, 1080, PixelFormat.Format24bppRgb);
            Rectangle rectangle = new Rectangle(0, 0, 1440, 1080);
            while (Visible)
            {
                BitmapData bmd = bmp.LockBits(rectangle, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                ImageToCs(bmd.Scan0);
                bmp.UnlockBits(bmd);
                pictureBox1.Image = bmp;
                Application.DoEvents();
            }
        }
    }
}
