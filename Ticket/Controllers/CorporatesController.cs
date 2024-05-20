using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ticket.Models;
using TicketDataLayer.Context;
using TicketDataLayer.Model;

namespace Ticket.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CorporatesController : Controller
    {
        private UnitOfWork db;

        // GET: Corporates
        public ActionResult Index()
        {
            db = new UnitOfWork();
            var entity = db.CorporateRepository.GetAll();
            db.Dispose();
            return View(entity);
        }

        // GET: Corporates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db = new UnitOfWork();
            Corporate corporate = db.CorporateRepository.GetById(id.Value);
            db.Dispose();
            if (corporate == null)
            {
                return HttpNotFound();
            }
            return View(corporate);
        }

        // GET: Corporates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Corporates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CorporateId,CorporateName,CorporateImage")] Corporate corporate, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db = new UnitOfWork();

                string fileName = Guid.NewGuid() + Path.GetFileName(file.FileName);
                string path = "~/Content/Image/" + fileName;

                file.SaveAs(Server.MapPath(path));

                corporate.CorporateImage = fileName;

                db.CorporateRepository.Insert(corporate);
                db.CorporateRepository.Save();
                db.Dispose();
                return RedirectToAction("Index");
            }

            return View(corporate);
        }

        // GET: Corporates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db = new UnitOfWork();
            Corporate corporate = db.CorporateRepository.GetById(id.Value);
            db.Dispose();
            if (corporate == null)
            {
                return HttpNotFound();
            }
            return View(corporate);
        }

        // POST: Corporates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CorporateId,CorporateName,CorporateImage")] Corporate corporate, HttpPostedFileBase file)
        {
            db = new UnitOfWork();
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~/Content/Image/" + corporate.CorporateImage)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/Content/Image/" + corporate.CorporateImage));
                    }

                    string fileName = Guid.NewGuid() + Path.GetFileName(file.FileName);
                    string path = "~/Content/Image/" + fileName;
                    file.SaveAs(Server.MapPath(path));
                    corporate.CorporateImage = fileName;
                }

                db.CorporateRepository.Update(corporate);
                db.CorporateRepository.Save();
                db.Dispose();
                return RedirectToAction("Index");
            }
            return View(corporate);
        }

        // GET: Corporates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db = new UnitOfWork();
            Corporate corporate = db.CorporateRepository.GetById(id.Value);
            db.Dispose();
            if (corporate == null)
            {
                return HttpNotFound();
            }
            return View(corporate);
        }

        // POST: Corporates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db = new UnitOfWork();


            Corporate corporate = db.CorporateRepository.GetById(id);

            if (System.IO.File.Exists(Server.MapPath("~/Content/Image/" + corporate.CorporateImage)))
            {
                System.IO.File.Delete(Server.MapPath("~/Content/Image/" + corporate.CorporateImage));
            }


            db.CorporateRepository.Delete(corporate);
            db.CorporateRepository.Save();
            db.Dispose();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
