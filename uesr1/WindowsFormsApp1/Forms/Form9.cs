using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp1.Forms
{
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }
        [DllImport(".\\txdyDll.dll")]
        public static extern uint ConnectTest(string ip, int port);
        public static uint conntction;
        
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            Regex ipreg = new Regex(@"\d+\.\d+\.\d+\.\d+");
            Regex portreg = new Regex(@"^\d+$");
            if (ipreg.IsMatch(uiTextBox3.Text) && portreg.IsMatch(uiTextBox1.Text))
            {
                string ip = uiTextBox3.Text;
                int port = int.Parse(uiTextBox1.Text);
                conntction = ConnectTest(ip, port);
                if (conntction == 0)
                    MessageBox.Show("连接失败");
                else
                {
                    Form5.conntction = conntction;
                    MessageBox.Show("连接成功");
                }
            }
            else
                MessageBox.Show("请输入正确的IP及端口");
        }
        [DllImport(".\\txdyDll.dll")]
        public static extern int SendData(uint m_DataSocket, string pcData, int iLength);
        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            string pcData = uiTextBox2.Text;
            SendData(conntction, pcData, pcData.Length);
        }
        [DllImport(".\\txdyDll.dll")]
        public static extern IntPtr ReceiveData(uint m_DataSocket, int nlength);
        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {
            IntPtr receiveptr = ReceiveData(conntction, 100);
            string receivestr = Marshal.PtrToStringAnsi(receiveptr);
            uiTextBox4.Text = receivestr;
        }
        [DllImport(".\\txdyDll.dll")]
        public static extern void ConnectSave(string ip, int port);
        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            Regex ipreg = new Regex(@"\d+\.\d+\.\d+\.\d+");
            Regex portreg = new Regex(@"^\d+$");
            if (ipreg.IsMatch(uiTextBox3.Text) && portreg.IsMatch(uiTextBox1.Text))
            {
                string ip = uiTextBox3.Text;
                int port = int.Parse(uiTextBox1.Text);
                ConnectSave(ip, port);
            }
            else
                MessageBox.Show("请输入正确的IP及端口");
        }
    }
}
