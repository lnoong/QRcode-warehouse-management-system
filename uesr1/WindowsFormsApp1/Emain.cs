using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Emain : Form
    {
        private object notifyIcon1;

        public Emain()
        {
            InitializeComponent();
            CreateTreeNode();
            LoadForm(MainTabControl1, "出库--入库", typeof(Forms.Form5));
        }
        private void CreateTreeNode()
        {
            int pageIndex = 1000;
            //一级节点
            TreeNode parent3 = Aside1.CreateNode("老炮儿", 62094, 22, pageIndex);
            Aside1.CreateChildNode(parent3, "出库--入库", ++pageIndex);
            Aside1.CreateChildNode(parent3, "物资管理", ++pageIndex);
            Aside1.CreateChildNode(parent3, "用户管理", ++pageIndex);
            Aside1.CreateChildNode(parent3, "相机设置", ++pageIndex);
            Aside1.CreateChildNode(parent3, "通讯设置", ++pageIndex);

        }
        private void LoadForm(TabControl mainTabControl, string menuText, Type fromType)
        {
            Form frm = (Form)Activator.CreateInstance(fromType);
            frm.Text = menuText;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;

            TabPage StabPage = new TabPage(menuText);
            StabPage.Font = new Font("宋体", 9F);
            StabPage.Controls.Add(frm);

            mainTabControl.Controls.Add(StabPage);
            mainTabControl.SelectedTab = StabPage;

            frm.Show();

        }

        private void uiNavMenu1_MenuItemClick(TreeNode node, Sunny.UI.NavMenuItem item, int pageIndex)
        {
            if (item != null)
            {
                string menuText = item.Text;
                foreach (TabPage tab in MainTabControl1.TabPages)
                {
                    if (tab.Text == menuText)
                    {
                        MainTabControl1.SelectedTab = tab;
                        return;
                    }
                }
                switch (menuText)
                {

                    case "出库&入库":
                        LoadForm(MainTabControl1, menuText, typeof(Forms.Form5));
                        break;
                    case "物资管理":
                        LoadForm(MainTabControl1, menuText, typeof(Forms.Form6));
                        break;
                    case "用户管理":
                        LoadForm(MainTabControl1, menuText, typeof(Forms.Form7));
                        break;
                    case "相机设置":
                        LoadForm(MainTabControl1, menuText, typeof(Forms.Form8));
                        break;
                    case "通讯设置":
                        LoadForm(MainTabControl1, menuText, typeof(Forms.Form9));
                        break;
                    default:
                        break;
                }

            }
        }


        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            this.Hide();
            f2.ShowDialog();
            this.Dispose();

        }
        [DllImport(".\\camera.dll")]
        public static extern int CameraClose();

        private void Close_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("请您确认是否退出(Y/N)", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                try
                {
                    this.Dispose();
                    this.Close();
                    CameraClose();//关闭相机
                    System.Environment.Exit(0);
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.notifyIcon1 = true;
        }
    }
}
