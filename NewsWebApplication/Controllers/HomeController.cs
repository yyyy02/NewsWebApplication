using Microsoft.AspNetCore.Mvc;
using NewsWebApplication.Models;
using System.Data.Common;
using System.Diagnostics;
using System.Dynamic;
using EntityFrameworkDemo.DB;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore.Metadata;


namespace NewsWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly SynchronizationService _synchronizationService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, SynchronizationService synchronizationService)
        {
            _logger = logger;
            _synchronizationService = synchronizationService;
        }

        public IActionResult Index()
        {

            //get the current user
            ViewData["Username"] = HttpContext.Session.GetString("UserName");
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            //Console.WriteLine(HttpContext.Session.GetInt32("UserId"));
            using DBContext context = new DBContext();
            //HttpClient http = new HttpClient();
            //var str = "";
            //HttpContent content = new StringContent(str);

            //string url = "http://192.168.1.102:5000/";
            //Task<HttpResponseMessage> postTask = http.PostAsync(url, content); 
            //HttpResponseMessage result= postTask.Result;
            //result.EnsureSuccessStatusCode();
            //Task<string> task = result.Content.ReadAsStringAsync();
            //Console.WriteLine(task.Result);

            //_synchronizationService.GetData();




            int UserId = 0;
            var userid = from User in context.User
                         where User.UserName == HttpContext.Session.GetString("UserName")
                         select new
                         {
                             User.Id
                         };
            foreach (var i in userid)
            {
                UserId = i.Id;
            }
            //HttpClient http = new HttpClient();
            //string url = "http://192.168.1.102:5000/";
            //Task<string> task = http.GetStringAsync(url);
            //Console.WriteLine(task.Result);

            List<dynamic> heat = HeatList();
            ViewBag.RecommNews = null;
            Random random = new Random();

            //Get the Recommend list by Re.exe
            List<string> globalValue = GetCsv();

            if (HttpContext.Session.GetString("UserName") != null && globalValue == null)
            {
                List<string> ReList = RecommendationList(UserId);
                //List<string> ReList = new List<string> { "1", "2", "4", "5", "6", "7", "8", "9", "10" };
                List<dynamic> ReCommList = new List<dynamic>();
                ReCommList = GetNews(ReList);
                ViewBag.RecommNews = ReCommList;
            }
            else if (HttpContext.Session.GetString("UserName") != null && globalValue != null) {
                List<dynamic> ReCommList = new List<dynamic>();
                ReCommList = GetNews(globalValue);
                ViewBag.RecommNews = ReCommList;
            }
            else {
                List<string> ReList = new List<string>();
                while (ReList.Count <= 40) {
                    String temp = random.Next(1, 915).ToString();
                    if (!ReList.Contains(temp)) {
                        ReList.Add(temp);
                    }
                }
                List<dynamic> ReCommList = new List<dynamic>();
                ReCommList = GetNews(ReList);
                ViewBag.RecommNews = ReCommList;
            }
            //获取轮播图新闻
            List<string> PicList = new List<string> { };
            while (PicList.Count <= 8)
            {
                String temp = random.Next(1, 915).ToString();
                if (!PicList.Contains(temp))
                {
                    PicList.Add(temp);
                }
            }
            List<dynamic> PicList1 = GetNews(PicList);
            ViewBag.PicNews = PicList1;
            ViewBag.HeatNews = heat;
            return View();
        }

        public IActionResult Admin()
        {
            using DBContext context = new DBContext();
            var data = (from User in context.User
                        select new
                        {
                            User.Id,
                            User.UserName,
                            User.Email,
                            User.Password,
                        }).ToList();
            List<dynamic> UserList = new List<dynamic>();
            foreach (var item in data)
            {
                dynamic dyObject = new ExpandoObject();
                dyObject.Id = item.Id;
                dyObject.UserName = item.UserName;
                dyObject.Email = item.Email;
                dyObject.Password = item.Password;
                UserList.Add(dyObject);
            }
            ViewBag.User = UserList;

            List<dynamic> News = new List<dynamic>();
            var data2 = (from New in context.New
                         select new
                         {
                             New.Id,
                             New.NewColumn,
                             New.NewContent,
                             New.NewTitle,
                             New.Date,
                             New.NewPicture,
                             New.NewHeat
                         }).ToList();
            foreach (var item in data2)
            {
                dynamic dyObject = new ExpandoObject();
                dyObject.NewId = item.Id;
                dyObject.NewColumn = item.NewColumn;
                dyObject.NewTitle = item.NewTitle;
                dyObject.NewContent = item.NewContent;
                dyObject.NewDate = item.Date;
                dyObject.NewPicture = item.NewPicture;
                dyObject.NewHeat = item.NewHeat;
                News.Add(dyObject);
            }
            ViewBag.News = News;

            var data3 = (from Feedback in context.Feedback
                         select new
                         {
                             Feedback.Id,
                             Feedback.FeedbackContent,
                         }).ToList();
            List<dynamic> FeedbackList = new List<dynamic>();
            foreach (var item in data3)
            {
                dynamic dyObject = new ExpandoObject();
                dyObject.Id = item.Id;
                dyObject.Feedback = item.FeedbackContent;
                FeedbackList.Add(dyObject);
            }
            ViewBag.Feedback = FeedbackList;

            var data4 = (from Comment in context.Comment
                         select new
                         {
                             Comment.Id,
                             Comment.NewId,
                             Comment.UserId,
                             Comment.CommentDetail
                         }).ToList();
            List<dynamic> CommentList = new List<dynamic>();
            foreach (var item in data4)
            {
                dynamic dyObject = new ExpandoObject();
                dyObject.Id = item.Id;
                dyObject.NewId = item.NewId;
                dyObject.UserId = item.UserId;
                dyObject.CommentDetail = item.CommentDetail;
                CommentList.Add(dyObject);
            }
            ViewBag.Comment = CommentList;
            return View();
        }

        public IActionResult Channel(string column)
        {
            ViewBag.column = column;
            ViewData["Username"] = HttpContext.Session.GetString("UserName");
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            using DBContext context = new DBContext();
            var data = (from New in context.New
                        where New.NewColumn == column
                        select new
                        {
                            New.Id,
                            New.NewContent,
                            New.NewTitle,
                            New.Date,
                            New.NewPicture
                        }).ToList();
            List<dynamic> Newsdata = new List<dynamic>();
            foreach (var item in data)
            {
                dynamic dyObject = new ExpandoObject();
                dyObject.Id = item.Id;
                dyObject.Title = item.NewTitle;
                dyObject.Date = item.Date;
                dyObject.Content = item.NewContent;
                dyObject.Picture = item.NewPicture;
                dyObject.Column = column;
                Newsdata.Add(dyObject);
            }
            ViewBag.dyObject = Newsdata;

            var data2 = (from New in context.New
                         orderby New.NewHeat descending
                         where New.NewColumn == column
                         select new
                         {
                             New.Id,
                             New.NewColumn,
                             New.NewTitle,
                             New.NewPicture,
                         }).ToList();
            List<dynamic> TopicNews = new List<dynamic>();
            foreach (var item in data2)
            {
                dynamic dyObject = new ExpandoObject();
                dyObject.Id = item.Id;
                dyObject.Title = item.NewTitle;
                dyObject.Column = item.NewColumn;
                dyObject.Picture = item.NewPicture;
                TopicNews.Add(dyObject);
            }
            ViewBag.TopicNews = TopicNews;
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Search(string search)
        {
            using DBContext context = new DBContext();
            if (search != null) {
                var SelectNews = (from New in context.New
                                  where New.NewTitle.Contains(search)
                                  select new
                                  {
                                      New.Id,
                                      New.NewColumn,
                                      New.NewContent,
                                      New.NewTitle,
                                      New.Date,
                                      New.NewPicture,
                                      New.NewHeat
                                  }).ToList();
                List<dynamic> Newsdata = new List<dynamic>();
                foreach (var item in SelectNews)
                {
                    dynamic dyObject = new ExpandoObject();
                    dyObject.Id = item.Id;
                    dyObject.Title = item.NewTitle;
                    dyObject.Date = item.Date;
                    dyObject.Content = item.NewContent;
                    dyObject.Picture = item.NewPicture;
                    dyObject.Column = item.NewColumn;
                    dyObject.Heat = item.NewHeat;
                    Newsdata.Add(dyObject);
                }
                ViewBag.SearchNews = Newsdata;
            }
            ViewData["Username"] = HttpContext.Session.GetString("UserName");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new MVCFilter { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //返回热度表
        public List<dynamic> HeatList() {
            using DBContext context = new DBContext();
            var data = (from New in context.New
                        orderby New.NewHeat descending
                        select new
                        {
                            New.Id,
                            New.NewColumn,
                            New.NewTitle
                        }).ToList();
            List<dynamic> HeatNews = new List<dynamic>();
            foreach (var item in data)
            {
                dynamic dyObject = new ExpandoObject();
                dyObject.Id = item.Id;
                dyObject.Title = item.NewTitle;
                dyObject.Column = item.NewColumn;
                HeatNews.Add(dyObject);
            }
            return HeatNews;
        }

        public List<string> GetCsv() {
            string Path = @"D:\Desktop\alg\dist\Re\Re.exe";
            string Li = "";
            Process p = new Process();
            p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = Path;
            p.StartInfo.UseShellExecute = false;
            p.OutputDataReceived += (sender, argsx) =>
            {
                Li += argsx.Data;
                //Console.WriteLine(argsx.Data);
            };
            p.Start();
            p.BeginOutputReadLine();
            while (Li.Length == 0)
            {
                p.WaitForExit(1000);
            }
            p.Close();
            Li = Li.Substring(1, Li.Length - 2);
            var list = Li.Split(',').ToList();
            return list;
        }
        public List<string> RecommendationList(int UserId) {
            string Path = @"D:\Desktop\alg\dist\Reg.exe";
            string Li = "";
            Process p = new Process();
            p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = Path;
            p.StartInfo.UseShellExecute = false;
            String m = UserId.ToString();
            p.StartInfo.Arguments = m;
            p.OutputDataReceived += (sender, argsx) =>
            {
                Li += argsx.Data;
                //Console.WriteLine(argsx.Data);
            };
            p.Start();
            p.BeginOutputReadLine();
            while (Li.Length == 0) {
                p.WaitForExit(1000);
            }
            p.Close();
            //Console.WriteLine(result.Substring(1,2));
            //Process pro = Process.Start(Path, "4");
            //pro.WaitForExit();
            //int Re = pro.ExitCode;
            //pro.Close();
            Li = Li.Substring(1, Li.Length - 2);
            var list = Li.Split(',').ToList();
            return list;
        }


        public List<dynamic> GetNews(List<string> ReList)
        {
            using DBContext context = new DBContext();
            List<dynamic> ReCommList = new List<dynamic>();
            foreach (var i in ReList)
            {
                var data = (from New in context.New
                            where New.Id == int.Parse(i)
                            select new
                            {
                                New.Id,
                                New.NewColumn,
                                New.NewContent,
                                New.NewTitle,
                                New.Date,
                                New.NewPicture,
                                New.NewHeat
                            }).ToList();
                foreach (var item in data)
                {
                    dynamic dyObject = new ExpandoObject();
                    dyObject.NewId = item.Id;
                    dyObject.NewColumn = item.NewColumn;
                    dyObject.NewTitle = item.NewTitle;
                    dyObject.NewContent = item.NewContent;
                    dyObject.NewDate = item.Date;
                    dyObject.NewPicture = item.NewPicture;
                    dyObject.NewHeat = item.NewHeat;
                    ReCommList.Add(dyObject);
                }
            }
            return ReCommList;
        }

        public void changeUser(string name, string password, string email) {
            using DBContext context = new DBContext();
            User u = context.User.Where(x => x.UserName == name).FirstOrDefault();
            if (u != null)
            {
                u.UserName = name;
                u.Password = password;
                u.Email = email;
            }
            context.SaveChanges();
        }

        public ActionResult Feedback(string feedback) {
            using DBContext context = new DBContext();
            context.Add(new Feedback
            {
                FeedbackContent = feedback,
            });
            context.SaveChanges();
            return Json(new { d = "评论成功" });
        }

    }
}