using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDemo7.Models;
using WebDemo7.publics.MyClass;

namespace WebDemo7.Controllers
{
    public class UserManageController : Controller
    {
        UserManage userManage = new UserManage();
        // GET: UserManage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserManageGrid()
        {
            return View();
        }

        public ActionResult AddUser()
        {
            User user = new User();
            
            user.UserName = Request.Params["userName"];
            user.PassWord = Request.Params["userPass"];
            user.Email = Request.Params["userEmail"];
            user.Role = "5";
            if(userManage.AddUser(user) > 0)
            {
                return Content("true");
            }
            else
            {
                return Content("false");
            }
        }

        public ActionResult GetAllUser()
        {
            string json = userManage.GetAllUserB();
            return Content(json,"text/html"); 
        }
    }    
}