using Microsoft.AspNetCore.Mvc;
using NewsWebApplication.Models;
using System.Diagnostics;

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
            return View();
        }

        public IActionResult Channel(string column)
        {
            ViewBag.column = column;
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}