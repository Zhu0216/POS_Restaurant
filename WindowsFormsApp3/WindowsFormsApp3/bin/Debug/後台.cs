using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class 後台 : Form
    {
        public SQLiteConnection connection = new SQLiteConnection("Data Source=gin8.db;Version=3;");

        public 後台()
        {
            InitializeComponent();
            this.textBox2.KeyPress += new KeyPressEventHandler(this.textBox2_KeyPress);
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void Form2_Load(object sender, EventArgs e) { }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "未結帳明細" || comboBox1.Text == "交易紀錄")
            {
                MessageBox.Show("請勿修改交易紀錄");
                return;
            }

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("請輸入欲新增品項之價格");
                return;
            }

            string table = comboBox1.Text;
            string item = textBox1.Text;
            int price;

            if (!int.TryParse(textBox2.Text, out price))
            {
                MessageBox.Show("請輸入有效價格");
                return;
            }

            try
            {
                connection.Open();
                string insert = $"INSERT INTO {table} (品項, 單價) VALUES (@item, @price)";
                SQLiteCommand cmd = new SQLiteCommand(insert, connection);
                cmd.Parameters.AddWithValue("@item", item);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.ExecuteNonQuery();

                RefreshDataGrid(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show("錯誤: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "未結帳明細" || comboBox1.Text == "交易紀錄")
            {
                MessageBox.Show("請勿修改交易紀錄");
                return;
            }

            string table = comboBox1.Text;
            string item = textBox1.Text;

            try
            {
                connection.Open();
                string delete = $"DELETE FROM {table} WHERE 品項 = @item";
                SQLiteCommand cmd = new SQLiteCommand(delete, connection);
                cmd.Parameters.AddWithValue("@item", item);
                cmd.ExecuteNonQuery();
                RefreshDataGrid(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show("錯誤: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "未結帳明細" || comboBox1.Text == "交易紀錄")
            {
                MessageBox.Show("請勿修改交易紀錄");
                return;
            }

            string table = comboBox1.Text;
            string item = textBox1.Text;
            int price;

            if (!int.TryParse(textBox2.Text, out price))
            {
                MessageBox.Show("請輸入有效價格");
                return;
            }

            try
            {
                connection.Open();
                string update = $"UPDATE {table} SET 單價 = @price WHERE 品項 = @item";
                SQLiteCommand cmd = new SQLiteCommand(update, connection);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@item", item);
                cmd.ExecuteNonQuery();
                RefreshDataGrid(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show("錯誤: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            點餐 f = new 點餐();
            this.Visible = false;
            f.Visible = true;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("未選擇桌號，無法結帳");
                return;
            }

            label6.Visible = true;
            label7.Visible = true;
            string tableNo = comboBox2.Text;
            try
            {
                connection.Open();
                string query = "SELECT * FROM 未結帳明細 WHERE 桌號 = @tableNo";
                SQLiteDataAdapter da = new SQLiteDataAdapter(query, connection);
                da.SelectCommand.Parameters.AddWithValue("@tableNo", tableNo);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                int sum = 0;
                foreach (DataRow row in dt.Rows)
                {
                    sum += Convert.ToInt32(row[5]);
                }
                label8.Text = sum.ToString();
                label6.Text = "桌號 : " + tableNo;

                DialogResult result = MessageBox.Show($"桌號 : {tableNo} 消費金額 {label8.Text}, 確認結帳?", "", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    string delete = "DELETE FROM 未結帳明細 WHERE 桌號 = @tableNo";
                    SQLiteCommand cmd = new SQLiteCommand(delete, connection);
                    cmd.Parameters.AddWithValue("@tableNo", tableNo);
                    cmd.ExecuteNonQuery();
                    RefreshDataGrid("未結帳明細");
                    MessageBox.Show("已完成結帳!");
                }
                else
                {
                    MessageBox.Show("結帳已取消");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("錯誤: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                RefreshDataGrid(comboBox1.Text);
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                textBox1.Text = "";
                textBox2.Text = "";
                comboBox2.SelectedIndex = -1;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                string tableNo = comboBox2.Text;
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM 未結帳明細 WHERE 桌號 = @tableNo";
                    SQLiteDataAdapter da = new SQLiteDataAdapter(query, connection);
                    da.SelectCommand.Parameters.AddWithValue("@tableNo", tableNo);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoResizeColumns();
                    label6.Visible = true;
                    label7.Visible = true;
                    label8.Visible = true;
                    int sum = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        sum += Convert.ToInt32(row[5]);
                    }
                    label8.Text = sum.ToString();
                    label6.Text = "桌號 : " + tableNo;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    comboBox1.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("錯誤: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (comboBox1.Text != "未結帳明細" && comboBox1.Text != "交易紀錄")
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
        }

        private void RefreshDataGrid(string table)
        {
            string query = $"SELECT * FROM {table}";
            SQLiteDataAdapter da = new SQLiteDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoResizeColumns();
        }
    }
}
