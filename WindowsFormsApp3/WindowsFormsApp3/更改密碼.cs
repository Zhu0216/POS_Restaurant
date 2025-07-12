using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class 更改密碼 : Form
    {
        public SQLiteConnection connection = new SQLiteConnection(@"Data Source=gin8.db;Version=3;");

        public 更改密碼()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            系統登入 f = new 系統登入();
            this.Visible = false;
            f.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                // 讀取舊密碼
                string str = "SELECT * FROM 密碼";
                SQLiteCommand command = new SQLiteCommand(str, connection);
                SQLiteDataReader Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    label3.Text = Reader["密碼"].ToString();
                }

                Reader.Close();
                connection.Close();

                // 驗證原密碼與輸入相同
                if (textBox1.Text == label3.Text)
                {
                    if (textBox2.Text == textBox3.Text)
                    {
                        connection.Open();
                        string updateStr = "UPDATE 密碼 SET 密碼 = @newPwd";
                        SQLiteCommand updateCmd = new SQLiteCommand(updateStr, connection);
                        updateCmd.Parameters.AddWithValue("@newPwd", textBox2.Text);
                        updateCmd.ExecuteNonQuery();
                        connection.Close();

                        MessageBox.Show("密碼已修改為：" + textBox2.Text);
                    }
                    else
                    {
                        MessageBox.Show("新密碼不相同，請確認新密碼");
                    }
                }
                else
                {
                    MessageBox.Show("原密碼錯誤");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("錯誤：" + ex.Message);
            }
        }

        private void 更改密碼_Load(object sender, EventArgs e)
        {

        }
    }
}
