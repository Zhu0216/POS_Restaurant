using System;
using System.Data;
using System.Data.SQLite; // 用 SQLite 取代 OleDb
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class 系統登入 : Form
    {
        public SQLiteConnection connection = new SQLiteConnection(@"Data Source=gin8.db;Version=3;");

        public 系統登入()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string str = "SELECT * FROM 密碼";
                SQLiteCommand command = new SQLiteCommand(str, connection);
                SQLiteDataReader Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    label1.Text = Reader["密碼"].ToString();
                }

                Reader.Close();
                connection.Close();

                if (textBox2.Text == label1.Text)
                {
                    點餐 f = new 點餐();
                    this.Visible = false;
                    f.Visible = true;
                }
                else
                {
                    MessageBox.Show("密碼錯誤");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("錯誤: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
            textBox2.MaxLength = 14;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            更改密碼 f = new 更改密碼();
            this.Visible = false;
            f.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string strConnectionString = @"Data Source=gin8.db;Version=3;";
            connection = new SQLiteConnection(strConnectionString);
            try
            {

                MessageBox.Show("目前資料庫位置：" + Path.GetFullPath("gin8.db"));
                connection.Open();
            }
            catch (SQLiteException ee)
            {
                MessageBox.Show("SQLite 錯誤");
                MessageBox.Show("錯誤訊息 = " + ee.Message);
            }
            catch (InvalidOperationException eee)
            {
                MessageBox.Show("InvalidOperation: " + eee.Message);
            }

            if (connection.State != ConnectionState.Open)
            {
                MessageBox.Show("資料庫連線失敗！");
                Application.Exit();
            }
            else
            {
                MessageBox.Show("連線成功！");
                connection.Close();
            }
        }
    }
}
