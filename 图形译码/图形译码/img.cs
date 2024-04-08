using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 图形译码.Properties;

namespace 图形译码
{
    internal class imgs
    {
        private int number;//图片对应的数字
        private String path;//图片的路径
        public String Path { get => this.path; }//只读属性，获取图片路径
        public int Number { get => this.number; }//只读属性，获取图片对应的数字

        public imgs(int n, int num)//构造函数
        {
            this.number = n; //设置图片对应的数字
            this.path = $"D:\\new code\\图形译码\\图形译码\\Resources\\{num}.png";//设置图片的路径

        }
    }
}
