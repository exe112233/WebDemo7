using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.Reflection;

namespace WebDemo7.publics.MyClass
{
    public class SqlHelper
    {
        private static bool IsCanConnectioned = false;

        //返回一个连接字符串。
        public string MyConnString
        {
            get
            {
                return "Data Source=.;Initial Catalog = Test;User ID = sa;password = Aa312313;";

            }
        }

        //测试数据库是否连接成功
        public bool ConnectionTest()
        {
            SqlConnection conn = new SqlConnection(MyConnString);
            try
            {
                //Open DataBase
                //打开数据库
                conn.Open();
                IsCanConnectioned = true;
                return IsCanConnectioned;
            }
            catch
            {
                //Can not Open DataBase
                //打开不成功 则连接不成功
                return IsCanConnectioned;
            }
            finally
            {
                //Close DataBase
                //关闭数据库连接
                conn.Close();
            }
        }
        //运行Sql语句，返回查询到的第一个表的第一行数据行数 。
        public object Run_SQL(string SQL, string MyConnString)
        {
            SqlConnection Conn = new SqlConnection(MyConnString);
            SqlCommand Cmd = new SqlCommand(SQL, Conn);
            Conn.Open();
            try
            {
                object result_count = Cmd.ExecuteScalar();
                Conn.Close();
                return result_count;
            }
            catch
            {
                Conn.Close();
                return 0;
            }
        }
        //
        //基于SQL命令实现数据库操作，返回值是受影响的行数，可实现insert,updata,delete命令。
        public int ExcuteSqlReturnInt(string sql)
        {
            //创建连接对象conn.
            SqlConnection conn = new SqlConnection(MyConnString);
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                {
                    conn.Open();
                }
                int count = cmd.ExecuteNonQuery();
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        //ExcuteSqlReturnInt方法带参数的重载,基于SQL命令实现数据库操作,返回值是受影响的行数,可实现insert,updata,delete命令,
        public int ExcuteSqlReturnInt(string sql, SqlParameter[] pars)
        {
            //创建连接对象conn.
            SqlConnection conn = new SqlConnection(MyConnString);
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                {
                    conn.Open();
                }
                if (pars != null && pars.Length > 0)
                {
                    foreach (SqlParameter p in pars)
                    {
                        //遍历pars参数数组元素，将参数填到SQL命令中。
                        cmd.Parameters.Add(p);
                    }
                }
                int count = cmd.ExecuteNonQuery();
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        //ExcuteSqlReturnInt方法带参数的重载,基于存储过程实现数据库操作,返回值是受影响的行数,可实现insert,updata,delete命令
        public int ExcuteSqlReturnInt(string sql, SqlParameter[] pars, CommandType type)
        {
            //创建连接对象conn.
            SqlConnection conn = new SqlConnection(MyConnString);
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                {
                    conn.Open();
                }
                if (pars != null && pars.Length > 0)
                {
                    foreach (SqlParameter p in pars)
                    {
                        //遍历pars参数数组元素，将参数填到SQL命令中。
                        cmd.Parameters.Add(p);
                    }
                }
                cmd.CommandType = type; //判断调用方式
                int count = cmd.ExecuteNonQuery();
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }


        //基于SQL命令实现数据库操作，返回数据集合Data，可实现数据查询select。
        public DataSet SelectSqlReturnDataSet(string sql)
        {
            SqlConnection conn = new SqlConnection(MyConnString);
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
                sda.Fill(ds, "ds");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return ds;

        }

        //SelectSqlReturnDataSet方法带参数的重载,用于实现带参数的查询.SqlDataAdapter数据适配器对象不用手动Open.CommandType 参数指定的是命令方式,确定是调用数据库的存储方式还是用SQL命令.
        public DataSet SelectSqlReturnDataSet(string sql, SqlParameter[] pars, CommandType type)
        {
            SqlConnection conn = new SqlConnection(MyConnString);
            SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
            if (pars != null && pars.Length > 0)
            {
                foreach (SqlParameter p in pars)
                {
                    //遍历pars参数数组元素，将参数填到SQL命令中。
                    sda.SelectCommand.Parameters.Add(p);
                }
            }
            sda.SelectCommand.CommandType = type;//根据传进来的值,来确定调用方式.
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }

        public DataTable SelectSqlReturnDataTable(string sql, SqlParameter[] pars, CommandType type)
        {
            SqlConnection conn = new SqlConnection(MyConnString);
            SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
            if (pars != null && pars.Length > 0)
            {
                foreach (SqlParameter p in pars)
                {
                    //遍历pars参数数组元素，将参数填到SQL命令中。
                    sda.SelectCommand.Parameters.Add(p);
                }
            }
            sda.SelectCommand.CommandType = type;//根据传进来的值,来确定调用方式.
            DataSet ds = new DataSet();
            sda.Fill(ds, "MyTable");              //将数据传给DataSet的MyTable表,
            return ds.Tables["MyTable"];
        }


        //基于SQL语句利用数据阅读器实现数据库查询操作
        public SqlDataReader SelectSqlReturnDataReader(string sql)
        {
            SqlConnection conn = new SqlConnection(MyConnString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            conn.Close();
            return reader;

        }


        //基于SQL语句或存储过程,利用数据阅读器实现数据库查询操作
        public SqlDataReader SelectSqlReturnDataReader(string sql, SqlParameter[] pars, CommandType type)
        {
            SqlConnection conn = new SqlConnection(MyConnString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            if (pars != null && pars.Length > 0)
            {
                foreach (SqlParameter p in pars)
                {
                    //遍历pars参数数组元素，将参数填到SQL命令中。
                    cmd.Parameters.Add(p);
                }
            }
            cmd.CommandType = type;
            //当应用程序完成数据获取后，会自动关闭阅读器和与阅读器相关联的连接
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;

        }

        //EXCEL数据读取到DataSet组件的静态方法,参数为EXCEL文件的文件名
        static public DataSet ExcelToDataSet(string filename)
        {
            DataSet ds;
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                            "Extended Properties=Excel 8.0;" +
                            "data source=" + filename;
            OleDbConnection myConn = new OleDbConnection(strCon);
            string strCom = " SELECT * FROM [Sheet1$]";
            myConn.Open();
            OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
            ds = new DataSet();
            myCommand.Fill(ds);
            myConn.Close();
            return ds;
        }

      

        /// <summary>
        /// 把dataset数据转换成json的格式
        /// </summary>
        /// <param name="ds">dataset数据集</param>
        /// <returns>json格式的字符串</returns>
        public string GetJsonByDataset(DataSet ds)
        {
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                //如果查询到的数据为空则返回标记ok:false
                return "{\"ok\":false}";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"ok\":true,");
            foreach (DataTable dt in ds.Tables)
            {
                sb.Append(string.Format("\"{0}\":[", dt.TableName));

                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("{");
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\",", dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"), ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                    }
                    sb.Remove(sb.ToString().LastIndexOf(','), 1);
                    sb.Append("},");
                }

                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("],");
            }
            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 将object转换成为string
        /// </summary>
        /// <param name="ob">obj对象</param>
        /// <returns></returns>
        public static string ObjToStr(object ob)
        {
            if (ob == null)
            {
                return string.Empty;
            }
            else
                return ob.ToString();
        }
    }
}