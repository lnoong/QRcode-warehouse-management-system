using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApp1.Forms
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        MySqlConnection conn; //连接数据库对象
        MySqlDataAdapter mda; //适配器变量
        DataSet ds;  //临时数据集
        [DllImport(".\\camera.dll")]
        public static extern IntPtr MysqlLinkInfo();
        public static IntPtr link = MysqlLinkInfo();
        public static string linksql = Marshal.PtrToStringAnsi(link);
        private void FormLoad(object sender, EventArgs e)
        {

            string M_str_sqlcon = linksql;
            conn = new MySqlConnection(M_str_sqlcon);
            string sql = "SELECT username AS '用户名',userid AS '用户ID',email AS'邮箱',power AS'权限',password AS '密码'FROM user;";
            mda = new MySqlDataAdapter(sql, conn);
            ds = new DataSet();
            mda.Fill(ds, "user");
            //显示数据
            uiDataGridView1.DataSource = ds.Tables["user"];
            conn.Open();
        }

        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlCommandBuilder builder = new MySqlCommandBuilder(mda); //命令生成器。
                mda.Update(ds, "user");
                MessageBox.Show("修改成功", "提示");

            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入正确数据", "错误信息");
                //MessageBox.Show(ex.Message);
            }
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            int index = uiDataGridView1.CurrentCell.RowIndex;
            string id = uiDataGridView1.Rows[index].Cells[1].Value.ToString();
            string sql = "delete from user where userid=" + id + "";
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            int i = cmd.ExecuteNonQuery();
            uiDataGridView1.ClearAll();
            string sqld = "SELECT username AS '用户名',userid AS '用户ID',email AS'邮箱',power AS'权限',password AS '密码'FROM user;";
            mda = new MySqlDataAdapter(sqld, conn);
            ds = new DataSet();
            mda.Fill(ds, "user");
            //显示数据
            uiDataGridView1.DataSource = ds.Tables["user"];
            if (i < 0)
            {
                MessageBox.Show("删除失败");
            }
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlCommandBuilder builder = new MySqlCommandBuilder(mda); //命令生成器。
                mda.Update(ds, "user");
                uiDataGridView1.ClearAll();
                string sqld = "SELECT username AS '用户名',userid AS '用户ID',email AS'邮箱',power AS'权限',password AS '密码'FROM user;";
                mda = new MySqlDataAdapter(sqld, conn);
                ds = new DataSet();
                mda.Fill(ds, "user");
                //显示数据
                uiDataGridView1.DataSource = ds.Tables["user"];
                MessageBox.Show("添加成功", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入正确数据", "提示");
            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            string selectMode = uiComboBox1.Text;
            string selectInfo = uiTextBox2.Text;
            string sql = "SELECT username AS '用户名',userid AS '用户ID',email AS'邮箱',power AS'权限',password AS '密码'FROM user;";
            if (selectMode == "按姓名查询")
            {
                if (selectInfo == "" || selectInfo == "请输入信息...")
                    sql = "select username AS '用户名',userid AS '用户ID',email AS'邮箱',power AS'权限',password AS '密码' from user ORDER BY username";
                else
                    sql = "select username AS '用户名',userid AS '用户ID',email AS'邮箱',power AS'权限',password AS '密码' from user where username =" + "\"" + selectInfo.ToString() + "\"";
            }
            else if (selectMode == "按ID查询")
            {
                if (selectInfo == "" || selectInfo == "请输入信息...")
                    sql = "select username AS '用户名',userid AS '用户ID',email AS'邮箱',power AS'权限',password AS '密码' from user ORDER BY userid";
                else
                {
                    sql = "select username AS '用户名',userid AS '用户ID',email AS'邮箱',power AS'权限',password AS '密码' from user where userid =" + "\"" + selectInfo.ToString() + "\"";
                }
            }
            else if (selectMode == "按邮箱查询")
            {
                if (selectInfo == "" || selectInfo == "请输入信息...")
                    sql = "select username AS '用户名',userid AS '用户ID',email AS'邮箱',power AS'权限',password AS '密码' from user ORDER BY email";
                else
                {
                    sql = "select username AS '用户名',userid AS '用户ID',email AS'邮箱',power AS'权限',password AS '密码' from user where email =" + "\"" + selectInfo.ToString() + "\"";
                }
            }
            else if (selectMode == "按权限查询")
            {
                if (selectInfo == "" || selectInfo == "请输入信息...")
                    sql = "select username AS '用户名',userid AS '用户ID',email AS'邮箱',power AS'权限',password AS '密码' from user ORDER BY power";
                else
                {
                    sql = "select username AS '用户名',userid AS '用户ID',email AS'邮箱',power AS'权限',password AS '密码' from user where power =" + "\"" + selectInfo.ToString() + "\"";
                }
            }
            else
                MessageBox.Show("请输入正确信息");
            uiDataGridView1.ClearAll();
            mda = new MySqlDataAdapter(sql, conn);
            ds = new DataSet();
            mda.Fill(ds, "user");
            //显示数据
            uiDataGridView1.DataSource = ds.Tables["user"];
        }

        private void DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("请输入正确信息！","老炮儿出入库管理系统",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }
}
