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
    public class BotDeploymentsController : Controller
    {
        private SyraDbContext db = new SyraDbContext();

        // GET: BotDeployments
        public ActionResult Index()
        {
            var botDeployments = db.BotDeployments.Include(b => b.Customer).Include(b => b.LuisDomain);
            return View(botDeployments.ToList());
        }

        // GET: BotDeployments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BotDeployment botDeployment = db.BotDeployments.Find(id);
            if (botDeployment == null)
            {
                return HttpNotFound();
            }
            return View(botDeployment);
        }

        // GET: BotDeployments/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "Name");
            ViewBag.LuisId = new SelectList(db.LuisDomains, "Id", "Name");
            return View();
        }

        // POST: BotDeployments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,DeploymentDate,ResourceGroupName,BlobStorageName,WebSiteUrl,DeleteDate,LuisId,IsDeleted,IsPlanActive,DeploymentScript")] BotDeployment botDeployment)
        {
            if (ModelState.IsValid)
            {
                botDeployment.DeploymentDate = DateTime.Now;
                botDeployment.DeleteDate = DateTime.Now;
                db.BotDeployments.Add(botDeployment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "Name", botDeployment.CustomerId);
            ViewBag.LuisId = new SelectList(db.LuisDomains, "Id", "Name", botDeployment.LuisId);
            return View(botDeployment);
        }

        // GET: BotDeployments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BotDeployment botDeployment = db.BotDeployments.Find(id);
            if (botDeployment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "Name", botDeployment.CustomerId);
            ViewBag.LuisId = new SelectList(db.LuisDomains, "Id", "Name", botDeployment.LuisId);
            return View(botDeployment);
        }

        // POST: BotDeployments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,DeploymentDate,ResourceGroupName,BlobStorageName,WebSiteUrl,DeleteDate,LuisId,IsDeleted,IsPlanActive,DeploymentScript")] BotDeployment botDeployment)
        {
            if (ModelState.IsValid)
            {
                botDeployment.DeploymentDate = DateTime.Now;
                botDeployment.DeleteDate = DateTime.Now;
                db.Entry(botDeployment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "Name", botDeployment.CustomerId);
            ViewBag.LuisId = new SelectList(db.LuisDomains, "Id", "Name", botDeployment.LuisId);
            return View(botDeployment);
        }

        // GET: BotDeployments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BotDeployment botDeployment = db.BotDeployments.Find(id);
            if (botDeployment == null)
            {
                return HttpNotFound();
            }
            return View(botDeployment);
        }

        // POST: BotDeployments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            BotDeployment botDeployment = db.BotDeployments.Find(id);
            db.BotDeployments.Remove(botDeployment);
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
