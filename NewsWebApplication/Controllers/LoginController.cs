using Microsoft.AspNetCore.Mvc;
using EntityFrameworkDemo.DB;
using NewsWebApplication.Models;
using System.Dynamic;

namespace NewsWebApplication.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string loginName, string loginPwd)
        {
            if (havingornot(loginName, loginPwd) == true)
            {
                using DBContext context = new DBContext();
                var userid = from User in context.User
                             where User.UserName == loginName
                             select new
                             {
                                 User.Id
                             };
                foreach (var i in userid)
                {
                    HttpContext.Session.SetInt32("UserId", i.Id);
                }
                HttpContext.Session.SetString("UserName", loginName);
                //Console.WriteLine(loginName);
                return RedirectToAction("Index", "Home");
            } else if(loginName =="admin" && loginPwd == "123456"){
                return RedirectToAction("Admin", "Home");
            }
            else
            {
                TempData["Message"] = "The Password or Username is wrong.";
                return RedirectToAction("Index", "Login");
            }
        }
        public IActionResult Logout(){

            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Index", "Home");
    }

        public IActionResult Signup(string name,string password,string email) {
            using DBContext context = new DBContext();
            context.Add(new User
            {
                UserName = name,
                Password = password,
                Email = email

            });
            context.SaveChanges();
            //获取刚刚注册的用户的用户Id
            var userid = from User in context.User
                         where User.UserName == name
                         select new
                         {
                             User.Id
                         };
            var UserId = 0;
            foreach (var i in userid)
            {
                UserId = i.Id;
            }
            //添加该用户对于每个新闻的推荐值
            List<dynamic> NewId = GetNewId();
            foreach (var id in NewId)
            {
                context.Add(new Recommend
                {
                    UserId = UserId,
                    NewId = id.Id,
                    Recommendation = 0
                });
            }
            context.SaveChanges();
            HttpContext.Session.SetString("UserName", name);
            return RedirectToAction("Index", "Home");
        }
        private bool havingornot(string UserName, string PassWord)
        {
            using DBContext context = new DBContext();
            var user = context.User.FirstOrDefault(s => s.UserName == UserName && s.Password == PassWord);
            if (user == null)
            {
                return false;
            }
            else
            { return true; }
        }
        private List<dynamic> GetNewId() { 
            using DBContext context = new DBContext();
            var NewId = from New in context.New
                        select new
                        {
                            New.Id,
                        };
            List<dynamic> Id = new List<dynamic>();
            foreach (var item in NewId)
            {
                dynamic dyObject = new ExpandoObject();
                dyObject.Id = item.Id;
                Id.Add(dyObject);
            }
            return Id;
        }
    }
}
