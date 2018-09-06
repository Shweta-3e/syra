using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Syra.Admin.DbContexts;
using Syra.Admin.Entities;

namespace Syra.Admin.Controllers
{
    public class LuisDomainsController : Controller
    {
        private SyraDbContext db = new SyraDbContext();

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
