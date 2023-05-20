using Microsoft.AspNetCore.Mvc;
using EntityFrameworkDemo.DB;
using NewsWebApplication.Models;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

namespace NewsWebApplication.Controllers
{
    public class NewsPageController : Controller
    {
        public IActionResult Index(/*List<dynamic> New, */string Column, int Id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("UserName");
            List<dynamic> Newsdata = GetData(Column, Id);
            ViewBag.dyObject = Newsdata;
            List<dynamic> CommentsList = GetComments(Id);
            ViewBag.dyComments = CommentsList;
            ViewData["Column"] = Column;
            ViewData["NewId"] = Id;
            List<dynamic> HeatsList = HeatList();
            ViewBag.HeatNews = HeatsList;
            List<dynamic> TopicsList = TopicList(Column);
            ViewBag.TopicNews = TopicsList;
            using DBContext context = new DBContext();
            var userid = from User in context.User
                         where User.UserName == HttpContext.Session.GetString("UserName")
                         select new
                         {
                             User.Id
                         };
            foreach (var i in userid)
            {
                ViewData["UserId"] = i.Id;
            }
            AddHeat(Id);
            return View();
        }

        //获取该新闻的内容
        public List<dynamic> GetData(string Column,int Id) {
            using DBContext context = new DBContext();
            var News = (from New in context.New
                                 where New.Id == Id
                                 select new
                                 {
                                     New.NewContent,
                                     New.NewTitle,
                                     New.Date,
                                     New.NewPicture
                                 }).ToList();
            List<dynamic> Newsdata = new List<dynamic>();
            foreach (var item in News)
            {
                dynamic dyObject = new ExpandoObject();
                dyObject.NewTitle = item.NewTitle;
                dyObject.NewContent = item.NewContent;
                dyObject.NewDate = item.Date;
                dyObject.NewPicture = item.NewPicture;
                Newsdata.Add(dyObject);
            }
            return Newsdata;
        }

        public ActionResult PushComments(int NewId, int UserId, string Comment)
        {
            using DBContext context = new DBContext();
            context.Add(new Comment
            {
                NewId = NewId,
                UserId = UserId,
                CommentDetail = Comment
            });
            context.SaveChanges();
            return Json(new { d = "评论成功" });
        }
        
        public List<dynamic> GetComments(int Id) {
            using DBContext context = new DBContext();
            var Comments = (from Comment in context.Comment
                            join New in context.New
                            on Comment.NewId equals New.Id
                            join User in context.User
                            on Comment.UserId equals User.Id
                            where New.Id == Id
                            select new
                            {
                                User.UserName,
                                Comment.CommentDetail,
                            }).ToList();
            List<dynamic> CommentsList = new List<dynamic>();
            foreach (var data in Comments.ToList()) {
                dynamic dyObject = new ExpandoObject();
                dyObject.User = data.UserName;
                dyObject.Comment = data.CommentDetail;
                CommentsList.Add(dyObject);
            }
            return CommentsList;
        }

        //增加热度
        public void AddHeat(int NewId) {
            using DBContext context = new DBContext();
            New Heat = context.New.Where(x => x.Id == NewId).FirstOrDefault();
            if (Heat != null)
            {
                Heat.NewHeat++;
            }

            //推荐
            var name = HttpContext.Session.GetString("UserName");
            var UserId = 0;
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
            if (name != null) { 
                Recommend Re = context.Recommend.Where(x => x.NewId == NewId && x.UserId == UserId).FirstOrDefault();
                if (Re != null)
                {
                    Re.Recommendation++;
                }
            }
            context.SaveChanges();
        }
        //public ActionResult GetData(int id)//用于接受前台传过来的值
        //{
        //    using DBContext context= new DBContext();
        //    var data = (from New in context.New
        //                where New.Id == id
        //                select new
        //                {
        //                    New.NewContent,
        //                    New.NewTitle,
        //                    New.Date
        //                }).ToList();
        //    List<dynamic> Newsdata = new List<dynamic>();
        //    foreach (var item in data)
        //    {
        //        dynamic dyObject = new ExpandoObject();
        //        dyObject.Title = item.NewTitle;
        //        dyObject.Date = item.Date;
        //        dyObject.Content = item.NewContent;
        //        Newsdata.Add(dyObject);
        //    }
        //    //return Json(new { d = data });
        //    return RedirectToAction("Index", new { New = Newsdata });
        //}


        public List<dynamic> HeatList()
        {
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

        public List<dynamic> TopicList(string Column) {
            using DBContext context = new DBContext();
            var data = (from New in context.New
                        orderby New.NewHeat descending
                        where New.NewColumn == Column
                        select new
                        {
                            New.Id,
                            New.NewColumn,
                            New.NewTitle
                        }).ToList();
            List<dynamic> TopicNews = new List<dynamic>();
            foreach (var item in data)
            {
                dynamic dyObject = new ExpandoObject();
                dyObject.Id = item.Id;
                dyObject.Title = item.NewTitle;
                dyObject.Column = item.NewColumn;
                TopicNews.Add(dyObject);
            }
            return TopicNews;
        }

        public void Like(int NewId, int UserId) {
            using DBContext context = new DBContext();
            Recommend R = context.Recommend.Where(x => x.UserId == UserId && x.NewId == NewId).FirstOrDefault();
            if (R != null)
            {
                R.Recommendation += 2;
            }
            context.SaveChanges();
        }
        public void DisLike(int NewId, int UserId)
        {
            using DBContext context = new DBContext();
            Recommend R = context.Recommend.Where(x => x.UserId == UserId && x.NewId == NewId).FirstOrDefault();
            if (R != null)
            {
                R.Recommendation -= 2;
            }
            context.SaveChanges();
        }
    }
}
