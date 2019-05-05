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
        public List<User> getAllUser()
        {
            //string sql = "select UserName AS 用户名,PassWord AS 密码,Email AS 邮箱,Role AS 角色 from td_user";
            string sql = "select * from td_user";
            DataSet ds = mysql.SelectSqlReturnDataSet(sql);
            return TransformToList.ToList<User>(ds);
        }

        public int addUser(User user)
        {
            string sql = "INSERT INTO td_user (UserName,PassWord,Email,Role) VALUES ('"+ user.UserName + "','" + user.PassWord + "','" + user.Email + "','" + user.Role + "')";
            return mysql.ExcuteSqlReturnInt(sql);
        }
    }
}