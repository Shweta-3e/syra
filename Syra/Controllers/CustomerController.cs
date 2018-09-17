using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Syra.Admin.DbContexts;
using Syra.Admin.Entities;
using Syra.Admin.Helper;
using Syra.Admin.Models;
using Syra.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                        response.isSaved = false;
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
                        if(botdeployment.BotQuestionAnswers != null)
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
                        
                        customer.BotDeployments.Add(bot);
                        db.SaveChanges();
                        response.isSaved = true;
                        response.Message = "Chat bot created successfully";
                        return response.GetResponse();
                        //return RedirectToAction("MyBots");
                    }
                }
                else
                {
                    response.isSaved = false;
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
        public string GetMyBots(int pagesize=10, int pageno=0)
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

                result.isSaved = true;
                result.Entities = Mapper.Map<BotDeploymentView[]>(userbots.Skip(skip).Take(pagesize));
                return result.GetResponse();
            }
            else
            {
                response.isSaved = false;
                //return RedirectToAction("Login", "Account");
            }

            return response.GetResponse();
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
                    chatbot.BotQuestionAnswers = new List<BotQuestionAnswers>();

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

                    response.isSaved = true;
                    response.Message = "Record updated successfully";
                    return response.GetResponse();
                }
                else
                {
                    response.isSaved = false;
                    response.Message = "Record does not exist";
                }
            }
            catch (Exception ex)
            {
                response.isSaved = false;
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
                    response.isSaved = true;
                    response.Message = "Record deleted successfully";
                }
                return response.GetResponse();
            }
            catch (Exception ex)
            {
                response.isSaved = false;
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

                    response.isSaved = true;
                    response.Data = Mapper.Map<CustomerView>(customer);
                    response.Message = "Profile updated successfully";
                    return response.GetResponse();
                }
            }
            catch(Exception ex)
            {
                response.isSaved = true;
                response.Message = "Unable to update profile";
            }
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
        #endregion

    }
}