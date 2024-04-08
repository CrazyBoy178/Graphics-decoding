using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using ContentAlignment = System.Drawing.ContentAlignment;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;

namespace 图形译码
{
    public partial class Game : Form
    {

        [DllImport("winmm.dll")]
        public static extern uint timeGetTime();

        private double time = 0;//表示使用 DLLImport 属性导入 winmm.dll 动态链接库。
        private int n;// 定义两个整型变量n和m记录行和列。
        private int m;
        private double accuracy = 0;//定义精确度
        private uint startTime;//定义开始时间
        private int right = 0;//添加正确的
        Boolean flag = false;//游戏状态 未开始

        Label label = new Label();

        private Timer timer = new Timer();//表示定义一个名为timer的计时器。
        private Random random = new Random();//表示定义一个名为random的随机数生成器。
        private TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();//表示定义一个名为tableLayoutPanel的表格布局控件。
        private ImageList imageList = new ImageList();//表示定义一个名为imageList的图片列表控件。


        private PictureBox[] pictureBoxes;//表示定义一个名为pictureBoxes的PictureBox类型的数组。
        private Label[] labels;//表示定义一个名为labels的Label类型的数组。
        private imgs[] images;//表示定义一个名为images的imgs类型的数组。
        private TextBox[,] game_number;//表示定义名为game_number和game_picture的二维数组，表示游戏中的数字和图片。
        private PictureBox[,] game_picture;
        private int[] rightNumber;//表示定义一个名为rightNumber的整型数组，用于保存正确的数字。
        private Panel[,] panels; //表示定义一个名为panels的Panel类型的二维数组。


        void setTimer()
        {

            startTime = timeGetTime();

            timer.Interval = 1;

            timer.Tick += new EventHandler(timer_Tick);
        }

        public Game(int n, int m)
        {
            InitializeComponent();
            this.n = n;
            this.m = m;
            this.label.Text = "时间:0.000";
            this.label.Location = new Point(10, 50);
            this.Controls.Add(label);

            this.tableLayoutPanel.ColumnCount = n;
            this.tableLayoutPanel.RowCount = m;
            this.tableLayoutPanel.AutoScroll = true;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.pictureBoxes = new PictureBox[this.n];

            this.labels = new Label[this.n];
            this.images = new imgs[this.n];
            this.game_picture = new PictureBox[this.n, this.m];
            this.game_number = new TextBox[this.n, this.m];
            this.rightNumber = new int[this.n * this.m];
            this.panels = new Panel[this.n, this.m];

            imageList.ImageSize = new Size(50, 50);
        }


        private void 重新开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.timer.Stop();
            button1.Enabled = true;
            button2.Enabled = false;
            flag = false;
            remove();
            imageList.Images.Clear();
            this.right = 0;
            this.time = 0;
            label.Text = "时间:0.000s";
            show_Img();
        }

        private void remove()
        {
            for (int i = 0; i < n; i++)
            {
                this.Controls.Remove(pictureBoxes[i]);
                this.Controls.Remove(labels[i]);
            }
            tableLayoutPanel.Controls.Clear();
            this.Controls.Remove(tableLayoutPanel);
        }


        private void Game_Load(object sender, EventArgs e)
        {
            label.Text = "时间:0.000s";
            show_Img();

        }

        void timer_Tick(object sender, EventArgs e)
        {
            uint currentTime = timeGetTime();
            this.time = (currentTime - startTime) / 1000.0;
            label.Text = "时间:" + time.ToString("F3") + "s";
        }

        private void 查询历史成绩ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            timer.Stop();
            this.Hide();
            History h = new History();
            h.ShowDialog();
            if (flag)
            {
                timer.Start();
            }

            this.Show();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            flag = true;
            setTimer();
            add_Imgs();
            Controls.Add(tableLayoutPanel);

            timer.Start();
            button2.Enabled = true;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            timer.Stop();
            button2.Enabled = false;
            flag = false;
            Judge();
            this.accuracy = Math.Round(right / (double)(n * m), 3);
            MessageBox.Show($"游戏结束！时间:{time} 正确率:{this.accuracy * 100}%");
            Dao dao = new Dao();
            string sql = $"insert into Gameinfo(col,row,time,game_time,Accuracy) values('{this.n}','{this.m}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{this.time}','{this.accuracy}');";
            SqlDataReader dc = dao.read(sql);
            if (dc != null)
            {
                MessageBox.Show("成绩保存成功");
            }
            else
            {
                MessageBox.Show("保存成绩失败");
            }
            dc.Close();
        }


        private void add_Imgs()
        {

            tableLayoutPanel.Size = new Size(1000, m * 100);
            tableLayoutPanel.Location = new Point(100, 50);


            tableLayoutPanel.ColumnStyles.Clear();
            for (int i = 0; i < tableLayoutPanel.ColumnCount; i++)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / tableLayoutPanel.ColumnCount));
            }
            tableLayoutPanel.RowStyles.Clear();
            for (int i = 0; i < tableLayoutPanel.RowCount; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / tableLayoutPanel.RowCount));
            }

            int sum = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    int r = random.Next(this.n);

                    game_picture[i, j] = new PictureBox();
                    game_picture[i, j].SizeMode = PictureBoxSizeMode.Zoom;
                    game_picture[i, j].Dock = DockStyle.Top;

                    rightNumber[sum++] = images[r].Number;

                    game_number[i, j] = new TextBox();
                    game_number[i, j].Size = new Size(30, 5);
                    game_number[i, j].TextAlign = HorizontalAlignment.Center;
                    game_number[i, j].Dock = DockStyle.Bottom;

                    panels[i, j] = new Panel();
                    panels[i, j].Size = new Size(50, 50);
                    panels[i, j].Dock = DockStyle.Fill;
                    game_picture[i, j].Image = imageList.Images[r];

                    panels[i, j].Controls.Add(game_picture[i, j]);
                    panels[i, j].Controls.Add(game_number[i, j]);

                    if (tableLayoutPanel.RowCount <= 10 || tableLayoutPanel.ColumnCount <= 10)
                    {
                        game_picture[i, j].Size = new Size(50, 50);
                    }
                    else if (tableLayoutPanel.RowCount <= 15 && tableLayoutPanel.RowCount > 10 || tableLayoutPanel.ColumnCount <= 15 && tableLayoutPanel.ColumnCount > 10)
                    {
                        game_picture[i, j].Size = new Size(35, 35);
                    }
                    else if (tableLayoutPanel.RowCount > 15 && tableLayoutPanel.RowCount <= 18 || tableLayoutPanel.ColumnCount > 15 && tableLayoutPanel.ColumnCount <= 18)
                    {
                        game_picture[i, j].Size = new Size(25, 25);
                    }
                    else
                    {
                        game_picture[i, j].Size = new Size(20, 20);
                    }

                    tableLayoutPanel.Controls.Add(panels[i, j], i, j);
                }

            }
        }


        private void Judge()
        {
            int num;
            int sum = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    game_number[i, j].ReadOnly = true;
                    if (game_number[i, j].Text != "")
                    {
                        try
                        {
                            num = Convert.ToInt32(game_number[i, j].Text);
                        }
                        catch
                        {
                            num = 0;
                        }

                        if (num == rightNumber[sum++])
                        {
                            right++;
                        }
                    }
                    else
                    {
                        sum++;
                        continue;
                    }
                }
            }
        }

        private void show_Img()
        {

            int[] numbers = getRandom();


            for (int i = 1; i <= images.Length; i++)
            {
                images[i - 1] = new imgs(i, numbers[i - 1]);
            }

            for (int i = 1; i <= n; i++)
            {
                Image image = Image.FromFile(images[i - 1].Path);
                imageList.Images.Add(image);
            }

            for (int i = 0; i < n; i++)
            {
                pictureBoxes[i] = new PictureBox();
                pictureBoxes[i].Size = new Size(50, 50);
                pictureBoxes[i].Location = new Point(i % 5 * 50 + 1100, i / 5 * 100 + 270);
                pictureBoxes[i].Image = imageList.Images[i];
                this.Controls.Add(pictureBoxes[i]);

                labels[i] = new Label();
                labels[i].Size = new Size(50, labels[i].Height);
                labels[i].Font = new Font("宋体", 16);
                labels[i].Text = (i + 1).ToString();
                labels[i].TextAlign = ContentAlignment.MiddleCenter;
                labels[i].Location = new Point(i % 5 * 50 + 1100, i / 5 * 100 + 320);
                this.Controls.Add(labels[i]);

            }
        }

        private int[] getRandom()
        {
            List<int> list = new List<int>();
            for (int i = 1; i <= this.n; i++)
            {
                int num;
                do
                {
                    num = random.Next(1, 21);
                } while (list.Contains(num));
                list.Add(num);
            }
            return list.ToArray();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
