using EntityFrameworkDemo.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using NewsWebApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using NewsWebApplication.Controllers;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
// Add Sessions to the container
builder.Services.AddSession();

builder.Services.AddHostedService<SynchronizationService>();
builder.Services.AddSingleton<SynchronizationService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseMiddleware<LoginCheckr>();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


public class SynchronizationService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly ILogger<SynchronizationService> _logger;
    private ManualResetEvent _dataReadyEvent;
    private int _userId;

    public SynchronizationService(ILogger<SynchronizationService> logger)
    {
        _logger = logger;
        _dataReadyEvent = new ManualResetEvent(false);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoSynchronization, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        return Task.CompletedTask;
    }

    private void DoSynchronization(object state)
    {
        string globalData = LoginController.GlobalData;
        _logger.LogInformation("Start Recommend...for" + globalData);
        if (globalData != null)
        {
            List<string> ReList = RecommendationList(int.Parse(globalData));
            _logger.LogInformation("Start success...");
        }
        else {
            _logger.LogInformation("have not login...");
        }
        


        //globalValue = ReList;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
    public void GetData()
    {
        // Your logic to fetch or process data goes here
        _logger.LogInformation("Fetching data...");
    }
    public List<string> RecommendationList(int UserId)
    {
        //string Path = @"D:\Desktop\alg\dist\Reg.exe";
        string Path = @"./wwwroot/alg/dist/Reg.exe";
        string Li = "";
        Process p = new Process();
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.FileName = Path;
        p.StartInfo.UseShellExecute = false;
        String m = UserId.ToString();
        p.StartInfo.Arguments = m;
        p.OutputDataReceived += (sender, argsx) =>
        {
            Li += argsx.Data;
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
}
//public class SynchronizationService : IHostedService, IDisposable
//{
//    private readonly ILogger<SynchronizationService> _logger;
//    private List<string> globalValue;

//    public SynchronizationService(ILogger<SynchronizationService> logger)
//    {
//        _logger = logger;
//        globalValue = new List<string>();
//    }

//    public Task StartAsync(CancellationToken cancellationToken)
//    {
//        return Task.CompletedTask;
//    }

//    public async Task DoSynchronization()
//    {
//        _logger.LogInformation("Start Recommend...");
//        List<string> reList = await RecommendationListAsync(6);
//        globalValue = reList;

//        // Your synchronization logic goes here
//    }

//    public Task StopAsync(CancellationToken cancellationToken)
//    {
//        return Task.CompletedTask;
//    }

//    public void Dispose()
//    {
//    }

//    public async Task<List<string>> GetGlobalValueAsync()
//    {
//        return new List<string>(globalValue);
//    }

//    public async Task<List<string>> RecommendationListAsync(int UserId)
//    {
//        string Path = @"D:\Desktop\alg\dist\Reg.exe";
//        string Li = "";
//        Process p = new Process();
//        p.StartInfo.RedirectStandardOutput = true;
//        //p.StartInfo.CreateNoWindow = true;
//        p.StartInfo.FileName = Path;
//        p.StartInfo.UseShellExecute = false;
//        String m = UserId.ToString();
//        p.StartInfo.Arguments = m;
//        p.OutputDataReceived += (sender, argsx) =>
//        {
//            Li += argsx.Data;
//            //Console.WriteLine(argsx.Data);
//        };
//        p.Start();
//        p.BeginOutputReadLine();
//        while (Li.Length == 0)
//        {
//            p.WaitForExit(1000);
//        }
//        p.Close();
//        //Console.WriteLine(result.Substring(1,2));
//        //Process pro = Process.Start(Path, "4");
//        //pro.WaitForExit();
//        //int Re = pro.ExitCode;
//        //pro.Close();
//        Li = Li.Substring(1, Li.Length - 2);
//        var list = Li.Split(',').ToList();
//        return list;
//    }
//}
