using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebDemo7.Models
{
    /// <summary>
    /// 这是一个表的实体类
    /// 需要注意的点:
    /// 1、主键上需要添加[Key]特性;
    /// 2、类名一定要与表名一致;
    /// 3、类的成员属性一定要与表的字段类型一致;
    /// </summary>
    public class User
    {
        public string UserName { set; get; }
        public string PassWord { set; get; }
        public string Email { set; get; }
        public string Role { set; get; }
    }
}