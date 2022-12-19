using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Omain : Form
    {
        private bool notifyIcon1;

        public Omain()
        {
            InitializeComponent();
            CreateTreeNode();
            LoadForm(MainTabControl, "出库--入库", typeof(Forms.Form5));

        }
        //创建节点

        private void CreateTreeNode()
        {
            int pageIndex = 1000;
            //一级节点
            TreeNode parent1 = Aside.CreateNode("老炮儿", 62094, 22, pageIndex);
            //二级节点

            Aside.CreateChildNode(parent1, "出库--入库", ++pageIndex);
        }
        private void Aside_MenuItemClick(TreeNode node, Sunny.UI.NavMenuItem item, int pageIndex)
        {
            if (item != null)
            {
                string menuText = item.Text;
                foreach (TabPage tab in MainTabControl.TabPages)
                {
                    if (tab.Text == menuText)
                    {
                        MainTabControl.SelectedTab = tab;
                        return;
                    }
                }
                switch (menuText)
                {
                    case "出库&入库":
                        LoadForm(MainTabControl, menuText, typeof(Forms.Form5));
                        break;

                    default:
                        break;
                }

            }
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

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            this.Hide();
            f2.ShowDialog();
            this.Dispose();
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
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
