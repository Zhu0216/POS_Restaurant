using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class 點餐 : Form
    {
        public SQLiteConnection connection = new SQLiteConnection("Data Source=gin8.db;Version=3;");

        public 點餐()
        {
            InitializeComponent();
            LoadMenu("冬季限定");
        }

        private void LoadMenu(string tableName)
        {
            try
            {
                connection.Open();
                string sql = $"SELECT * FROM [{tableName}]";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                dataGridView2.AutoResizeColumns();
            }
            finally
            {
                connection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                MessageBox.Show("請選擇桌號");
                return;
            }
            if (label7.Text == "label7")
            {
                MessageBox.Show("請選擇品項");
                return;
            }
            if (comboBox1.Text == "0")
            {
                MessageBox.Show("請選擇數量");
                return;
            }

            label9.Text = (double.Parse(label8.Text) * double.Parse(comboBox1.Text)).ToString();
            dataGridView1.Rows.Add(comboBox2.Text, label7.Text, label8.Text, comboBox1.Text, label9.Text, dateTimePicker1.Text);
            dataGridView1.AutoResizeColumns();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 1)
            {
                MessageBox.Show("請選擇餐點");
                return;
            }

            int sum = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    sum += Convert.ToInt32(row.Cells[3].Value);
                }
            }
            label4.Text = sum.ToString();

            try
            {
                connection.Open();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string insertSql = "INSERT INTO 交易紀錄 (日期, 品項, 單價, 數量, 桌號, 小計) VALUES (@日期, @品項, @單價, @數量, @桌號, @小計)";
                        SQLiteCommand insertCmd = new SQLiteCommand(insertSql, connection);
                        insertCmd.Parameters.AddWithValue("@日期", row.Cells[5].Value);
                        insertCmd.Parameters.AddWithValue("@品項", row.Cells[1].Value);
                        insertCmd.Parameters.AddWithValue("@單價", row.Cells[2].Value);
                        insertCmd.Parameters.AddWithValue("@數量", row.Cells[3].Value);
                        insertCmd.Parameters.AddWithValue("@桌號", row.Cells[0].Value);
                        insertCmd.Parameters.AddWithValue("@小計", row.Cells[4].Value);
                        insertCmd.ExecuteNonQuery();

                        string insertPending = "INSERT INTO 未結帳明細 (日期, 品項, 單價, 數量, 桌號, 小計) VALUES (@日期, @品項, @單價, @數量, @桌號, @小計)";
                        SQLiteCommand pendingCmd = new SQLiteCommand(insertPending, connection);
                        pendingCmd.Parameters.AddWithValue("@日期", row.Cells[5].Value);
                        pendingCmd.Parameters.AddWithValue("@品項", row.Cells[1].Value);
                        pendingCmd.Parameters.AddWithValue("@單價", row.Cells[2].Value);
                        pendingCmd.Parameters.AddWithValue("@數量", row.Cells[3].Value);
                        pendingCmd.Parameters.AddWithValue("@桌號", row.Cells[0].Value);
                        pendingCmd.Parameters.AddWithValue("@小計", row.Cells[4].Value);
                        pendingCmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("訂單已送出");
                dataGridView1.Rows.Clear();
            }
            finally
            {
                connection.Close();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dataGridView1.SelectedRows)
            {
                if (!r.IsNewRow)
                {
                    dataGridView1.Rows.Remove(r);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (label7.Text != "label7")
            {
                label9.Text = (double.Parse(label8.Text) * double.Parse(comboBox1.Text)).ToString();
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 1)
            {
                MessageBox.Show("尚有訂單未送出");
            }
            else
            {
                後台 f = new 後台();
                this.Visible = false;
                f.Visible = true;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            label7.Text = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
            label8.Text = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
            label7.Visible = true;
            label8.Visible = true;
            comboBox1.Text = "1";
            label9.Text = (double.Parse(label8.Text) * double.Parse(comboBox1.Text)).ToString();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox3.Text))
            {
                LoadMenu(comboBox3.Text);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            系統登入 f = new 系統登入();
            this.Visible = false;
            f.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e) => LoadMenu("冬季限定");
        private void button4_Click(object sender, EventArgs e) => LoadMenu("冰品");
        private void button5_Click(object sender, EventArgs e) => LoadMenu("豆花加糖水");
        private void button6_Click(object sender, EventArgs e) => LoadMenu("夏季限定");
        private void button7_Click(object sender, EventArgs e) => LoadMenu("桂冠湯圓系列");
        private void button37_Click(object sender, EventArgs e) => LoadMenu("綠豆湯系列");
    }
}
