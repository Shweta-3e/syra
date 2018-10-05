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

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private SyraDbContext db = new SyraDbContext();
        private Response response = new Response();
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
                response.IsSuccess = true;
                return response.GetResponse();
            }
            else
            {
                response.IsSuccess = false;
            }
            return response.GetResponse();
        }

        [HttpGet]
        public string GetDb()
        {
            var useremail = HttpContext.User.Identity.Name;

            _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var aspnetuser = _userManager.FindByEmailAsync(useremail).Result;
            if (aspnetuser != null)
            {
                var dbdetail = db.ManageDbs.ToList();
                response.Data = dbdetail;
                response.IsSuccess = true;
                return response.GetResponse();
            }
            else
            {
                response.IsSuccess = false;
            }
            return response.GetResponse();
        }
        [HttpPost]
        public string GetLuisDomain(Int64 Id)
        {
            var findluis = db.LuisDomains.Find(Id);
            if(findluis!=null)
            {
                response.Data = findluis;
                return response.GetResponse();
            }
            else
            {
                response.Data = null;
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string GetDbEntries(Int64 Id)
        {
            var finddb = db.ManageDbs.Find(Id);
            if (finddb != null)
            {
                response.Data = finddb;
                return response.GetResponse();
            }
            else
            {
                response.Data = null;
            }
            return response.GetResponse();
        }
        [HttpPost]
        public string CreateEntries(ManageDb manageDb)
        {
            var dbobj = new ManageDb();
            var existsdomain = db.ManageDbs.Where(x => x.Entity == manageDb.Entity).FirstOrDefault();
            if (existsdomain == null)
            {
                dbobj.Entity = manageDb.Entity;
                dbobj.Intent = manageDb.Intent;
                dbobj.Response = manageDb.Response;
                db.ManageDbs.Add(dbobj);
                db.SaveChanges();
                response.IsSuccess = true;
                response.Message = "Data is inserted successsfully";
                return response.GetResponse();
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Data isn't inserted successsfully";
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string UpdateDatabase(ManageDb managedb)
        {
            var finddb = db.ManageDbs.Find(managedb.Id);
            //luisdomain = luisDomain;
            var existsdata = db.ManageDbs.Find(managedb.Id);
            if (existsdata != null)
            {
                existsdata.Intent = managedb.Intent;
                existsdata.Entity = managedb.Entity;
                existsdata.Response = managedb.Response;
                db.SaveChanges();
                response.IsSuccess = true;
                response.Message = "Record is updated successfully";
                return response.GetResponse();
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Record isn't updated ";
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string UpdateLuisDomain(LuisDomain luisDomain)
        {
            var luisdomain = db.LuisDomains.Find(luisDomain.Id);
            //luisdomain = luisDomain;
            var existsdomain = db.LuisDomains.Find(luisDomain.Id);
            if(existsdomain!=null)
            {
                existsdomain.Name = luisDomain.Name;
                existsdomain.Details = luisDomain.Details;
                existsdomain.Category = luisDomain.Category;
                existsdomain.LuisAppId = luisDomain.LuisAppId;
                existsdomain.LuisAppKey = luisDomain.LuisAppKey;
                db.SaveChanges();
                response.IsSuccess = true;
                response.Message = "Record is updated successfully";
                return response.GetResponse();
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Record isn't updated ";
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
                response.IsSuccess = true;
                response.Message = "LuisDomain is created successsfully";
                return response.GetResponse();
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "LuisDomain isn't created successsfully";
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string DeleteData(Int64 Id)
        {
            var existsdata = db.ManageDbs.Find(Id);
            if (existsdata != null)
            {
                db.ManageDbs.Remove(existsdata);
                db.SaveChanges();
                response.IsSuccess = true;
                response.Message = "Record is deleted successfully";
                return response.GetResponse();
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Record is not deleted successfully";
            }
            return response.GetResponse();
        }

        [HttpPost]
        public string DomainDelete(Int64 Id)
        {
            var existsdomain = db.LuisDomains.Find(Id);
            if(existsdomain!=null)
            {
                db.LuisDomains.Remove(existsdomain);
                db.SaveChanges();
                response.IsSuccess = true;
                response.Message = "Record is deleted successfully";
                return response.GetResponse();
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Record is not deleted successfully";
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
