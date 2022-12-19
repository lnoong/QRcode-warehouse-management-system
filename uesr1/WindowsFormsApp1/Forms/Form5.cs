using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1.Forms
{

    public partial class Form5 : Form
    {
        public static uint conntction;

        public Form5()
        {
            InitializeComponent();
            conntction = Connect();
            int state = (int)conntction;
            if (state == 0)
            {
                MessageBox.Show("PLC连接失败!\n请打开安装目录下的config.ini文件配置PLC信息。", "老炮儿出入库管理系统", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Thread t = new Thread(MessageToPlc);
                t.Start();
                t.IsBackground = true;
            }
        }

        [DllImport(".\\camera.dll")]
        public static extern int ObjectAdd(string oname, string type, string place);
        private void uiButton1_Click(object sender, EventArgs e)
        {
            string oname = uiTextBox2.Text;
            string type = uiComboBox1.Text;
            string place = uiComboBox3.Text;
            if (oname == "")
            {
                MessageBox.Show("请输入物品名称!","老炮儿出入库管理系统", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else 
            {
                if (type == "大" && place != "请选择库位")
                {
                    ObjectAdd(oname, "1", place);
                    int temp;
                    int.TryParse(place,out temp);
                    place = "1"+temp.ToString("00");
                    messageList.Add(place);
                    databaseUpdate();
                    MessageBox.Show("入库成功!","老炮儿出入库管理系统", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (type == "小" && place != "请选择库位")
                {
                    ObjectAdd(oname, "0", place);
                    int temp;
                    int.TryParse(place, out temp);
                    place = "1" + temp.ToString("00");
                    messageList.Add(place);
                    databaseUpdate();
                    MessageBox.Show(String.Join("", "入库成功!"),"老炮儿出入库管理系统", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("请选择物品类型!","老炮儿出入库管理系统", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        [DllImport(".\\camera.dll")]
        public static extern IntPtr MysqlLinkInfo();
        public static IntPtr link = MysqlLinkInfo();
        public static string linksql = Marshal.PtrToStringAnsi(link);
        public static string connect_str = linksql;
        public static MySqlConnection connect = new MySqlConnection(connect_str);
        public static MySqlCommand comm = new MySqlCommand();
        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {
            string selectMode = uiComboBox2.Text;
            string selectInfo = uiTextBox1.Text;
            string sql = "select * from object";
            if (selectMode == "按物品名称查询")
            {
                if (selectInfo == "" || selectInfo == "请输入信息...")
                    sql = "select * from object ORDER BY objectname";
                else
                    sql = "select * from object where objectname =" + "\"" + selectInfo.ToString() + "\"";
            }
            else if (selectMode == "按物品ID查询")
            {
                if (selectInfo == "" || selectInfo == "请输入信息...")
                    sql = "select * from object ORDER BY objectid";
                else
                {
                    sql = "select * from object where objectid =" + "\"" + selectInfo.ToString() + "\"";
                }
            }
            else if (selectMode == "按物品类型查询")
            {
                if (selectInfo == "大")
                    selectInfo = "1";
                else if (selectInfo == "小")
                    selectInfo = "0";
                else
                { }

                if (selectInfo == "" || selectInfo == "请输入信息...")
                    sql = "select * from object ORDER BY type";
                else
                    sql = "select * from object where type =" + "\"" + selectInfo.ToString() + "\"";
            }
            uiDataGridView1.Rows.Clear();
            MySqlDataAdapter database_un = new MySqlDataAdapter(sql, connect);
            DataSet database = new DataSet();
            database_un.Fill(database, "object");
            DataTable objectTable = database.Tables["object"];
            int indexes = 0;
            for (indexes = 0; indexes < objectTable.Rows.Count; indexes++)
            {
                uiDataGridView1.Rows.Add();
                uiDataGridView1.Rows[indexes].Cells[1].Value = objectTable.Rows[indexes][0].ToString();
                uiDataGridView1.Rows[indexes].Cells[2].Value = objectTable.Rows[indexes][1].ToString();
                uiDataGridView1.Rows[indexes].Cells[3].Value = objectTable.Rows[indexes][2].ToString();
                if (objectTable.Rows[indexes][2].ToString() == "1")
                    uiDataGridView1.Rows[indexes].Cells[3].Value = "大";
                else
                    uiDataGridView1.Rows[indexes].Cells[3].Value = "小";
                uiDataGridView1.Rows[indexes].Cells[4].Value = objectTable.Rows[indexes][3].ToString();
                uiDataGridView1.Rows[indexes].Cells[5].Value = objectTable.Rows[indexes][4].ToString();
                if (objectTable.Rows[indexes][4].ToString() == "0")
                {
                    uiDataGridView1.Rows[indexes].Cells[0].ReadOnly = true;
                    uiDataGridView1.Rows[indexes].Cells[5].Value = "否";
                }
                else
                {
                    uiDataGridView1.Rows[indexes].Cells[0].ReadOnly = false;
                    uiDataGridView1.Rows[indexes].Cells[5].Value = "是";
                }
                uiDataGridView1.Rows[indexes].Cells[6].Value = objectTable.Rows[indexes][5].ToString();
            }
        }



        [DllImport(".\\txdyDll.dll")]
        public static extern uint Connect();

        private void DataView_Load(object sender, EventArgs e)
        {
            comm.Connection = connect;
            connect.Open();
            string sql = "select * from object";
            MySqlDataAdapter database_un = new MySqlDataAdapter(sql, connect);
            DataSet database = new DataSet();
            database_un.Fill(database, "object");
            DataTable objectTable = database.Tables["object"];
            int indexes = 0;
            for (indexes = 0; indexes < objectTable.Rows.Count; indexes++)
            {
                uiDataGridView1.Rows.Add();
                uiDataGridView1.Rows[indexes].Cells[1].Value = objectTable.Rows[indexes][0].ToString();
                uiDataGridView1.Rows[indexes].Cells[2].Value = objectTable.Rows[indexes][1].ToString();
                uiDataGridView1.Rows[indexes].Cells[3].Value = objectTable.Rows[indexes][2].ToString();
                if (objectTable.Rows[indexes][2].ToString() == "1")
                    uiDataGridView1.Rows[indexes].Cells[3].Value = "大";
                else
                    uiDataGridView1.Rows[indexes].Cells[3].Value = "小";
                uiDataGridView1.Rows[indexes].Cells[4].Value = objectTable.Rows[indexes][3].ToString();
                uiDataGridView1.Rows[indexes].Cells[5].Value = objectTable.Rows[indexes][4].ToString();
                if (objectTable.Rows[indexes][4].ToString() == "0")
                {
                    uiDataGridView1.Rows[indexes].Cells[0].ReadOnly = true;
                    uiDataGridView1.Rows[indexes].Cells[5].Value = "否";
                }
                else
                {
                    uiDataGridView1.Rows[indexes].Cells[0].ReadOnly = false;
                    uiDataGridView1.Rows[indexes].Cells[5].Value = "是";
                }
                uiDataGridView1.Rows[indexes].Cells[6].Value = objectTable.Rows[indexes][5].ToString();

            }
        }
        [DllImport(".\\txdyDll.dll")]
        public static extern int SendData(uint a, string data, int lenth);
        private void 出库_Click(object sender, EventArgs e)
        {
            List<string> objectidList = new List<string>();
            List<string> placeList = new List<string>();
            
            for (int iRow = 0; iRow < uiDataGridView1.RowCount; iRow++)
            {
                if (Convert.ToBoolean(uiDataGridView1.Rows[iRow].Cells[0].EditedFormattedValue.ToString()))
                {
                    string id = uiDataGridView1.Rows[iRow].Cells[2].EditedFormattedValue.ToString();
                    string place = uiDataGridView1.Rows[iRow].Cells[4].EditedFormattedValue.ToString();
                    objectidList.Add(id);
                    placeList.Add(place);
                }
            }
            for (int iRow = 0; iRow < objectidList.Count; iRow++)
            {
                comm.CommandText = String.Format(" update object set state = '0' where objectid= " + objectidList[iRow]);
                comm.ExecuteNonQuery();
            }
            databaseUpdate();
            /*通讯*/
            if (objectidList.Count() > 0)
            {
                string place_out = "0";
                for (int i = 0; i < placeList.Count(); i++)
                {
                    int temp;
                    int.TryParse(placeList[i], out temp);
                    place_out = "0" + temp.ToString("00");
                    messageList.Add(place_out);
                }
                objectidList.Clear();
                placeList.Clear();
                MessageBox.Show("出库成功！", "老炮儿出入库管理系统", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        [DllImport(".\\camera.dll")]
        public static extern void ImageToCs(IntPtr a);
        [DllImport(".\\camera.dll")]
        public static extern void DetectQR(int a);
        private void uiButton2_Click(object sender, EventArgs e)
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
        private void databaseUpdate()
        {
            uiDataGridView1.Rows.Clear();
            string sql = "select * from object";
            MySqlDataAdapter database_un = new MySqlDataAdapter(sql, connect);
            DataSet database = new DataSet();
            database_un.Fill(database, "object");
            DataTable objectTable = database.Tables["object"];
            int indexes = 0;
            for (indexes = 0; indexes < objectTable.Rows.Count; indexes++)
            {
                uiDataGridView1.Rows.Add();
                uiDataGridView1.Rows[indexes].Cells[1].Value = objectTable.Rows[indexes][0].ToString();
                uiDataGridView1.Rows[indexes].Cells[2].Value = objectTable.Rows[indexes][1].ToString();
                uiDataGridView1.Rows[indexes].Cells[3].Value = objectTable.Rows[indexes][2].ToString();
                if (objectTable.Rows[indexes][2].ToString() == "1")
                    uiDataGridView1.Rows[indexes].Cells[3].Value = "大";
                else
                    uiDataGridView1.Rows[indexes].Cells[3].Value = "小";
                uiDataGridView1.Rows[indexes].Cells[4].Value = objectTable.Rows[indexes][3].ToString();
                uiDataGridView1.Rows[indexes].Cells[5].Value = objectTable.Rows[indexes][4].ToString();
                if (objectTable.Rows[indexes][4].ToString() == "0")
                {
                    uiDataGridView1.Rows[indexes].Cells[0].ReadOnly = true;
                    uiDataGridView1.Rows[indexes].Cells[5].Value = "否";
                }
                else
                {
                    uiDataGridView1.Rows[indexes].Cells[0].ReadOnly = false;
                    uiDataGridView1.Rows[indexes].Cells[5].Value = "是";
                }
                uiDataGridView1.Rows[indexes].Cells[6].Value = objectTable.Rows[indexes][5].ToString();
            }
        }

        [DllImport(".\\camera.dll")]
        public static extern IntPtr ObjectInfo(string id, string coloum);

        [DllImport(".\\camera.dll")]
        public static extern IntPtr StrToCs();

        private void uiButton3_Click(object sender, EventArgs e)
        {
            IntPtr strptr = StrToCs();
            string id = Marshal.PtrToStringAnsi(strptr);
            comm.CommandText = String.Format(" update object set state = '1' where objectid= " + id);
            int check = comm.ExecuteNonQuery();
            if (check == 1)
            {
                IntPtr place = ObjectInfo(id, "3");
                string place_out = Marshal.PtrToStringAnsi(place);
                int temp;
                int.TryParse(place_out, out temp);
                place_out = "1" + temp.ToString("00");
                messageList.Add(place_out);
                MessageBox.Show(place_out + "入库成功！", "老炮儿出入库管理系统", MessageBoxButtons.OK, MessageBoxIcon.Information);
                databaseUpdate();
            }
            else
            {
                MessageBox.Show("二维码错误！","老炮儿出入库管理系统", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public static List<string> messageList = new List<string>();
        [DllImport(".\\txdyDll.dll")]
        public static extern IntPtr ReceiveData(uint m_DataSocket, int nlength);
        public static void MessageToPlc()
        {
            while (true)
            {
                IntPtr plcstate = ReceiveData(conntction, 1);
                string plcstate_s = Marshal.PtrToStringAnsi(plcstate);
                if (plcstate_s == "1" && messageList.Count() > 0)
                {
                    SendData(conntction, messageList[0], messageList[0].Length);
                    plcstate_s = "0";
                    messageList.RemoveAt(0);
                }
                else
                { }
            }
        }

    }
}
