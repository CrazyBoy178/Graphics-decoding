using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace 图形译码
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
            this.SetBounds(0, 0, 600, 500);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

        }


        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("欢迎下次游玩");
            Application.Exit();


        }

        private void button1_Click(object sender, EventArgs e)
        {

            int n = Convert.ToInt32(comboBox1.SelectedItem);
            int m = Convert.ToInt32(comboBox2.SelectedItem);
            if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null && n >= 5 && n <= 20 && m >= 5 && m <= 20)
            {
                game(n, m);
            }
            else
            {
                MessageBox.Show("error");
            }
        }
        void game(int n, int m)
        {
            MessageBox.Show("输入正确,即将开始游戏,祝您游戏愉快!!!");
            this.Hide();
            Game g1 = new Game(n, m);
            g1.ShowDialog();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            History h1 = new History();
            h1.ShowDialog();
            this.Show();
        }
    }
}
