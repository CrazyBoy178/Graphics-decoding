using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 图形译码
{
    public partial class History : Form
    {
        private int n;
        private int m;
        public History()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public void Table(string sql)
        {
            dataGridView1.Rows.Clear();
            Dao dao = new Dao();
            SqlDataReader dc = dao.read(sql);
            while (dc.Read())
            {
                //向DataGridView控件中添加一行记录
                dataGridView1.Rows.Add(dc[3].ToString(), dc[4].ToString() + 'S',
                    (Convert.ToDouble(dc[5].ToString()) * 100) + "%");
            }
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("没有信息");
            }
            dc.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "" && textBox2.Text == "")
                {
                    this.n = Convert.ToInt32(textBox1.Text);
                    string sql = $"select * from Gameinfo where col = {n}";
                    Table(sql);
                    textBox1.Text = "";
                }
                else if (textBox1.Text == "" && textBox2.Text != "")
                {
                    this.m = Convert.ToInt32(textBox2.Text);
                    string sql = $"select * from Gameinfo where row = {m}";
                    Table(sql);
                    textBox2.Text = "";
                }
                else if (textBox1.Text != "" && textBox2.Text != "")
                {
                    this.n = Convert.ToInt32(textBox1.Text);
                    this.m = Convert.ToInt32(textBox2.Text);
                    string sql = $"select TOP 1 * from Gameinfo where col = {n} and row = {m} ORDER BY Accuracy DESC,game_time ASC";
                    Table(sql);
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
                else
                {
                    MessageBox.Show("请输入m或n");
                }
            }
            catch
            {
                MessageBox.Show("输入框有误,请重新输入");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
