using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Syra.Admin.DbContexts;
using Syra.Admin.Entities;
using Syra.Admin.Helper;

namespace Syra.Admin.Controllers
{
    public class LuisDomainsController : Controller
    {
        private SyraDbContext db = new SyraDbContext();
        private Response response = new Response();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        // GET: LuisDomains
        public ActionResult Index()
        {
            return View(db.LuisDomains.ToList());
        }

        // GET: LuisDomains/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LuisDomain luisDomain = db.LuisDomains.Find(id);
            if (luisDomain == null)
            {
                return HttpNotFound();
            }
            return View(luisDomain);
        }

        // GET: LuisDomains/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LuisDomains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Details,Category,LuisAppId,LuisAppKey")] LuisDomain luisDomain)
        {
            if (ModelState.IsValid)
            {
                db.LuisDomains.Add(luisDomain);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(luisDomain);
        }

        [HttpGet]
        public string GetDomain()
        {
            var useremail = HttpContext.User.Identity.Name;

            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;
            if(aspnetuser!=null)
            {
                var luisdomain = db.LuisDomains.ToList();
                response.Data = luisdomain;
                response.isSaved = true;
                return response.GetResponse();
            }
            else
            {
                response.isSaved = false;
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string CreateDomain(LuisDomain luisDomain)
        {
            var luisobj = new LuisDomain();
            var existsdomain = db.LuisDomains.Where(x => x.Name == luisDomain.Name).FirstOrDefault();
            if(existsdomain==null)
            {
                luisobj.Name = luisDomain.Name;
                luisobj.Details = luisDomain.Details;
                luisobj.Category = luisDomain.Category;
                luisobj.LuisAppId = luisDomain.LuisAppId;
                luisobj.LuisAppKey = luisDomain.LuisAppKey;

                db.LuisDomains.Add(luisobj);
                db.SaveChanges();
                response.isSaved = true;
                response.Message = "LuisDomain is created successsfully";
                return response.GetResponse();
            }
            else
            {
                response.isSaved = false;
                response.Message = "LuisDomain isn't created successsfully";
            }
            return response.GetResponse();
        }
        // GET: LuisDomains/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LuisDomain luisDomain = db.LuisDomains.Find(id);
            if (luisDomain == null)
            {
                return HttpNotFound();
            }
            return View(luisDomain);
        }

        // POST: LuisDomains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Details,Category,LuisAppId,LuisAppKey")] LuisDomain luisDomain)
        {
            if (ModelState.IsValid)
            {
                db.Entry(luisDomain).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(luisDomain);
        }

        // GET: LuisDomains/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LuisDomain luisDomain = db.LuisDomains.Find(id);
            if (luisDomain == null)
            {
                return HttpNotFound();
            }
            return View(luisDomain);
        }

        // POST: LuisDomains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            LuisDomain luisDomain = db.LuisDomains.Find(id);
            db.LuisDomains.Remove(luisDomain);
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
