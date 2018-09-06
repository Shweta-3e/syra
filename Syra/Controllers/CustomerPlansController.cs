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
    public class CustomerPlansController : Controller
    {
        private SyraDbContext db = new SyraDbContext();

        // GET: CustomerPlans
        public ActionResult Index()
        {
            var customerPlans = db.CustomerPlans.Include(c => c.Customer).Include(c => c.Plan);
            return View(customerPlans.ToList());
        }

        // GET: CustomerPlans/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerPlan customerPlan = db.CustomerPlans.Find(id);
            if (customerPlan == null)
            {
                return HttpNotFound();
            }
            return View(customerPlan);
        }

        // GET: CustomerPlans/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "Id");
            ViewBag.PlanId = new SelectList(db.Plans, "Id", "Id");
            return View();
        }

        // POST: CustomerPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,PlanId,ActivationDate,ExpiryDate,IsActive")] CustomerPlan customerPlan)
        {
            if (ModelState.IsValid)
            {
                customerPlan.ActivationDate = DateTime.Now;
                customerPlan.ExpiryDate = DateTime.Now;
                db.CustomerPlans.Add(customerPlan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "Id", customerPlan.CustomerId);
            ViewBag.PlanId = new SelectList(db.Plans, "Id", "Id", customerPlan.PlanId);
            return View(customerPlan);
        }

        // GET: CustomerPlans/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerPlan customerPlan = db.CustomerPlans.Find(id);
            if (customerPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "Id", customerPlan.CustomerId);
            ViewBag.PlanId = new SelectList(db.Plans, "Id", "Id", customerPlan.PlanId);
            return View(customerPlan);
        }

        // POST: CustomerPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,PlanId,ActivationDate,ExpiryDate,IsActive")] CustomerPlan customerPlan)
        {
            if (ModelState.IsValid)
            {
                customerPlan.ActivationDate = DateTime.Now;
                customerPlan.ExpiryDate = DateTime.Now;
                db.Entry(customerPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "Id", customerPlan.CustomerId);
            ViewBag.PlanId = new SelectList(db.Plans, "Id", "Id", customerPlan.PlanId);
            return View(customerPlan);
        }

        // GET: CustomerPlans/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerPlan customerPlan = db.CustomerPlans.Find(id);
            if (customerPlan == null)
            {
                return HttpNotFound();
            }
            return View(customerPlan);
        }

        // POST: CustomerPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CustomerPlan customerPlan = db.CustomerPlans.Find(id);
            db.CustomerPlans.Remove(customerPlan);
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
