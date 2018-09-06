using Microsoft.AspNet.Identity.Owin;
using Syra.Admin.DbContexts;
using Syra.Admin.Entities;
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
                return RedirectToAction("Login","Account");
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
                    int count = db.BotDeployments.Where(c=>c.CustomerId==customer.Id).Count();
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




    }


}