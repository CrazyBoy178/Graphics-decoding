using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 图形译码
{
    internal class Dao
    {
        SqlConnection sc;// 数据库连接对象
        // 连接数据库，返回连接对象
        public SqlConnection connect()
        {
            string str = @"Data Source=LAPTOP-2F32NPK8 ;Initial Catalog=GraphicsDecoding;Integrated Security=True";//数据库连接字符串
            sc = new SqlConnection(str);//创建数据库连接对象
            sc.Open();//打开数据库
            return sc;//返回数据库
        }
        // 创建 SqlCommand 对象，并返回
        public SqlCommand command(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, connect());
            return cmd;
        }
        // 执行更新操作，并返回更新的行数
        public int Execute(string sql)//更新操作
        {
            return command(sql).ExecuteNonQuery();
        }
        // 执行查询操作，并返回 SqlDataReader 对象
        public SqlDataReader read(string sql)//读取操作
        {
            return command(sql).ExecuteReader();
        }
        // 关闭数据库连接
        public void DaoClose()
        {
            sc.Close();//关闭数据库连接
        }
    }
}
