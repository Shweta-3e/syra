using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using Syra.Admin.DbContexts;
using Syra.Admin.Entities;
using Syra.Admin.Helper;

namespace Syra.Admin.Controllers
{
    public class PlansController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private SyraDbContext db = new SyraDbContext();
        private Response response = new Response();

        // GET: Plans
        public ActionResult Index()
        {
            return View(db.Plans.ToList());
        }

        // GET: Plans/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan plan = db.Plans.Find(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(plan);
        }

        // GET: Plans/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public string CreateNewPlan(Plan plan)
        {
            var planobj = new Plan();
            var plancheck = db.Plans.Where(x=>x.Name==planobj.Name).FirstOrDefault();
            if(plancheck==null)
            {
                planobj.Name = plan.Name;
                planobj.Types = plan.Types;
                planobj.MonthlyCharge = plan.MonthlyCharge;
                planobj.SetupFees = plan.SetupFees;
                planobj.Contract = plan.Contract;
                planobj.SiteSpecification = plan.SiteSpecification;
                planobj.KnowledgeDomain = plan.KnowledgeDomain;
                planobj.AllowedBotLimit = plan.AllowedBotLimit;
                planobj.InitialTraining = plan.InitialTraining;
                planobj.AdvanceTraining = plan.AdvanceTraining;
                planobj.TextQuery = plan.TextQuery;
                planobj.PagesScrapping = plan.PagesScrapping;
                planobj.Entities = plan.Entities;
                planobj.Intent = plan.Intent;
                planobj.LogRetainingDay = plan.LogRetainingDay;
                planobj.Analyticsplan = plan.Analyticsplan;
                planobj.SupportAvailability = plan.SupportAvailability;
                planobj.EmbedWidget = plan.EmbedWidget;
                planobj.FBWidget = plan.FBWidget;
                planobj.SlackWidget = plan.SlackWidget;
                planobj.SlackWidget = plan.SlackWidget;
                planobj.SkypeWidget = plan.SkypeWidget;
                planobj.TelegramWidget = plan.TelegramWidget;
                planobj.KikWidget = plan.KikWidget;

                db.Plans.Add(planobj);
                db.SaveChanges();
                response.isSaved = true;
                response.Message = "Plan is created successfully";
                return response.GetResponse();
            }
            else
            {
                response.Message = "Plan is not created.";
                response.isSaved = false;
                //return response.GetResponse();
            }
            return response.GetResponse();
        }

        [HttpGet]
        public string GetPlans()
        {
            var useremail = HttpContext.User.Identity.Name;

            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;
            if(aspnetuser!=null)
            {
                var plan = db.Plans.ToList();
                response.isSaved = true;
                response.Data = plan;
                return response.GetResponse();
            }
            else
            {
                response.isSaved = false;
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string UpdatePlan(Plan plan)
        {
            var planobj = new Plan();
            var plancheck = db.Plans.Where(x => x.Name == planobj.Name).FirstOrDefault();
            if (plancheck == null)
            {
                planobj.Name = plan.Name;
                planobj.Types = plan.Types;
                planobj.MonthlyCharge = plan.MonthlyCharge;
                planobj.SetupFees = plan.SetupFees;
                planobj.Contract = plan.Contract;
                planobj.SiteSpecification = plan.SiteSpecification;
                planobj.KnowledgeDomain = plan.KnowledgeDomain;
                planobj.AllowedBotLimit = plan.AllowedBotLimit;
                planobj.InitialTraining = plan.InitialTraining;
                planobj.AdvanceTraining = plan.AdvanceTraining;
                planobj.TextQuery = plan.TextQuery;
                planobj.PagesScrapping = plan.PagesScrapping;
                planobj.Entities = plan.Entities;
                planobj.Intent = plan.Intent;
                planobj.LogRetainingDay = plan.LogRetainingDay;
                planobj.Analyticsplan = plan.Analyticsplan;
                planobj.SupportAvailability = plan.SupportAvailability;
                planobj.EmbedWidget = plan.EmbedWidget;
                planobj.FBWidget = plan.FBWidget;
                planobj.SlackWidget = plan.SlackWidget;
                planobj.SlackWidget = plan.SlackWidget;
                planobj.SkypeWidget = plan.SkypeWidget;
                planobj.TelegramWidget = plan.TelegramWidget;
                planobj.KikWidget = plan.KikWidget;

                db.Plans.Add(planobj);
                db.SaveChanges();
                response.isSaved = true;
                response.Message = "Plan is created successfully";
                return response.GetResponse();
            }
            else
            {
                response.Message = "Plan is not created.";
                response.isSaved = false;
                //return response.GetResponse();
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string DeletePlan(Int64 Id)
        {
            try
            {
                var planfind = db.Plans.Find(Id);
                if(planfind!=null)
                {
                    db.Plans.Remove(planfind);
                    db.SaveChanges();
                    response.isSaved = true;
                    response.Message = "Record deleted successfully";
                    return response.GetResponse();
                }
                else
                {
                    response.isSaved = false;
                    response.Message = "Record isn't deleted successfully";
                }
            }
            catch(Exception e)
            {
                string message = e.Message;
                response.Message = message;
            }
            return response.GetResponse();
        }

        // POST: Plans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Types,MonthlyCharge,SetupFees,Contract,SiteSpecification,KnowledgeDomain,InitialTraining,AdvanceTraining,TextQuery,PagesScrapping,Entities,Intent,LogRetainingDay,Analyticsplan,EmbedWidget,FBWidget,SlackWidget,SkypeWidget,TelegramWidget,KikWidget,SupportAvailability")] Plan plan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var plan_obj = new Plan();
                    plan_obj = db.Plans.Where(x => x.Name == plan.Name).FirstOrDefault();
                    if (plan_obj == null)
                    {
                        db.Plans.Add(plan);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
            }
            catch(Exception e)
            {
                string message = e.Message.ToString();
                
            }

            return View(plan);
        }

        // GET: Plans/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan plan = db.Plans.Find(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(plan);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Types,MonthlyCharge,SetupFees,Contract,SiteSpecification,KnowledgeDomain,InitialTraining,AdvanceTraining,TextQuery,PagesScrapping,Entities,Intent,LogRetainingDay,Analyticsplan,EmbedWidget,FBWidget,SlackWidget,SkypeWidget,TelegramWidget,KikWidget,SupportAvailability")] Plan plan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(plan);
        }

        // GET: Plans/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan plan = db.Plans.Find(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(plan);
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Plan plan = db.Plans.Find(id);
            db.Plans.Remove(plan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
