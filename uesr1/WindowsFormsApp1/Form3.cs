using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        public void SetWindowRegion()
        {
            System.Drawing.Drawing2D.GraphicsPath FormPath;
            FormPath = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle rect = new Rectangle(0, 22, this.Width, this.Height - 22);//this.Left-10,this.Top-10,this.Width-10,this.Height-10);                 
            FormPath = GetRoundedRectPath(rect, 30);
            this.Region = new Region(FormPath);
        }
        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();
            //   左上角   
            path.AddArc(arcRect, 180, 90);
            //   右上角   
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);
            //   右下角   
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);
            //   左下角   
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();
            return path;
        }
        protected override void OnResize(System.EventArgs e)
        {
            this.Region = null;
            SetWindowRegion();
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            this.Hide();
            f2.ShowDialog();
            this.Dispose();
        }
        [DllImport(".\\camera.dll")]
        public static extern int SignUp(string userName, string userID, string password, string password_retype, string email);

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtName.Text;
            string userid = textBox5.Text;
            string password = textBox3.Text;
            string password_retype = textBox4.Text;
            string email = textBox1.Text;
            int state = SignUp(username, userid, password, password_retype, email);
            if (checkBox1.Checked == true)
            {
                if (state == -1)
                    MessageBox.Show("用户已存在");
                else if (state == -2)
                    MessageBox.Show("用户名长度为3-20的所有字符");
                else if (state == -3)
                    MessageBox.Show("账号为11位手机号");
                else if (state == -4)
                    MessageBox.Show("密码为长度在8~18之间,大写或小写字母+数字+字符");
                else if (state == -5)
                {
                    if (password != password_retype)
                        MessageBox.Show("两次密码不一致");
                    else
                        MessageBox.Show("邮箱不合规");
                }
                else if (state == -6)
                    MessageBox.Show("两次密码不一致");
                else if (state == 0)
                {
                    MessageBox.Show("注册成功！");
                    Form2 f2 = new Form2();
                    this.Hide();
                    f2.ShowDialog();
                    this.Dispose();
                }
                else
                    MessageBox.Show("未知错误");
            }
            else
                MessageBox.Show("请勾选服务条款");
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            this.Hide();
            f2.ShowDialog();
            this.Dispose();
        }

        private void label4_Click_1(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            this.Hide();
            f2.ShowDialog();
            this.Dispose();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(".\\web\\index.html");
        }
    }
}
