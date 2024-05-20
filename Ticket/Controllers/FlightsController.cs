using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class FlightsController : Controller
    {
        UnitOfWork db = new UnitOfWork();

        // GET: Flights
        public ActionResult Index()
        {
            //var flights = db.Flights.Include(f => f.Corporate);
            var flights = db.FlightRepository.GetAll(null, null, "Corporate");
            return View(flights.ToList());
        }

        // GET: Flights/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Flight flight = db.Flights.Find(id);
            Flight flight = db.FlightRepository.GetById(id.Value);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // GET: Flights/Create
        public ActionResult Create()
        {

            //ViewBag.CorporateId = new SelectList(db.Corporates, "CorporateId", "CorporateName");
            ViewBag.CorporateId = new SelectList(db.CorporateRepository.GetAll(), "CorporateId", "CorporateName");

            return View();

        }

        // POST: Flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FlightId,Flight_Number,CargoWeight,Flight_Class,Start_Houre,Start_Date,Ticket_Type,Start_Location,End_Location,Price,Capacity,CorporateId")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                //db.Flights.Add(flight);
                //db.SaveChanges();

                db.FlightRepository.Insert(flight);
                db.FlightRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.CorporateId = new SelectList(db.CorporateRepository.GetAll(), "CorporateId", "CorporateName", flight.CorporateId);
            return View(flight);
        }

        // GET: Flights/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Flight flight = db.Flights.Find(id);
            Flight flight = db.FlightRepository.GetById(id.Value);
            if (flight == null)
            {
                return HttpNotFound();
            }
            ViewBag.CorporateId = new SelectList(db.CorporateRepository.GetAll(), "CorporateId", "CorporateName", flight.CorporateId);
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FlightId,Flight_Number,CargoWeight,Flight_Class,Start_Houre,Start_Date,Ticket_Type,Start_Location,End_Location,Price,Capacity,CorporateId")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(flight).State = EntityState.Modified;
                //db.SaveChanges();
                db.FlightRepository.Update(flight);
                db.FlightRepository.Save();
                TempData["SuccessEdit"] = "عمل ویرایش پرواز با موفقیت انجام شد";
                return RedirectToAction("Index");
            }
            ViewBag.CorporateId = new SelectList(db.CorporateRepository.GetAll(), "CorporateId", "CorporateName", flight.CorporateId);
            return View(flight);
        }

        // GET: Flights/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Flight flight = db.Flights.Find(id);
            Flight flight = db.FlightRepository.GetById(id.Value);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Flight flight = db.Flights.Find(id);
            Flight flight = db.FlightRepository.GetById(id);
            //db.Flights.Remove(flight);
            //db.SaveChanges();
            db.FlightRepository.Delete(flight);
            db.FlightRepository.Save();
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
