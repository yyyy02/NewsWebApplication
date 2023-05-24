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
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            //get the current user
            ViewData["Username"] = HttpContext.Session.GetString("UserName");
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

            //HttpClient http = new HttpClient();
            //string url = "http://192.168.1.102:5000/";
            //Task<string> task = http.GetStringAsync(url);
            //Console.WriteLine(task.Result);

            List<dynamic> heat = HeatList();
            ViewBag.RecommNews = null;
            if (HttpContext.Session.GetString("UserName") != null)
            {
                //List<string> ReList = RecommendationList();
                List<string> ReList = new List<string> { "1", "2", "4", "5", "6", "7", "8", "9", "10" };
                List<dynamic> ReCommList = new List<dynamic>();
                ReCommList = GetNews(ReList);
                ViewBag.RecommNews = ReCommList;
            }
            else {
                Random random = new Random();
                List<string> ReList = new List<string>();
                while(ReList.Count <= 40) { 
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
            List<string> PicList = new List<string> { "11", "12", "13", "14", "15", "16", "17","18" };
            List<dynamic> PicList1 = GetNews(PicList);
            ViewBag.PicNews = PicList1;
            ViewBag.HeatNews = heat;
            return View();
        }

        public IActionResult Channel(string column)
        {
            ViewBag.column = column;
            ViewData["Username"] = HttpContext.Session.GetString("UserName");
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

        public List<string> RecommendationList() {
            string Path = @"D:\Desktop\alg\dist\Reg.exe";
            string Li = "";
            Process p = new Process();
            p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = Path;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = @"5";
            p.OutputDataReceived += (sender, argsx) =>
            {
                Li += argsx.Data;
                //Console.WriteLine(argsx.Data);
            };
            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit(20000);
            p.Close();
            //Console.WriteLine(result.Substring(1,2));
            //Process pro = Process.Start(Path, "4");
            //pro.WaitForExit();
            //int Re = pro.ExitCode;
            //pro.Close();
            Li = Li.Substring(1, Li.Length-2);
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
    }
}