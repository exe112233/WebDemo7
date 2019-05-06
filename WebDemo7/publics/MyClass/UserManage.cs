using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebDemo7.Models;

namespace WebDemo7.publics.MyClass
{
    public class UserManage
    {
        SqlHelper mysql = new SqlHelper();
        /// <summary>
        /// 获取所有的用户集合
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUserA()
        {
            string sql = "select UserName AS 用户名,PassWord AS 密码,Email AS 邮箱,Role AS 角色 from td_user";
            //string sql = "select * from td_user";
            DataSet ds = mysql.SelectSqlReturnDataSet(sql);
            return TransformToList.ToList<User>(ds);
        }

        public string GetAllUserB()
        {
            //string sql = "select UserId AS id, UserName AS 用户名,PassWord AS 密码,Email AS 邮箱,Role AS 角色 from td_user";
            string sql = "select * from td_user";
            DataSet ds = mysql.SelectSqlReturnDataSet(sql);
            return ConvertJson.ToJson(ds);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">参数为一个user对象</param>
        /// <returns></returns>
        public int AddUser(User user)
        {
            string sql = "INSERT INTO td_user (UserName,PassWord,Email,Role) VALUES ('"+ user.UserName + "','" + user.PassWord + "','" + user.Email + "','" + user.Role + "')";
            return mysql.ExcuteSqlReturnInt(sql);
        }
    }
}