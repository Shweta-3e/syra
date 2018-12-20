using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Syra.Admin.DbContexts;
using Syra.Admin.Entities;
using Syra.Admin.Helper;
using Syra.Admin.Models;
using Syra.Admin.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static Syra.Admin.Entities.SessionLog;


namespace Syra.Admin.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private Response response = new Response();
        private SyraDbContext db = new SyraDbContext();

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyBots()
        {
            var useremail = HttpContext.User.Identity.Name;

            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;
            var db = new SyraDbContext();
            ViewBag.Bots = new List<BotDeployment>();

            var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);
            if (customer != null)
            {
                var userbots = db.BotDeployments.Where(c => c.CustomerId == customer.Id).ToList();
                ViewBag.Bots = userbots;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View(ViewBag.Bots);
        }

        // GET: Plans/Create
        public ActionResult CreateBot()
        {
            SyraDbContext db = new SyraDbContext();

            //get availavle luis domain
            ViewBag.LuisDomains = db.LuisDomains.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBot([Bind(Include = "LuisId,Id,Name,CustomerId,CompanyName,FacebookPage,Website,ContactPage,ContactNo,WelcomeMessage,BackGroundColor,ChatBotGoal")] BotDeployment botdeployment)
        {
            SyraDbContext db = new SyraDbContext();
            ViewBag.LuisDomains = db.LuisDomains.ToList();
            //Get current Logged in user by his email 
            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var useremail = HttpContext.User.Identity.Name;
            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;


            if (ModelState.IsValid)
            {
                //based on aspnetuser object, get customer details
                var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);

                //Check custmer plan
                var planobj = db.CustomerPlans.FirstOrDefault(c => c.CustomerId == customer.Id);
                var plancheck = db.CustomerPlans.FirstOrDefault(c => c.PlanId == planobj.PlanId && c.IsActive);
                if (plancheck != null)
                {
                    int count = db.BotDeployments.Where(c => c.CustomerId == customer.Id).Count();
                    //customer has active plan
                    if (count >= plancheck.Plan.AllowedBotLimit)
                    {
                        ModelState.AddModelError("", "Plan allowed limit exceeds! Please upgrade your plan");
                        //all bots used, so no remaining new bot slot available. Upgrade is required
                    }
                    else
                    {
                        //Customer can add new bot in the account
                        BotDeployment bot = new BotDeployment();
                        bot.Name = botdeployment.Name;
                        bot.CompanyName = botdeployment.CompanyName;
                        bot.FacebookPage = botdeployment.FacebookPage;
                        bot.Website = botdeployment.Website;
                        bot.ContactPage = botdeployment.ContactPage;
                        bot.ContactNo = botdeployment.ContactNo;
                        bot.WelcomeMessage = botdeployment.WelcomeMessage;
                        bot.BackGroundColor = botdeployment.BackGroundColor;
                        bot.WebSiteURI = botdeployment.WebSiteURI;
                        bot.DomainName = botdeployment.DomainName;
                        bot.DeploymentDate = DateTime.Now;
                        bot.DeleteDate = DateTime.Now;
                        bot.LuisId = botdeployment.LuisId;

                        customer.BotDeployments.Add(bot);
                        db.SaveChanges();
                        return RedirectToAction("MyBots");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please Active your plan");

                    //Customer is not having any plan active, so message on screen accordingly
                }
            }
            return View(botdeployment);
        }

        public string GetIPDetails(string ipaddress)
        {
            string ipadd = ipaddress.Replace(" ", "");
            var Uri = "https://extreme-ip-lookup.com/json/" + ipadd;
            HttpWebRequest request = WebRequest.Create(Uri) as System.Net.HttpWebRequest;
            Encoding encoding = new UTF8Encoding();
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            String jsonresponse = "";
            String temp = null;
            while (!reader.EndOfStream)
            {
                temp = reader.ReadLine();
                jsonresponse += temp;
            }
            return jsonresponse;
        }

        public void EpochTime(string json)
        {
            string oldcontent = "";
            var file = System.Web.HttpContext.Current.Request.MapPath("..\\AppScript\\Analytics\\Template\\Epochtime.json");
            using (StreamReader reader = new StreamReader(file))
            {
                oldcontent = reader.ReadToEnd();
                if (oldcontent != null)
                {
                    oldcontent = null;
                }
            }
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.Write(json.ToString());
                writer.Close();
            }
        }

        //[HttpPost]
        //public string LowPeakTime(DateTime startdt, DateTime enddt)
        //{
        //    SyraDbContext db = new SyraDbContext();
        //    List<ArrayList> arraylist = new List<ArrayList>();
        //    List<LowHighTime> dataline = new List<LowHighTime>();
        //    ArrayList arrayList = new ArrayList();
        //    List<Location> _data = new List<Location>();
        //    _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        //    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    var useremail = HttpContext.User.Identity.Name;
        //    var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;

        //    if (aspnetuser != null)
        //    {
        //        var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);
        //        var botdata = db.BotDeployments.Where(c => c.CustomerId == customer.Id).FirstOrDefault();
        //        var connstring = botdata.BlobConnectionString;
        //        var blobstorage = botdata.BlobStorageName;
        //        var containername = botdata.ContainerName;

        //        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connstring);
        //        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        //        CloudBlobContainer container = blobClient.GetContainerReference(containername);
        //        var count = container.ListBlobs().Count();
        //        ArrayList array = new ArrayList();

        //        bool blob_check = false;
        //        try
        //        {
        //            for (DateTime date = startdt; date <= enddt; date = date.AddDays(1))
        //            {
        //                var startdateonly = date.Date.ToString("dd-MM-yyyy");
        //                var dateonly = date.Date.ToString("dd/MM/yyyy");
        //                var blob_file_name = startdateonly + "" + ".csv";
        //                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
        //                blob_check = blockBlob.Exists();
        //                if (blob_check == false)
        //                {
        //                    string timedate = startdateonly.ToString().Replace("-", "/").Replace(" ", "");
        //                    CultureInfo culture = new CultureInfo("en-US");
        //                    DateTime dateobj = DateTime.ParseExact(timedate, "dd/MM/yyyy", culture);
        //                    int year = dateobj.Year;
        //                    int month = dateobj.Month;
        //                    int day = dateobj.Day;
        //                    int hour = dateobj.Hour;
        //                    int minute = dateobj.Minute;
        //                    int second = dateobj.Second;
        //                    var dateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        //                    var dateTimeOffset = new DateTimeOffset(dateTime);
        //                    var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
        //                    Int64 epochtime = Convert.ToInt64(unixDateTime) * 1000;
        //                    dataline.Add(new LowHighTime { epochtime = epochtime, status = "no data"});
        //                }
        //                else
        //                {
        //                    using (StreamReader reader = new StreamReader(blockBlob.OpenRead()))
        //                    {
        //                        SessionLog log = new SessionLog();
        //                        CultureInfo culture = new CultureInfo("en-US");
        //                        List<Int64> logtime = new List<Int64>();
        //                        ArrayList time_frequency = new ArrayList();
        //                        //Object data = new Object();
        //                    //string dateonly = startdateonly.Date.ToString("dd/MM/yyyy");
        //                    //obtain epoch time from blob name
        //                        DateTime dateobj = DateTime.ParseExact(dateonly,"dd/MM/yyyy",culture);
        //                        int year = dateobj.Year;
        //                        int month = dateobj.Month;
        //                        int day = dateobj.Day;
        //                        int hour = dateobj.Hour;
        //                        int minute = dateobj.Minute;
        //                        int second = dateobj.Second;
        //                        var dateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        //                        var dateTimeOffset = new DateTimeOffset(dateTime);
        //                        var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
        //                        Int64 epochtime = Convert.ToInt64(unixDateTime) * 1000;
        //                        while (!reader.EndOfStream)
        //                        {
        //                            var line = reader.ReadLine();
        //                            string[] splitedword = line.Split('|');
        //                            log.LogDate = splitedword[5];
        //                            log.LogDate = log.LogDate.Replace("-", "/").Replace(" ", "");
        //                            log.LogTime = splitedword[6].Replace(" ", "");
        //                            //CultureInfo culture = new CultureInfo("en-US");
        //                            //string timedate = log.LogDate;
        //                            //DateTime dateobj = DateTime.ParseExact(timedate, "dd/MM/yyyy", culture);
        //                            //int year = dateobj.Year;
        //                            //int month = dateobj.Month;
        //                            //int day = dateobj.Day;
        //                            //int hour = dateobj.Hour;
        //                            //int minute = dateobj.Minute;
        //                            //int second = dateobj.Second;
        //                            //var dateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        //                            //var dateTimeOffset = new DateTimeOffset(dateTime);
        //                            //var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
        //                            //Int64 epochtime = Convert.ToInt64(unixDateTime) * 1000;
        //                            //Convert.ToDateTime(log.LogTime).ToShortTimeString()
        //                            logtime.Add((DateTime.ParseExact(log.LogTime,"HH:mm:ss",culture).Hour));
        //                        }
        //                        var frequency = logtime.GroupBy(c => c);
        //                        foreach(var item in frequency)
        //                        {
        //                            time_frequency.Add(new CountHour {hour=item.Key, hourcount=item.Count() });
        //                        }
        //                        dataline.Add(new LowHighTime { epochtime = epochtime, status = "data",dateTimes=time_frequency.ToArray() });
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            string errmsg = e.StackTrace;
        //            response.Message = errmsg;
        //        }

        //        //var dupepochtime = dataline.GroupBy(x => new { x.epochtime, x.status }).Select(group => new { Name = group.Key, Count = group.Count() });
        //        //foreach (var item in dupepochtime)
        //        //{
        //        //    if (item.Name.status == "no data")
        //        //    {

        //        //        arraylist.Add(new ArrayList { item.Name.epochtime, 0 });
        //        //    }
        //        //    else
        //        //    {
        //        //        arraylist.Add(new ArrayList { item.Name.epochtime, item.Count });
        //        //    }
        //        //}
        //        //string json = JsonConvert.SerializeObject(arraylist);
        //        //EpochTime(json);
        //        response.Data = new
        //        {
        //            Epochtime = dataline
        //        };
        //        response.IsSuccess = true;
        //    }
        //    return response.GetResponse();
        //}

        [HttpPost]
        public string LowPeakTime(DateTime startdt, DateTime enddt)
        {
            SyraDbContext db = new SyraDbContext();
            List<ArrayList> arraylist = new List<ArrayList>();
            List<LowHighTime> dataline = new List<LowHighTime>();
            ArrayList arrayList = new ArrayList();
            List<Location> _data = new List<Location>();
            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var useremail = HttpContext.User.Identity.Name;
            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;

            if (aspnetuser != null)
            {
                var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);
                var botdata = db.BotDeployments.Where(c => c.CustomerId == customer.Id).FirstOrDefault();
                var connstring = botdata.BlobConnectionString;
                var blobstorage = botdata.BlobStorageName;
                var containername = botdata.ContainerName;

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connstring);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containername);
                var count = container.ListBlobs().Count();
                ArrayList array = new ArrayList();

                bool blob_check = false;
                try
                {
                    for (DateTime date = startdt; date <= enddt; date = date.AddDays(1))
                    {
                        var startdateonly = date.Date.ToString("dd-MM-yyyy");
                        var blob_file_name = startdateonly + "" + ".csv";
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
                        blob_check = blockBlob.Exists();
                        if (blob_check == false)
                        {
                            string timedate = startdateonly.ToString().Replace("-", "/").Replace(" ", "");
                            CultureInfo culture = new CultureInfo("en-US");
                            DateTime dateobj = DateTime.ParseExact(timedate, "dd/MM/yyyy", culture);
                            int year = dateobj.Year;
                            int month = dateobj.Month;
                            int day = dateobj.Day;
                            int hour = dateobj.Hour;
                            int minute = dateobj.Minute;
                            int second = dateobj.Second;
                            var dateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
                            var dateTimeOffset = new DateTimeOffset(dateTime);
                            var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                            Int64 epochtime = Convert.ToInt64(unixDateTime) * 1000;
                            dataline.Add(new LowHighTime { epochtime = epochtime, status = "no data" });
                        }
                        else
                        {
                            using (StreamReader reader = new StreamReader(blockBlob.OpenRead()))
                            {
                                SessionLog log = new SessionLog();
                                while (!reader.EndOfStream)
                                {
                                    var line = reader.ReadLine();
                                    string[] splitedword = line.Split('|');
                                    log.LogDate = splitedword[5];
                                    log.LogDate = log.LogDate.Replace("-", "/").Replace(" ", "");
                                    log.LogTime = splitedword[6].Replace(" ", "");
                                    CultureInfo culture = new CultureInfo("en-US");
                                    string timedate = log.LogDate;
                                    DateTime dateobj = DateTime.ParseExact(timedate, "dd/MM/yyyy", culture);
                                    int year = dateobj.Year;
                                    int month = dateobj.Month;
                                    int day = dateobj.Day;
                                    int hour = dateobj.Hour;
                                    int minute = dateobj.Minute;
                                    int second = dateobj.Second;
                                    var dateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
                                    var dateTimeOffset = new DateTimeOffset(dateTime);
                                    var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                                    Int64 epochtime = Convert.ToInt64(unixDateTime) * 1000;
                                    dataline.Add(new LowHighTime { epochtime = epochtime, status = "data" });
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    string errmsg = e.Message;
                    response.Message = errmsg;
                }
                var dupepochtime = dataline.GroupBy(x => new { x.epochtime, x.status, x.timecount }).Select(group => new { Name = group.Key, Count = group.Count() });
                foreach (var item in dupepochtime)
                {
                    if (item.Name.status == "no data")
                    {

                        arraylist.Add(new ArrayList { item.Name.epochtime, 0 });
                    }
                    else
                    {
                        arraylist.Add(new ArrayList { item.Name.epochtime, item.Count });
                    }
                }
                string json = JsonConvert.SerializeObject(arraylist);
                //EpochTime(json);
                response.Data = new
                {
                    Epochtime = arraylist
                };
                response.IsSuccess = true;
            }
            return response.GetResponse();
        }


        [HttpPost]
        public string BotReply(DateTime startdt, DateTime enddt)
        {
            SyraDbContext db = new SyraDbContext();
            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var useremail = HttpContext.User.Identity.Name;
            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;

            //to hold asked questions and its responses 
            var catagories = new List<string>();
            var logs = new List<SessionLog>();


            if (aspnetuser != null)
            {
                //based on aspnetuser object, get customer details
                var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);
                var botdata = db.BotDeployments.Where(c => c.CustomerId == customer.Id).FirstOrDefault();
                var connstring = botdata.BlobConnectionString;
                var blobstorage = botdata.BlobStorageName;
                var containername = botdata.ContainerName;
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connstring);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containername);


                bool blob_check = false;
                for (DateTime date = startdt; date <= enddt; date = date.AddDays(1))
                {
                    var startdateonly = date.Date.ToString("dd-MM-yyyy");
                    var blob_file_name = startdateonly + "" + ".csv";
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
                    blob_check = blockBlob.Exists();
                    if (blob_check == false)
                    {
                        //No logs for that date, hence do nothing
                        Console.WriteLine("Blob Container doesn't exist");
                    }
                    else
                    {
                        // we have log file, hence going to read it and send back to client
                        using (StreamReader reader = new StreamReader(blockBlob.OpenRead()))
                        {
                            try
                            {
                                while (!reader.EndOfStream)
                                {
                                    //string oldcontent;
                                    SessionLog log = new SessionLog();
                                    var line = reader.ReadLine();
                                    string[] splitedword = line.Split('|');
                                    log.SessionId = splitedword[0];
                                    log.UserQuery = splitedword[3];
                                    log.BotAnswers = splitedword[4];
                                    log.IPAddress = splitedword[1];
                                    log.Region = splitedword[2];
                                    log.LogDate = splitedword[5];
                                    if (log.BotAnswers.Contains("Hmmm...I didn’t quite get that"))
                                    {
                                        log.BotResponse = "Failed to Response/understand user question";
                                        log.IsWrongAnswer = true;
                                    }
                                    logs.Add(log);
                                }
                            }
                            catch (Exception e)
                            {
                                string errmsg = e.Message;
                                response.Message = errmsg;
                            }
                        }
                    }
                }

                //all distinct questions being asked in bot
                catagories = logs.Select(c => c.UserQuery).Distinct().ToList();

                var result = (from c in logs
                              group c by new { c.UserQuery, c.IsWrongAnswer } into g
                              select new
                              {
                                  g.Key.UserQuery,
                                  g.Key.IsWrongAnswer,
                                  Total = g.Count(),
                              });
                var totalQuestions = result.Sum(c => c.Total);
                var rightAnswers = result.Where(c => c.IsWrongAnswer == false).Sum(c => c.Total);
                var wrongAnswers = totalQuestions - rightAnswers;


                response.Data = new
                {
                    catagories,
                    result,
                    TotalQuestions = totalQuestions,
                    RightAnswers = rightAnswers,
                    WrongAnswers = wrongAnswers,
                    AllQuestions = logs,
                };

                if (result.Count() <= 0)
                    response.Data = false;

            }
            return response.GetResponse();
        }

        [HttpPost]
        public string UserQuery(DateTime startdt, DateTime enddt)
        {
            List<ArrayList> arraylist = new List<ArrayList>();
            List<ArrayList> alldata = new List<ArrayList>();
            List<ArrayList> anslist = new List<ArrayList>();
            List<SessionLog> logs = new List<SessionLog>();
            List<Longtitude> countries = new List<Longtitude>();
            List<USARegion> region = new List<USARegion>();
            List<Location> _data = new List<Location>();
            List<USALocation> _location = new List<USALocation>();
            SyraDbContext db = new SyraDbContext();
            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var useremail = HttpContext.User.Identity.Name;
            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;
            if (aspnetuser != null)
            {
                //based on aspnetuser object, get customer details
                var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);
                var botdata = db.BotDeployments.Where(c => c.CustomerId == customer.Id).FirstOrDefault();
                var connstring = botdata.BlobConnectionString;
                var blobstorage = botdata.BlobStorageName;
                var containername = botdata.ContainerName;

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connstring);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containername);
                bool blob_check = false;
                for (DateTime date = startdt; date <= enddt; date = date.AddDays(1))
                {
                    var startdateonly = date.Date.ToString("dd-MM-yyyy");
                    var blob_file_name = startdateonly + "" + ".csv";
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
                    blob_check = blockBlob.Exists();
                    if (blob_check == false)
                    {
                        Console.WriteLine("Blob Container doesn't exist");
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(blockBlob.OpenRead()))
                        {

                            SessionLog log = new SessionLog();
                            try
                            {
                                while (!reader.EndOfStream)
                                {
                                    //string oldcontent;
                                    var line = reader.ReadLine();
                                    string[] splitedword = line.Split('|');
                                    log.SessionId = splitedword[0];
                                    log.IPAddress = splitedword[1];
                                    log.Region = splitedword[2];
                                    if (log.Region.Contains('.'))
                                    {
                                        string temp = log.IPAddress;
                                        log.IPAddress = log.Region;
                                        log.Region = temp;
                                    }
                                    log.UserQuery = splitedword[3];
                                    log.BotAnswers = splitedword[4];
                                    log.LogDate = splitedword[5];
                                    log.LogTime = splitedword[6];
                                    string tempdate = log.LogDate + log.LogTime;
                                    string dt = Convert.ToDateTime(startdt).ToString("dd-MM-yyyy");
                                    logs.Add(new SessionLog { SessionId = splitedword[0], IPAddress = splitedword[1], Region = splitedword[2], UserQuery = splitedword[3], BotAnswers = splitedword[4], LogDate = tempdate });
                                    alldata.Add(new ArrayList { splitedword[0], splitedword[1], splitedword[2], splitedword[3], splitedword[4], tempdate });

                                    countries.Add(new Longtitude { UserQuery = log.UserQuery });
                                }
                            }
                            catch (Exception e)
                            {
                                string errmsg = e.Message;
                                response.Message = errmsg;
                            }
                        }
                        response.IsSuccess = true;
                    }
                }
                var dupUserQuery = countries.GroupBy(x => new { x.UserQuery }).Select(group => new { Name = group.Key, Count = group.Count() })
                              .OrderByDescending(x => x.Count);
                foreach (var y in dupUserQuery)
                {
                    arraylist.Add(new ArrayList { y.Name.UserQuery, y.Count });
                }
                var firstTenArrivals = arraylist.Take(10);
                var allresponse = (from c in logs
                                   group c by new { c.UserQuery, c.LogDate, c.IPAddress, c.SessionId, c.BotAnswers, c.Region } into g
                                   select new
                                   {
                                       g.Key.LogDate,
                                       g.Key.SessionId,
                                       g.Key.IPAddress,
                                       g.Key.Region,
                                       g.Key.UserQuery,
                                       g.Key.BotAnswers
                                   });
                response.Data = new {
                    firstTenArrivals,
                    AllResponse = allresponse
                };

            }
            return response.GetResponse();
        }

        [HttpPost]
        public string GetClickedLink(DateTime startdt, DateTime enddt)
        {
            List<ArrayList> arraylist = new List<ArrayList>();
            List<SessionLog> logs = new List<SessionLog>();
            List<Longtitude> countries = new List<Longtitude>();
            SyraDbContext db = new SyraDbContext();
            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var useremail = HttpContext.User.Identity.Name;
            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;

            if (aspnetuser != null)
            {
                //based on aspnetuser object, get customer details
                var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);
                var botdata = db.BotDeployments.Where(c => c.CustomerId == customer.Id).FirstOrDefault();
                var connstring = botdata.BlobConnectionString;
                var blobstorage = botdata.BlobStorageName;
                var containername = ConfigurationManager.AppSettings["clickedlink_container"].ToString();

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connstring);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containername);
                bool blob_check = false;
                for (DateTime date = startdt; date <= enddt; date = date.AddDays(1))
                {
                    var startdateonly = date.Date.ToString("dd-MM-yyyy");
                    var blob_file_name = startdateonly + "" + ".csv";
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
                    blob_check = blockBlob.Exists();
                    if (blob_check == false)
                    {
                        Console.WriteLine("Blob Container doesn't exist");
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(blockBlob.OpenRead()))
                        {

                            SessionLog log = new SessionLog();
                            try
                            {
                                while (!reader.EndOfStream)
                                {
                                    //string oldcontent;
                                    var line = reader.ReadLine();
                                    string[] splitedword = line.Split('|');
                                    log.SessionId = splitedword[0];
                                    log.Region = splitedword[1];
                                    log.IPAddress = splitedword[2];
                                    log.ClickedLink = splitedword[3];
                                    log.LogDate = splitedword[4];
                                    log.LogTime = splitedword[5];
                                    string tempdate = log.LogDate + log.LogTime;
                                    logs.Add(new SessionLog { SessionId = log.SessionId, Region = log.Region, IPAddress = log.IPAddress, ClickedLink = log.ClickedLink, LogDate = tempdate });
                                    countries.Add(new Longtitude { Links = log.ClickedLink, UserId = log.SessionId });
                                }
                            }
                            catch (Exception e)
                            {
                                string errmsg = e.Message;
                                response.Message = errmsg;
                            }

                        }
                        response.IsSuccess = true;
                        //response.Data = logs;
                    }
                }
                var duplinks = countries.GroupBy(x => new { x.Links }).Select(group => new { Name = group.Key, Count = group.Count() })
                              .OrderByDescending(x => x.Count);
                foreach (var y in duplinks)
                {
                    arraylist.Add(new ArrayList { y.Name.Links, y.Count });
                }
                var firstTenArrivals = arraylist.Take(10);
                response.Data = new
                {
                    firstTenArrivals,
                    AllResponse = logs
                };
            }
            return response.GetResponse();
        }

        public string GetUsaCode(string ipaddr,string region)
        {
            //Search from table
            string usacode = "";
            string cs = ConfigurationManager.ConnectionStrings["SyraDbContext"].ToString();
            string sql = "Select * from dbo.UsaRegion where Name ='" + region + "'";
            SqlConnection connection = new SqlConnection(cs);
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        usacode=dataReader["Code"].ToString();
                    }
                }
                command.Dispose();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Can not open connection ! ");
                usacode = e.Message;
            }
            return usacode;
        }

        [HttpPost]
        public string GetUSAAnalytics(DateTime startdt, DateTime enddt)
        {
            List<USALocation> usadata = new List<USALocation>();
            List<SessionLog> logs = new List<SessionLog>();
            List<Longtitude> countries = new List<Longtitude>();
            List<USARegion> region = new List<USARegion>();
            SyraDbContext db = new SyraDbContext();
            var ipdetails = new GetIPAddress();
            var geolocatoin = new GeoLocation();

            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var useremail = HttpContext.User.Identity.Name;
            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;

            if (aspnetuser != null)
            {
                //based on aspnetuser object, get customer details
                var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);
                var botdata = db.BotDeployments.Where(c => c.CustomerId == customer.Id).FirstOrDefault();
                var connstring = botdata.BlobConnectionString;
                var blobstorage = botdata.BlobStorageName;
                var containername = botdata.ContainerName;

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connstring);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containername);
                bool blob_check = false;
                for (DateTime date = startdt; date <= enddt; date = date.AddDays(1))
                {
                    var startdateonly = date.Date.ToString("dd-MM-yyyy");
                    var blob_file_name = startdateonly + "" + ".csv";
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
                    blob_check = blockBlob.Exists();
                    if (blob_check == false)
                    {
                        Console.WriteLine("Blob Container doesn't exist");
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(blockBlob.OpenRead()))
                        {
                            SessionLog log = new SessionLog();
                            try
                            {
                                while (!reader.EndOfStream)
                                {
                                    //string oldcontent;
                                    var line = reader.ReadLine();
                                    string[] splitedword = line.Split('|');
                                    log.SessionId = splitedword[0];
                                    log.IPAddress = splitedword[2];
                                    log.Region = splitedword[1].TrimStart().TrimEnd();
                                    if (log.Region.Contains('.'))
                                    {
                                        string temp = log.IPAddress;
                                        log.IPAddress = log.Region;
                                        log.Region = temp;
                                    }
                                    log.UserQuery = splitedword[3];
                                    log.BotAnswers = splitedword[4];
                                    log.LogDate = splitedword[5];
                                    log.LogTime = splitedword[6];
                                    string tempdate = log.LogDate + log.LogTime;
                                    string dt = Convert.ToDateTime(startdt).ToString("dd-MM-yyyy");
                                    logs.Add(new SessionLog { SessionId = splitedword[0], IPAddress = splitedword[2], Region = log.Region, UserQuery = splitedword[3], BotAnswers = splitedword[4], LogDate = tempdate });
                                    string usaregion = GetUsaCode(ipdetails.query, log.Region);
                                    region.Add(new USARegion { hckey = usaregion.ToLower() });
                                    countries.Add(new Longtitude { Countries = ipdetails.countryCode, UserQuery = log.UserQuery });
                                }
                            }
                            catch (Exception e)
                            {
                                string errmsg = e.Message;
                                response.Message = errmsg;
                                Console.WriteLine(errmsg);
                            }
                        }
                        response.IsSuccess = true;
                    }
                }
                var dupusaregion = region.GroupBy(x => new { x.hckey }).Select(group => new { Name = group.Key, Count = group.Count() });
                foreach (var item in dupusaregion)
                {
                    usadata.Add(new USALocation()
                    {
                        hckey = item.Name.hckey,
                        value = item.Count
                    });
                }
                response.Data = new
                {
                    usadata,
                    AllResponse = logs
                };
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string GetAnalytics(DateTime startdt, DateTime enddt)
        {

            List<SessionLog> logs = new List<SessionLog>();
            List<Longtitude> countries = new List<Longtitude>();
            List<USARegion> region = new List<USARegion>();
            List<Location> _data = new List<Location>();
            List<USALocation> _location = new List<USALocation>();
            SyraDbContext db = new SyraDbContext();
            var ipdetails = new GetIPAddress();
            var geolocation = new GeoLocation();
            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var useremail = HttpContext.User.Identity.Name;
            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;

            if (aspnetuser != null)
            {
                //based on aspnetuser object, get customer details
                var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);
                var botdata = db.BotDeployments.Where(c => c.CustomerId == customer.Id).FirstOrDefault();
                var connstring = botdata.BlobConnectionString;
                var blobstorage = botdata.BlobStorageName;
                var containername = botdata.ContainerName;

                //read data from blob storage
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connstring);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containername);
                bool blob_check = false;

                //get date based on each date of range
                for (DateTime date = startdt; date <= enddt; date = date.AddDays(1))
                {
                    var startdateonly = date.Date.ToString("dd-MM-yyyy");
                    var blob_file_name = startdateonly + "" + ".csv";
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
                    blob_check = blockBlob.Exists();
                    if (blob_check == false)
                    {
                        Console.WriteLine("Blob Container doesn't exist");
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(blockBlob.OpenRead()))
                        {

                            SessionLog log = new SessionLog();
                            try
                            {
                                while (!reader.EndOfStream)
                                {
                                    var line = reader.ReadLine();
                                    string[] splitedword = line.Split('|');
                                    log.SessionId = splitedword[0];
                                    log.IPAddress = splitedword[2];
                                    log.Region = splitedword[1];
                                    if (log.Region.Contains('.'))
                                    {
                                        string temp = log.IPAddress;
                                        log.IPAddress = log.Region;
                                        log.Region = temp;
                                    }
                                    log.UserQuery = splitedword[3];
                                    log.BotAnswers = splitedword[4];
                                    log.LogDate = splitedword[5];
                                    log.LogTime = splitedword[6];
                                    string tempdate = log.LogDate + log.LogTime;
                                    string dt = Convert.ToDateTime(startdt).ToString("dd-MM-yyyy");

                                    if (log.Region == " Virginia ")
                                    {
                                        string jsondeatil = GetUsaCode(log.IPAddress,log.Region);
                                        geolocation = JsonConvert.DeserializeObject<GeoLocation>(jsondeatil);
                                        countries.Add(new Longtitude { Countries = geolocation.Countrycode });
                                        logs.Add(new SessionLog { SessionId = splitedword[0], IPAddress = splitedword[2], Region = splitedword[1], UserQuery = splitedword[3], BotAnswers = splitedword[4], LogDate = tempdate, Country = geolocation.Country });
                                    }
                                    else
                                    {
                                        string jsonresponse = GetIPDetails(log.IPAddress);
                                        ipdetails = JsonConvert.DeserializeObject<GetIPAddress>(jsonresponse);
                                        countries.Add(new Longtitude { Countries = ipdetails.countryCode });
                                        logs.Add(new SessionLog { SessionId = splitedword[0], IPAddress = splitedword[2], Region = splitedword[1], UserQuery = splitedword[3], BotAnswers = splitedword[4], LogDate = tempdate, Country = ipdetails.country });
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                string errmsg = e.Message;
                                response.Message = errmsg;
                            }
                        }
                        response.IsSuccess = true;
                    }
                }
                //get count of distinct countries
                var dupcountries = countries.GroupBy(x => new { x.Countries }).Select(group => new { Name = group.Key, Count = group.Count() })
                             .OrderByDescending(x => x.Count);

                //convert data for json format of country
                foreach (var x in dupcountries)
                {
                    try
                    {
                        RegionInfo myRI1 = new RegionInfo(x.Name.Countries);
                        if (x.Name.Countries == "" || x.Name.Countries == null)
                        {
                            Console.WriteLine("Error");
                        }
                        else
                        {
                            _data.Add(new Location()
                            {
                                code3 = myRI1.ThreeLetterISORegionName,
                                name = myRI1.EnglishName,
                                value = float.Parse(x.Count.ToString(), CultureInfo.InvariantCulture.NumberFormat),
                                code = myRI1.TwoLetterISORegionName
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        string message = e.Message;
                        response.Message = message;
                    }
                }
                var worldresult = (from c in logs
                                   group c by new { c.Region } into g
                                   select new
                                   {
                                       Region = g.Key.Region,
                                       Total = g.Count(),
                                   });

                response.Data = new {
                    _data,
                    AllResponse = logs,
                    WorldResult = worldresult
                };
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string GetLogs(DateTime startdt, DateTime enddt)
        {
            //try
            //{
            List<SessionLog> logs = new List<SessionLog>();
            List<Longtitude> countries = new List<Longtitude>();
            List<Location> _data = new List<Location>();
            SyraDbContext db = new SyraDbContext();
            var ipdetails = new GetIPAddress();
            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var useremail = HttpContext.User.Identity.Name;
            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;

            if (aspnetuser != null)
            {
                //based on aspnetuser object, get customer details
                var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);
                var botdata = db.BotDeployments.Where(c => c.CustomerId == customer.Id).FirstOrDefault();
                var connstring = botdata.BlobConnectionString;
                //var blobstorage = botdata.BlobStorageName;
                var containername = botdata.ContainerName;

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connstring);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containername);
                bool blob_check = false;
                for (DateTime date = startdt; date <= enddt; date = date.AddDays(1))
                {
                    var startdateonly = date.Date.ToString("dd-MM-yyyy");
                    var blob_file_name = startdateonly + "" + ".csv";
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
                    blob_check = blockBlob.Exists();
                    if (blob_check == false)
                    {
                        Console.WriteLine("Blob Container doesn't exist");
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(blockBlob.OpenRead()))
                        {

                            SessionLog log = new SessionLog();
                            try
                            {
                                while (!reader.EndOfStream)
                                {
                                    //string oldcontent;
                                    var line = reader.ReadLine();
                                    string[] splitedword = line.Split('|');
                                    log.SessionId = splitedword[0];
                                    log.IPAddress = splitedword[1];
                                    log.Region = splitedword[2];
                                    log.UserQuery = splitedword[3];
                                    log.BotAnswers = splitedword[4];
                                    log.LogDate = splitedword[5];
                                    log.LogTime = splitedword[6];
                                    string tempdate = log.LogDate + log.LogTime;
                                    string dt = Convert.ToDateTime(startdt).ToString("dd-MM-yyyy");
                                    logs.Add(new SessionLog { SessionId = splitedword[0], IPAddress = splitedword[1], Region = splitedword[2], UserQuery = splitedword[3], BotAnswers = splitedword[4], LogDate = tempdate });
                                }
                            }
                            catch (Exception e)
                            {
                                string errmsg = e.Message;
                                response.Message = errmsg;
                            }
                        }
                        response.IsSuccess = true;
                        response.Data = logs;

                        //return response.GetResponse();
                    }
                }
            }
            return response.GetResponse();
        }
        #region BotDeployment

        //Get LuisDomains
        [HttpGet]
        public string GetLuisDomains()
        {
            var luisdomains = db.LuisDomains.ToList();
            response.Data = from a in luisdomains
                            select new
                            {
                                a.Name,
                                a.Id
                            };
            return response.GetResponse();
        }

        //Create Bot
        [HttpPost]
        public string CreateNewChatBot(BotDeployment botdeployment)
        {
            SyraDbContext db = new SyraDbContext();
            ViewBag.LuisDomains = db.LuisDomains.ToList();
            //Get current Logged in user by his email 
            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var useremail = HttpContext.User.Identity.Name;
            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;

            if (ModelState.IsValid)
            {
                //based on aspnetuser object, get customer details
                var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);

                //Check custmer plan
                var planobj = db.CustomerPlans.FirstOrDefault(c => c.CustomerId == customer.Id);
                var plancheck = db.CustomerPlans.FirstOrDefault(c => c.PlanId == planobj.PlanId && c.IsActive);
                if (plancheck != null)
                {
                    int count = db.BotDeployments.Where(c => c.CustomerId == customer.Id).Count();
                    //customer has active plan
                    if (count >= plancheck.Plan.AllowedBotLimit)
                    {
                        response.IsSuccess = false;
                        response.Message = "Plan allowed limit exceeds! Please upgrade your plan";
                        return response.GetResponse();
                    }
                    else
                    {
                        //Customer can add new bot in the account
                        BotDeployment bot = new BotDeployment();
                        bot.Name = botdeployment.Name;
                        bot.CompanyName = botdeployment.CompanyName;
                        bot.FacebookPage = botdeployment.FacebookPage;
                        bot.Website = botdeployment.Website;
                        bot.ContactPage = botdeployment.ContactPage;
                        bot.ContactNo = botdeployment.ContactNo;
                        bot.WelcomeMessage = botdeployment.WelcomeMessage;
                        bot.BackGroundColor = botdeployment.BackGroundColor;
                        bot.WebSiteURI = botdeployment.WebSiteURI;
                        bot.DomainName = botdeployment.DomainName;
                        bot.DeploymentDate = DateTime.Now;
                        bot.DeleteDate = DateTime.Now;
                        bot.LuisId = botdeployment.LuisId;
                        bot.ChatBotGoal = botdeployment.ChatBotGoal;
                        bot.BotQuestionAnswers = new List<BotQuestionAnswers>();
                        bot.T_BotClientId = Guid.NewGuid().ToString();
                        bot.FirstMessage = botdeployment.FirstMessage;
                        bot.SecondMessage = botdeployment.SecondMessage;
                        if (botdeployment.BotQuestionAnswers != null)
                        {
                            if (botdeployment.BotQuestionAnswers.Any())
                            {
                                foreach (var ans in botdeployment.BotQuestionAnswers)
                                {
                                    BotQuestionAnswers item = new BotQuestionAnswers();
                                    item.Question = ans.Question;
                                    item.Answer = ans.Answer;
                                    bot.BotQuestionAnswers.Add(item);
                                }
                            }
                        }
                        bot.EmbeddedScript = "<script src= \"https://syra.ai/chatbots/assets/chatbots.js\" " + "clientID=\"" + bot.T_BotClientId + "\"></script>";
                        customer.BotDeployments.Add(bot);
                        db.SaveChanges();
                        response.IsSuccess = true;
                        response.Message = "Chat bot created successfully";
                        return response.GetResponse();
                        //return RedirectToAction("MyBots");
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Please Active your plan";
                    return response.GetResponse();
                    //return RedirectToAction("MyBots");
                    //ModelState.AddModelError("", "Please Active your plan");

                    //Customer is not having any plan active, so message on screen accordingly
                }
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string GetMyBots(int pagesize = 10, int pageno = 0)
        {
            try
            {
                var useremail = HttpContext.User.Identity.Name;

                _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;

                PaginatedResponse result = new PaginatedResponse();

                var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);
                if (customer != null)
                {
                    var userbots = db.BotDeployments.Where(c => c.CustomerId == customer.Id).ToList();

                    result.Count = userbots.Count();
                    result.TotalPages = (int)(result.Count / pagesize) + ((result.Count % pagesize) > 0.0M ? 1 : 0);
                    var skip = pageno * pagesize;
                    result.HasNext = (skip > 0 || pagesize > 0) && (skip + pagesize < result.Count);
                    result.HasPrevious = skip > 0;

                    result.IsSuccess = true;
                    result.Entities = Mapper.Map<BotDeploymentView[]>(userbots.Skip(skip).Take(pagesize));
                    return result.GetResponse();
                }
                else
                {
                    response.IsSuccess = false;
                    //return RedirectToAction("Login", "Account");
                }

                return response.GetResponse();
            } catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response.GetResponse();
            }
            //return View(ViewBag.Bots);
        }

        [HttpPost]
        public string GetChatBotEntry(Int64 Id)
        {
            var chatbot = db.BotDeployments.Find(Id);
            var botquestionanswers = db.BotQuestionAnswers.Where(c => c.BotDeploymentId == chatbot.Id).ToList();
            chatbot.BotQuestionAnswers = botquestionanswers;
            response.Data = Mapper.Map<BotDeploymentView>(chatbot);
            return response.GetResponse();
        }

        [HttpPost]
        public string UpdateChatBot(BotDeploymentView botdeploymentview)
        {
            try
            {
                var chatbot = db.BotDeployments.Find(botdeploymentview.Id);
                var botquestionanswer = db.BotQuestionAnswers.Where(c => c.BotDeploymentId == chatbot.Id).ToList();
                chatbot.BotQuestionAnswers = botquestionanswer;

                if (chatbot.BotQuestionAnswers.Any())
                {
                    var botquestionanswers = db.BotQuestionAnswers.Where(c => c.BotDeploymentId == chatbot.Id).ToList();
                    db.BotQuestionAnswers.RemoveRange(botquestionanswers);
                }

                if (chatbot != null)
                {
                    chatbot.Name = botdeploymentview.Name;
                    chatbot.CompanyName = botdeploymentview.CompanyName;
                    chatbot.FacebookPage = botdeploymentview.FacebookPage;
                    chatbot.Website = botdeploymentview.Website;
                    chatbot.ContactPage = botdeploymentview.ContactPage;
                    chatbot.ContactNo = botdeploymentview.ContactNo;
                    chatbot.WelcomeMessage = botdeploymentview.WelcomeMessage;
                    if(botdeploymentview.BackGroundColor.Contains('#')==false)
                    {
                        chatbot.BackGroundColor= botdeploymentview.BackGroundColor.Insert(0, "#");
                    }
                    else
                    {
                        chatbot.BackGroundColor = botdeploymentview.BackGroundColor;
                    }
                    chatbot.WebSiteURI = botdeploymentview.WebSiteURI;
                    chatbot.DomainName = botdeploymentview.DomainName;
                    chatbot.DeploymentDate = botdeploymentview.DeploymentDate;
                    chatbot.DeleteDate = botdeploymentview.DeleteDate;
                    chatbot.LuisId = botdeploymentview.LuisId;
                    chatbot.ChatBotGoal = botdeploymentview.ChatBotGoal;
                    chatbot.FirstMessage = botdeploymentview.FirstMessage;
                    chatbot.SecondMessage = botdeploymentview.SecondMessage;
                    chatbot.BotSecret = botdeploymentview.BotSecret;
                    chatbot.BotURI = botdeploymentview.BotURI;
                    chatbot.WebSiteUrl = botdeploymentview.WebSiteUrl;
                    chatbot.BotQuestionAnswers = new List<BotQuestionAnswers>();
                    chatbot.Status = botdeploymentview.Status;
                    chatbot.IsPlanActive = botdeploymentview.IsPlanActive;
                    chatbot.DomainKey = botdeploymentview.DomainKey;
                    chatbot.BlobConnectionString = botdeploymentview.BlobConnectionString;
                    chatbot.ContainerName = botdeploymentview.ContainerName;
                    if (botdeploymentview.BotQuestionAnswers != null)
                    {
                        if (botdeploymentview.BotQuestionAnswers.Any())
                        {
                            foreach (var ans in botdeploymentview.BotQuestionAnswers)
                            {
                                BotQuestionAnswers item = new BotQuestionAnswers();
                                item.Question = ans.Question;
                                item.Answer = ans.Answer;
                                chatbot.BotQuestionAnswers.Add(item);
                            }
                        }
                    }

                    var userStore = new UserStore<ApplicationUser>(db);
                    var manager = new UserManager<ApplicationUser>(userStore);

                    var matchingUser = manager.FindByEmail(chatbot.Customer.Email);

                    manager.AddToRole(matchingUser.Id, "Customer");
                    manager.Update(matchingUser);

                    db.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Record updated successfully";
                    return response.GetResponse();
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Record does not exist";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string UpdateCustomerPlan(CustomerPlan CustomerPlan)
        {
            try
            {
                var customerplan = db.CustomerPlans.FirstOrDefault(c => c.CustomerId == CustomerPlan.CustomerId);
                if (customerplan != null)
                {

                }
            }
            catch
            {

            }
            return response.GetResponse();
        }

        [HttpPost]
        public string Delete(Int64 Id)
        {
            try
            {
                var chatbot = db.BotDeployments.Find(Id);
                if (chatbot != null)
                {
                    db.BotDeployments.Remove(chatbot);
                    db.SaveChanges();
                    response.IsSuccess = true;
                    response.Message = "Record deleted successfully";
                }
                return response.GetResponse();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Unable to delete record";
                return response.GetResponse();
            }
        }

        [HttpPost]
        public string BotDeploymentDelete(Int64 Id)
        {
            try
            {
                var botdeployment = db.CustomerPlans.FirstOrDefault(c => c.CustomerId == Id);
                if (botdeployment != null)
                {
                    db.CustomerPlans.Remove(botdeployment);
                    db.SaveChanges();
                    response.IsSuccess = true;
                    response.Message = "Record deleted successfully";
                }
                return response.GetResponse();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Unable to delete record";
                return response.GetResponse();
            }
        }

        [HttpPost]
        public string UpdateProfile(CustomerView customerView)
        {
            try
            {
                var customer = db.Customer.Find(customerView.Id);

                if (customer != null)
                {
                    customer.Name = customerView.Name;
                    customer.JobTitle = customerView.JobTitle;
                    customer.PricingPlan = customerView.PricingPlan;
                    customer.ContactNo = customerView.ContactNo;
                    customer.Address1 = customerView.Address1;
                    customer.Address2 = customerView.Address2;
                    customer.Address3 = customerView.Address3;
                    customer.City = customerView.City;
                    customer.Country = customerView.Country;
                    customer.ZipCode = customerView.ZipCode;
                    customer.BusinessRequirement = customerView.BusinessRequirement;
                    db.SaveChanges();

                    response.IsSuccess = true;
                    response.Data = Mapper.Map<CustomerView>(customer);
                    response.Message = "Profile updated successfully";
                    return response.GetResponse();
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = true;
                response.Message = "Unable to update profile";
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string GetCurrentUser()
        {
            try
            {
                _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var useremail = HttpContext.User.Identity.Name;
                var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;
                var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);
                var botdeployments = db.BotDeployments.Where(c => c.CustomerId == customer.Id).ToList();
                customer.BotDeployments = botdeployments;
                response.Data = Mapper.Map<CustomerView>(customer);
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response.GetResponse();
        }

        [HttpPost]
        public string GetCustomerActivePlan(Int64 customerId)
        {
            var customer = db.Customer.Find(customerId);
            var plan = db.CustomerPlans.FirstOrDefault(c => c.CustomerId == customer.Id && c.IsActive);
            response.Data = new
            {
                plan.Plan.MonthlyCharge,
                plan.Plan.Contract,
                plan.Plan.AllowedBotLimit,
                plan.Plan.AdvanceTraining,
                plan.Plan.Analyticsplan,
                plan.Plan.LogRetainingDay,
                plan.Plan.SiteSpecification,
                plan.Plan.InitialTraining,
                plan.Plan.TextQuery,
                plan.Plan.Entities,
                plan.Plan.Intent,
                plan.Plan.EmbedWidget,
                plan.Plan.FBWidget,
                plan.Plan.SlackWidget,
                plan.Plan.SkypeWidget,
                plan.Plan.TelegramWidget,
                plan.Plan.KikWidget,
                plan.Plan.SupportAvailability,
                plan.Plan.PagesScrapping,
                plan.Plan.KnowledgeDomain,
                plan.Plan.Name,
                plan.Plan.SetupFees,
                plan.ActivationDate,
                plan.ExpiryDate
            };
            return response.GetResponse();
        }

        [HttpGet]
        public string GetCustomerDetails(string clientId)
        {
            var botdeployment = db.BotDeployments.FirstOrDefault(c => c.T_BotClientId == clientId);
            if (botdeployment != null)
            {
                if (botdeployment.IsPlanActive)
                {
                    var customerDetails = new
                    {
                        WelcomeMsg = botdeployment.WelcomeMessage,
                        FirstMsg = botdeployment.FirstMessage,
                        SecondMsg = botdeployment.SecondMessage,
                        BaseColor = botdeployment.BackGroundColor,
                        BotSecret = botdeployment.BotSecret,
                        BotURI = botdeployment.BotURI,
                        WebsiteURL = botdeployment.WebSiteUrl,
                        DomainName = botdeployment.DomainName,
                        Botchatname = botdeployment.DomainKey,
                        LuisAppId = botdeployment.LuisId
                    };
                    response.IsSuccess = true;
                    response.Data = customerDetails;
                    return response.GetResponse();
                }
                else
                {
                    var customerDetails = new
                    {
                        WelcomeMsg = botdeployment.WelcomeMessage,
                        FirstMsg = botdeployment.FirstMessage,
                        SecondMsg = botdeployment.SecondMessage,
                        BaseColor = botdeployment.BackGroundColor,
                        BotSecret = botdeployment.BotSecret,
                        BotURI = botdeployment.BotURI,
                        WebsiteURL = botdeployment.WebSiteUrl,
                        DomainName = botdeployment.DomainName,
                        Botchatname = botdeployment.DomainKey,
                        LuisAppId = botdeployment.LuisId
                    };
                    response.IsSuccess = false;
                    response.Data = customerDetails;
                    response.Message = "Your plan has been disabled, please contact administrator";
                    return response.GetResponse();
                }

            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Please provide valid client Id";
                return response.GetResponse();
            }
        }

        [HttpGet]
        public string GetCustomers()
        {
            var customers = db.Customer.Include("CustomerPlans").Include("BotDeployments").ToList();
            response.Data = (from a in customers
                             select new
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 PricingPlan = a.PricingPlan,
                                 RegisterDate = a.RegisterDate,
                                 ExpiaryDate = a.CustomerPlans.Where(c => c.CustomerId == a.Id).FirstOrDefault().ExpiryDate,
                                 Email = a.Email,
                                 ActivePlan = a.CustomerPlans.FirstOrDefault(c => c.IsActive).Plan.Name,
                                 BotLimit = a.CustomerPlans.FirstOrDefault(c => c.IsActive).Plan.AllowedBotLimit
                             }).ToList();

            return response.GetResponse();
        }

        [HttpPost]
        public string GetCustomerById(Int64 customerId)
        {
            var customer = db.Customer.Include("BotDeployments").FirstOrDefault(c => c.Id == customerId);
            if (customer != null)
            {
                response.Data = Mapper.Map<CustomerView>(customer);
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string Send(string message, string clientid)
        {
            List<string> LUISresponse = new List<string>(); ;
            // Call the broadcastMessage method to update clients.

            //Call LUIS API // GET ITS INTENT / DECIDE WHAT TO BE SENT OUT
            try
            {
                string entity = null;
                var detailsUri = "http://147.75.71.162:8585/customer/getcustomerdetails?clientid=" + clientid;
                HttpWebRequest detailsrequest = WebRequest.Create(detailsUri) as System.Net.HttpWebRequest;
                detailsrequest.Method = "GET";
                HttpWebResponse detailsresponse = (HttpWebResponse)detailsrequest.GetResponse();
                var detailsreader = new StreamReader(detailsresponse.GetResponseStream()).ReadToEnd();
                var userdetail = db.BotDeployments.FirstOrDefault(c => c.T_BotClientId == clientid);
                if(userdetail!=null)
                {
                    var userluis = db.LuisDomains.FirstOrDefault(c => c.Id == userdetail.LuisId);
                    //LuisDomain domain = JsonConvert.DeserializeObject<LuisDomain>(detailsreader);
                    //https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/136e3019-5e1e-487e-a086-d1541f89c47b?subscription-key=e6d3d39c8364452baec720946d038b6f&timezoneOffset=-360&q=
                    
                    var Uri = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/" + userluis.LuisAppId + "?subscription-key=" + userluis.LuisAppKey + "&q=" + message;
                    HttpWebRequest request = WebRequest.Create(Uri) as System.Net.HttpWebRequest;
                    Encoding encoding = new UTF8Encoding();
                    request.Method = "GET";
                    HttpWebResponse webresponse = (HttpWebResponse)request.GetResponse();
                    var reader = new StreamReader(webresponse.GetResponseStream());
                    String jsonresponse = "";
                    String temp = null;
                    while (!reader.EndOfStream)
                    {
                        temp = reader.ReadLine();
                        jsonresponse += temp;
                    }
                    LuisReply Data = JsonConvert.DeserializeObject<LuisReply>(jsonresponse);
                    if(Data.entities.Length!=0)
                    {
                        if(Data.entities[0].entity!=null)
                        {
                            entity = Data.entities[0].entity;
                            LUISresponse = FetchFromDB(Data.topScoringIntent.intent, entity, userdetail.DomainKey);
                            if (Demo.response == "thirdeye_demo")
                            {
                                keeplog_demo(message, LUISresponse, clientid);
                            }
                            if (Demo.response == null)
                            {
                                string logfile_content = keeplog(message, LUISresponse, clientid);
                            }
                        }
                        if(Data.entities[0].type!=null)
                        {
                            entity = Data.entities[0].type;
                            LUISresponse = FetchFromDB(Data.topScoringIntent.intent, entity, userdetail.DomainKey);
                            if (Demo.response == "thirdeye_demo")
                            {
                                keeplog_demo(message, LUISresponse, clientid);
                            }
                            if (Demo.response == null)
                            {
                                string logfile_content = keeplog(message, LUISresponse, clientid);
                            }
                        }
                        
                    }
                    else
                    {
                        LUISresponse.Add("Hmmm...I didn’t quite get that. Shall we try again?");
                        if (Demo.response == "thirdeye_demo")
                        {
                            keeplog_demo(message, LUISresponse, clientid);
                        }
                        if (Demo.response == null)
                        {
                            string logfile_content = keeplog(message, LUISresponse, clientid);
                        }
                    }
                    response.Data = LUISresponse;
                    response.Message = "Success";
                    response.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                LUISresponse.Add("Hmmm...I didn’t quite get that. Shall we try again?");
                response.Message = e.Message;
                response.IsSuccess = false;
                response.Data = LUISresponse;
            }
            return response.GetResponse();
        }

        public List<string> FetchFromDB(string Intent, string Entity, string DomainKey)
        {
            List<string> data = new List<string>();
            string cs = ConfigurationManager.ConnectionStrings["SyraDbContext"].ToString();
            string sql = "Select * from dbo.BotContents where Intent='" + Intent + "' and Entity='" + Entity + "' and DomainKey='" + DomainKey + "'";
            SqlConnection connection = new SqlConnection(cs);
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        data.Add(dataReader["BotReply"].ToString());
                    }
                }
                command.Dispose();
                connection.Close();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine("Can not open connection ! ");
                data.Add(e.Message);
                return data;
            }
        }
        #endregion

        public string keeplog(string question, List<string> listanswer, string clientId)
        {
            var userdetail = db.BotDeployments.FirstOrDefault(c => c.T_BotClientId == clientId);
            string contents = null;
            if (userdetail != null)
            {
                try
                {
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(userdetail.BlobConnectionString);
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(userdetail.ContainerName);
                    string ipaddr = State.ip;
                    string reg = State.region;
                    string session_id = State.uuid;
                    DateTime datetime = DateTime.Now;
                    var date = datetime.ToString("dd-MM-yyyy");
                    var time = datetime.ToString("HH:mm:ss");
                    var blob_file_name = date + "" + ".csv";
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
                    string answer = string.Join(",", listanswer.ToArray());
                    contents = session_id + " | " + reg + " | " + ipaddr + " | " + question + " | " + answer + " | " + date + " | " + time + "\n";
                    string oldcontent;
                    bool blob_check = blockBlob.Exists();
                    if (blob_check == false)
                    {
                        oldcontent = null;
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(blockBlob.OpenRead()))
                        {
                            oldcontent = reader.ReadToEnd();
                        }
                    }
                    using (StreamWriter writer = new StreamWriter(blockBlob.OpenWrite()))
                    {
                        writer.Write(oldcontent);
                        writer.Write(contents);
                    }
                }
                catch (Exception e)
                {
                    response.Message = e.Message;
                }
            }
            return contents;
        }

        [HttpPost]
        public void SendMail(string Name, string IPAddress, string UniqueId)
        {
            State.region = Name;
            State.ip = IPAddress;
            State.uuid = UniqueId;
            Demo.response = null;
            //keeplog(message, answer, clientid);
        }

        [HttpPost]
        public void SendResponse(string Name, string IPAddress, string UniqueId, string Response)
        {
            Demo.region = Name;
            Demo.ip = IPAddress;
            Demo.uuid = UniqueId;
            Demo.response = Response;
        }

        [HttpPost]
        public string SendLink(string Name, string IPAddress, string UniqueId, string Url ,string clientid)
        {
            try
            {
                var userdetails = db.BotDeployments.FirstOrDefault(c => c.T_BotClientId == clientid);
                if(userdetails!=null)
                {
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(userdetails.BlobConnectionString);
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(WebConfigurationManager.AppSettings["clickedlink_container"]);
                    DateTime datetime = DateTime.Now;
                    var date = datetime.ToString("dd-MM-yyyy");
                    var time = datetime.ToString("HH:mm:ss");
                    var blob_file_name = date + "" + ".csv";
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
                    string contents = UniqueId + " | " + Name + " | " + IPAddress + " | " + Url + " | " + date + " | " + time + "\n";
                    string oldcontent;
                    bool blob_check = blockBlob.Exists();
                    if (blob_check == false)
                    {
                        oldcontent = null;
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(blockBlob.OpenRead()))
                        {
                            oldcontent = reader.ReadToEnd();
                        }
                    }
                    using (StreamWriter writer = new StreamWriter(blockBlob.OpenWrite()))
                    {
                        writer.Write(oldcontent);
                        writer.Write(contents);
                    }
                    response.Message = "Success";
                    response.IsSuccess = true;
                    response.Data = contents;
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.IsSuccess = false;
            }
            return response.GetResponse();
        }

        public void keeplog_demo(string question, List<string> answer, string clientid)
        {
            var userdetails = db.BotDeployments.FirstOrDefault(c=>c.T_BotClientId==clientid);
            string listanswer = "";
            if(userdetails!=null)
            {
                try
                {
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(userdetails.BlobConnectionString);
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(WebConfigurationManager.AppSettings["demolog_container"]);
                    string response_demo = Demo.response;
                    string ipaddr = State.ip;
                    string reg = State.region;
                    string session_id = State.uuid;
                    DateTime datetime = DateTime.Now;
                    var date = datetime.ToString("dd-MM-yyyy");
                    var time = datetime.ToString("HH:mm:ss");
                    var blob_file_name = date + "" + ".csv";
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_file_name);
                    listanswer = string.Join(",", answer.ToArray());
                    string contents = response_demo + " | " + session_id + " | " + reg + " | " + ipaddr + " | " + question + " | " + listanswer + " | " + date + " | " + time + "\n";
                    string oldcontent;
                    bool blob_check = blockBlob.Exists();
                    if (blob_check == false)
                    {
                        oldcontent = null;
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(blockBlob.OpenRead()))
                        {
                            oldcontent = reader.ReadToEnd();
                        }
                    }
                    using (StreamWriter writer = new StreamWriter(blockBlob.OpenWrite()))
                    {
                        writer.Write(oldcontent);
                        writer.Write(contents);
                    }
                }
                catch (Exception e)
                {
                    response.Message = e.StackTrace;
                }
            }
            
        }
    }
}