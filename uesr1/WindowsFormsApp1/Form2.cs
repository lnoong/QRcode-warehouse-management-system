using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        [DllImport(".\\camera.dll")]
        public static extern int LinkMysql();
        [DllImport(".\\camera.dll")]
        public static extern void ReadPassword(byte[] state, byte[] userid, byte[] password);
        [DllImport(".\\camera.dll")]
        public static extern void WritePassword(string state, string userid, string password);

        public Form2()
        {
            InitializeComponent();
            int state = LinkMysql();
            if (state != 0)
            {
                if (DialogResult.OK == MessageBox.Show("数据库连接失败!\n请打开安装目录下的config.ini文件配置数据库信息。", "老炮儿出入库管理系统", MessageBoxButtons.OK, MessageBoxIcon.Error))
                {
                    System.Environment.Exit(0);
                }
            }
            byte[] state2_b = new byte[5];
            byte[] userid_b = new byte[20];
            byte[] password_b = new byte[20];
            ReadPassword(state2_b, userid_b, password_b);
            string state2 = Encoding.ASCII.GetString(state2_b).Replace("\0", "");
            string userid = Encoding.ASCII.GetString(userid_b);
            string password = Encoding.ASCII.GetString(password_b);
            if (state2.Equals("true"))
            {
                cb.Checked = true;
                txtName.Text = userid;
                txtPassword.Text = password;
            }
        }
        [DllImport(".\\camera.dll")]
        public static extern int SignIn(string userid, string password);

        [DllImport(".\\camera.dll")]
        public static extern IntPtr UserInfo(string userid, string column);


        private void button1_Click(object sender, EventArgs e)
        {
            string userid = txtName.Text;
            string password = txtPassword.Text;
            int state = SignIn(userid, password);
            if (state == -1)
                MessageBox.Show("用户不存在！","老炮儿出入库管理系统", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (state == -2)
                MessageBox.Show("密码错误！","老炮儿出入库管理系统", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (state == -3)
            { }
            else if (state == 0)
            {
                IntPtr p = UserInfo(userid, "3");
                string str_out = Marshal.PtrToStringAnsi(p);

                if (cb.Checked == true)
                {
                    WritePassword("true", userid, password);
                }
                else
                    WritePassword("false", "NULL", "NULL");
                if (str_out == "2")//工程师
                {
                    Emain f6 = new Emain();
                    this.Hide();
                    f6.ShowDialog();
                    this.Dispose();
                }
                else if (str_out == "1")//管理员
                {
                    Amain f5 = new Amain();
                    this.Hide();
                    f5.ShowDialog();
                    this.Dispose();
                }
                else//操作员
                {
                    Omain f3 = new Omain();
                    this.Hide();
                    f3.ShowDialog();
                    this.Dispose();
                }

            }
            else
                MessageBox.Show("未知错误！","老炮儿出入库管理系统", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Close_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("请您确认是否退出(Y/N)", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                try
                {
                    this.Dispose();
                    this.Close();
                }
                catch (System.Exception error)
                {//WinIO卸载失败异常
                    MessageBox.Show(error.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                return;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Form7 f2 = new Form7();
            this.Hide();
            f2.ShowDialog();
            this.Dispose();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Form3 f2 = new Form3();
            this.Hide();
            f2.ShowDialog();
            this.Dispose();
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

        private void label2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("U   I: 丁亮\nD  B: 袁骁\n业务: 郑龙\n业务: 王欣\n业务: 王璐", "老炮儿出入库管理系统", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
