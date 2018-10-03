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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

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
        public void CountryCode(string json)
        {
            string oldcontent = "";
            var file = "C:/Users/trainee/Desktop/syra-santosh/Syra/AppScript/Analytics/Template/locations.json";
            using (StreamReader reader = new StreamReader(file))
            {
                oldcontent = reader.ReadToEnd();
                if(oldcontent!=null)
                {
                    oldcontent = null;
                }
            }
            using (StreamWriter writer = new StreamWriter(file))
            {
                //writer.Write(oldcontent);
                writer.Write(json.ToString());
                writer.Close();
            }
        }

        [HttpPost]
        public string GetAnalytics(DateTime startdt, DateTime enddt)
        {
            //try
            //{
            List<ArrayList> arraylist = new List<ArrayList>();
            List<SessionLog> logs = new List<SessionLog>();
            List<Lontitude> countries = new List<Lontitude>();
            List<Location> _data = new List<Location>();
            SyraDbContext db = new SyraDbContext();
            var ipdetails = new GetIPAddress();
            //var query = new Lontitude();
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

                                string jsonresponse = GetIPDetails(log.IPAddress);
                                ipdetails = JsonConvert.DeserializeObject<GetIPAddress>(jsonresponse);
                                countries.Add(new Lontitude { Countries = ipdetails.countryCode,UserQuery=log.UserQuery});
                            }
                        }
                        response.IsSuccess = true;
                        response.Data = logs;

                        //return response.GetResponse();
                    }
                }
                var dupcountries = countries.GroupBy(x => new { x.Countries }).Select(group => new { Name = group.Key, Count = group.Count() })
                             .OrderByDescending(x => x.Count);
                var dupUserQuery= countries.GroupBy(x => new { x.UserQuery }).Select(group => new { Name = group.Key, Count = group.Count() })
                              .OrderByDescending(x => x.Count);
                foreach(var y in dupUserQuery)
                {
                    arraylist.Add(new ArrayList { y.Name.UserQuery, y.Count });
                }
                response.Data = arraylist;
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
                    catch(Exception e)
                    {
                        string message = e.Message;
                        response.Message = message;
                    }
                }
                string json = JsonConvert.SerializeObject(_data.ToArray());
                CountryCode(json);
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string GetLogs(DateTime startdt,DateTime enddt)
        {
            //try
            //{
                List<SessionLog> logs = new List<SessionLog>();
                List<Lontitude> countries = new List<Lontitude>();
                List<Location> _data = new List<Location>();
                SyraDbContext db = new SyraDbContext();
                var ipdetails = new GetIPAddress();
                _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var useremail = HttpContext.User.Identity.Name;
                var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;
                
                if(aspnetuser!=null)
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
                    for(DateTime date = startdt; date <= enddt; date = date.AddDays(1))
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

                                    //string jsonresponse = GetIPDetails(log.IPAddress);
                                    //ipdetails = JsonConvert.DeserializeObject<GetIPAddress>(jsonresponse);
                                    //countries.Add(new Lontitude { Countries = ipdetails.countryCode });
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
                        bot.EmbeddedScript = "<script src= \"https://syra.ai/genericbot/assets/genericbot.js\" " + "clientID=\"" + bot.T_BotClientId + "\"></script>";
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
        public string GetMyBots(int pagesize=10, int pageno=0)
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
            }catch(Exception ex)
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
                    chatbot.BackGroundColor = botdeploymentview.BackGroundColor;
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
                    if(botdeploymentview.BotQuestionAnswers != null)
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
        public string UpdateProfile(CustomerView customerView)
        {
            try
            {
                var customer = db.Customer.Find(customerView.Id);

                if(customer != null)
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
            catch(Exception ex)
            {
                response.IsSuccess = true;
                response.Message = "Unable to update profile";
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string GetCurrentUser()
        {
            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var useremail = HttpContext.User.Identity.Name;
            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;
            var customer = db.Customer.FirstOrDefault(c => c.UserId == aspnetuser.Id);
            var botdeployments = db.BotDeployments.Where(c => c.CustomerId == customer.Id).ToList();
            customer.BotDeployments = botdeployments;
            response.Data = Mapper.Map<CustomerView>(customer);
            return response.GetResponse();
        }

        [HttpPost]
        public string GetCustomerActivePlan(Int64 customerId)
        {
            var customer = db.Customer.Find(customerId);
            var planobj = db.CustomerPlans.FirstOrDefault(c => c.CustomerId == customer.Id);
            var plancheck = db.CustomerPlans.FirstOrDefault(c => c.PlanId == planobj.PlanId && c.IsActive);

            response.Data = Mapper.Map<CustomerPlanView>(plancheck);

            return response.GetResponse();
        }

        [HttpGet]
        public string GetCustomerDetails(string clientId)
        {
            var botdeployment = db.BotDeployments.FirstOrDefault(c => c.T_BotClientId == clientId);
            if(botdeployment != null)
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
                        Botchatname = botdeployment.DomainKey
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
                        Botchatname = botdeployment.DomainKey
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
            if(customer != null)
            {
                response.Data = Mapper.Map<CustomerView>(customer);
            }
            return response.GetResponse();
        }

        //[HttpGet]
        //public string UpdateGUID()
        //{
        //    var botdeployments = db.BotDeployments.ToList();

        //    foreach (var bot in botdeployments)
        //    {
        //        bot.T_BotClientId = Guid.NewGuid().ToString();
        //    }
        //    db.SaveChanges();
        //    return "";
        //}
        #endregion

    }
}