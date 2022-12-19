using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApp1.Forms
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            string selectMode = uiComboBox1.Text;
            string selectInfo = uiTextBox2.Text;
            string sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object;";
            if (selectMode == "按名称查询")
            {
                if (selectInfo == "" || selectInfo == "请输入信息...")
                    sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object ORDER BY objectname";
                else
                    sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object where objectname =" + "\"" + selectInfo.ToString() + "\"";
            }
            else if (selectMode == "按ID查询")
            {
                if (selectInfo == "" || selectInfo == "请输入信息...")
                    sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object  ORDER BY objectid";
                else
                {
                    sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object  where objectid =" + "\"" + selectInfo.ToString() + "\"";
                }
            }
            else if (selectMode == "按种类查询")
            {
                if (selectInfo == "" || selectInfo == "请输入信息...")
                    sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object  ORDER BY type";
                else
                {
                    sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object  where type =" + "\"" + selectInfo.ToString() + "\"";
                }
            }
            else if (selectMode == "按库位查询")
            {
                if (selectInfo == "" || selectInfo == "请输入信息...")
                    sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object  ORDER BY place";
                else
                {
                    sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object  where place =" + "\"" + selectInfo.ToString() + "\"";
                }
            }
            else if (selectMode == "按在库状态查询")
            {
                if (selectInfo == "" || selectInfo == "请输入信息...")
                    sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object  ORDER BY state";
                else
                {
                    sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object  where state =" + "\"" + selectInfo.ToString() + "\"";
                }
            }
            else if (selectMode == "按时间查询")
            {
                if (selectInfo == "" || selectInfo == "请输入信息...")
                    sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object  ORDER BY lasttime";
                else
                {
                    sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object  where lasttime LIKE " + "\"%" + selectInfo.ToString() + "%\"";
                }
            }
            else
                MessageBox.Show("请输入正确信息");
            uiDataGridView1.ClearAll();
            mda = new MySqlDataAdapter(sql, conn);
            ds = new DataSet();
            mda.Fill(ds, "object");
            //显示数据
            uiDataGridView1.DataSource = ds.Tables["object"];
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
            string sql = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object;";
            mda = new MySqlDataAdapter(sql, conn);
            ds = new DataSet();
            mda.Fill(ds, "object");
            //显示数据
            uiDataGridView1.DataSource = ds.Tables["object"];
            conn.Open();
        }

        private void uiSymbolButton2_Click_1(object sender, EventArgs e)
        {
            try
            {
                MySqlCommandBuilder builder = new MySqlCommandBuilder(mda); //命令生成器。
                mda.Update(ds, "object");
                uiDataGridView1.ClearAll();
                string sqld = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object;";
                mda = new MySqlDataAdapter(sqld, conn);
                ds = new DataSet();
                mda.Fill(ds, "object");
                //显示数据
                uiDataGridView1.DataSource = ds.Tables["object"];
                MessageBox.Show("添加成功", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入正确数据", "提示");
            }
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            int index = uiDataGridView1.CurrentCell.RowIndex;
            string id = uiDataGridView1.Rows[index].Cells[1].Value.ToString();
            string sql = "delete from object where objectid=" + id + "";
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            int i = cmd.ExecuteNonQuery();
            uiDataGridView1.ClearAll();
            string sqld = "SELECT objectname AS '物品名',objectid AS '物品ID',type AS'种类',place AS'库位',state AS '在库状态',lasttime AS '最后出入库时间'FROM object;";
            mda = new MySqlDataAdapter(sqld, conn);
            ds = new DataSet();
            mda.Fill(ds, "object");
            //显示数据
            uiDataGridView1.DataSource = ds.Tables["object"];
            if (i < 0)
            {
                MessageBox.Show("删除失败");
            }
        }

        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlCommandBuilder builder = new MySqlCommandBuilder(mda); //命令生成器。
                mda.Update(ds, "object");
                MessageBox.Show("修改成功", "提示");

            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入正确数据", "错误信息");
            }
        }

        private void uiDataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("请输入正确信息！","老炮儿出入库管理系统",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }
}
