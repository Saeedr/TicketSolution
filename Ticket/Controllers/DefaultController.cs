using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using TicketDataLayer.Context;
using TicketDataLayer.Model;
using TicketViewModel.SearchFlightViewModel;
using TicketViewModel.SearchTicketViewModel;
using TicketViewModel.TicketInfoViewModel;

namespace Ticket.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchFlight(SearchFlightViewModel entity)
        {
            UnitOfWork db = new UnitOfWork();

            Expression<Func<Flight, bool>> flightWhereCluase = f => f.Start_Location == entity.First_Location
                                                              && f.End_Location == entity.Second_Location
                                                              && f.Start_Date == entity.MoveDate
                                                              && f.Capacity >= entity.PassengerCount
                                                              && (entity.TicketType != 0 ? f.Ticket_Type == entity.TicketType : true)
                                                              && (entity.FlightClass != 0 ? f.Flight_Class == entity.FlightClass : true);
            Expression<Func<Corporate, bool>> corporateWhereCluase = c => c.CorporateId != 0 &&
                                                                        (entity.CorporateId != 0 ? c.CorporateId == entity.CorporateId : true);


            var entityList = from f in db.FlightRepository.GetAll(flightWhereCluase)
                             join c in db.CorporateRepository.GetAll(corporateWhereCluase) on f.CorporateId equals c.CorporateId
                             select new SearchTicketViewModel
                             {
                                 Flights = f,
                                 Corporates = c,
                                 CountPassenger = entity.PassengerCount
                             };
            return View(entityList);
        }

        public ActionResult FilterFlight()
        {
            UnitOfWork db = new UnitOfWork();
            ViewBag.Corporate = new SelectList(db.CorporateRepository.GetAll(), "CorporateId", "CorporateName");
            return PartialView();
        }

        [Authorize(Roles = "User")]
        public ActionResult AddTicket(int id, bool reLoad = false)
        {
            UnitOfWork db = new UnitOfWork();
            var countPassenger = Convert.ToInt32(Request.Params["countPassenger"]);

            if (!reLoad)
            {
                var flight = db.FlightRepository.GetById(id);
                UpdatePassengerCount(flight, countPassenger, true);
            }
            var entityList = (from f in db.FlightRepository.GetAll(f => f.FlightId == id)
                              join c in db.CorporateRepository.GetAll() on f.CorporateId equals c.CorporateId
                              select new SearchTicketViewModel
                              {
                                  Flights = f,
                                  Corporates = c,
                                  CountPassenger = countPassenger
                              }).FirstOrDefault();
            TempData["flightId"] = id;
            return View(entityList);
        }

        public void UpdatePassengerCount(Flight entity, int count, bool isSelectedFlight)
        {
            UnitOfWork db = new UnitOfWork();
            db.FlightsRepository.UpdateFlightCountPassenger(entity, count, isSelectedFlight);
        }

        public ActionResult PassengerInfo()
        {
            int count = Convert.ToInt32(Request.Params["countPassenger"]);
            var passenger = new List<Passenger>();
            for (int i = 0; i < count; i++)
            {
                passenger.Add(new Passenger());
            }
            return PartialView(passenger);
        }

        //[HttpPost]
        //public ActionResult InsertPassenger(IEnumerable<Passenger> entity, int countPassenger)
        //{
        //    UnitOfWork db = new UnitOfWork();
        //    string token = Guid.NewGuid().ToString();
        //    int flightId = Convert.ToInt32(TempData["flightId"]);
        //    foreach (var item in entity)
        //    {
        //        item.FlightId = flightId;
        //        item.token = token;
        //        item.UserId = User.Identity.GetUserId();
        //        db.PassengerRepository.Insert(item);
        //        db.PassengerRepository.Save();
        //    }
        //    db.Dispose();
        //    return RedirectToAction("AddTicket", new { id = flightId, countPassenger = countPassenger, reLoad = true, token = token });
        //}

        [HttpPost]
        public JsonResult InsertPassenger(IEnumerable<Passenger> entity)
        {
            UnitOfWork db = new UnitOfWork();
            string token = Guid.NewGuid().ToString();

            TempData["token"] = token;

            int flightId = Convert.ToInt32(TempData["flightId"]);
            foreach (var item in entity)
            {
                item.FlightId = flightId;
                item.token = token;
                item.UserId = User.Identity.GetUserId();
                db.PassengerRepository.Insert(item);
                db.PassengerRepository.Save();
            }
            db.Dispose();
            TempData["PassengerInfo"] = entity;
            return Json(entity, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPassengerInfo()
        {
            //UnitOfWork db = new UnitOfWork();
            //string token = Request.Params["token"].ToString();
            //var passengers = db.PassengerRepository.GetAll(p => p.token.Equals(token)).ToList();
            return PartialView(TempData["PassengerInfo"]);
        }

        [HttpPost]
        public JsonResult ReturnPassengerCount(int id, int passengerCount)
        {
            UnitOfWork db = new UnitOfWork();
            var flight = db.FlightRepository.GetById(id);
            UpdatePassengerCount(flight, passengerCount, false);
            db.Dispose();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Payment(int flightId, int countPassenger)
        {
            UnitOfWork db = new UnitOfWork();
            var flight = db.FlightRepository.GetById(flightId);
            int allPrice = countPassenger * (int)flight.Price;

            Order item = new Order();
            item.AllPrice = allPrice;
            item.Passenger_Count = countPassenger;
            item.UserId = User.Identity.GetUserId();
            item.FlightId = flightId;
            item.IsFinalPay = false;
            item.Order_Date = DateTime.Now;
            db.OrderRepository.Insert(item);
            db.OrderRepository.Save();


            System.Net.ServicePointManager.Expect100Continue = false;
            ZarinPal.PaymentGatewayImplementationServicePortTypeClient zp = new ZarinPal.PaymentGatewayImplementationServicePortTypeClient();
            string Authority;

            int Status = zp.PaymentRequest("YOUR-ZARINPAL-MERCHANT-CODE", allPrice, "درگاه پرداخت خرید بلیط", "Saeedrajayi@gmail.com", "09156000647", "http://localhost:49705/Default/Verify/" + item.OrderId + "?count=" + countPassenger, out Authority);

            if (Status == 100)
            {
                Response.Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + Authority);
            }
            else
            {
                UpdatePassengerCount(flight, countPassenger, false);
                ViewBag.Error = Status;
            }
            return View();
        }

        public ActionResult Verify(int id, int count)
        {
            UnitOfWork db = new UnitOfWork();
            var order = db.OrderRepository.GetById(id);
            var flight = db.FlightRepository.GetById(order.FlightId);

            string token = TempData["token"].ToString();
            if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null && Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
            {
                if (Request.QueryString["Status"].ToString().Equals("OK"))
                {


                    int Amount = order.AllPrice;
                    long RefID;
                    System.Net.ServicePointManager.Expect100Continue = false;
                    ZarinPal.PaymentGatewayImplementationServicePortTypeClient zp = new ZarinPal.PaymentGatewayImplementationServicePortTypeClient();

                    int Status = zp.PaymentVerification("YOUR-ZARINPAL-MERCHANT-CODE", Request.QueryString["Authority"].ToString(), Amount, out RefID);

                    if (Status == 100)
                    {
                        order.IsFinalPay = true;
                        db.OrderRepository.Update(order);
                        db.OrderRepository.Save();
                        ViewBag.IsSuccess = true;
                        ViewBag.RefId = RefID;

                        var entityList = (from o in db.OrderRepository.GetAll(oo => oo.OrderId == order.OrderId)
                                          join f in db.FlightRepository.GetAll() on o.FlightId equals f.FlightId
                                          join u in db.UserRepository.GetAll() on o.UserId equals u.Id
                                          join c in db.CorporateRepository.GetAll() on f.CorporateId equals c.CorporateId
                                          select new TicketInfoViewModel
                                          {
                                              Flights = f,
                                              Orders = o,
                                              User = u,
                                              Corporates = c,
                                              Count = count,
                                              Passengers = db.PassengerRepository.GetAll(p => p.FlightId == order.FlightId && p.token == token).ToList()

                                          }).FirstOrDefault();
                        return View(entityList);
                    }
                    else
                    {
                        UpdatePassengerCount(flight, count, false);
                        RedirectToAction("ErrorPage");
                        //Response.Write("Error!! Status: " + Status);
                    }

                }
                else
                {
                    UpdatePassengerCount(flight, count, false);
                    RedirectToAction("ErrorPage");
                    //Response.Write("Error! Authority: " + Request.QueryString["Authority"].ToString() + " Status: " + Request.QueryString["Status"].ToString());
                }
            }
            else
            {
                UpdatePassengerCount(flight, count, false);
                RedirectToAction("ErrorPage");
            }
            return View();
        }

        public ActionResult ErrorPage()
        {
            return View();
        }

        public ActionResult ReportInfo(int orderId, int count, string token)
        {
            return new Rotativa.ActionAsPdf("Print", new { id = orderId, count = count, token = token })
            {
                FileName = Server.MapPath("~/Content/Prints/sample.pdf")
            };
        }

        public ActionResult Print(int id, int count, string token)
        {
            UnitOfWork db = new UnitOfWork();
            var entityList = (from o in db.OrderRepository.GetAll(oo => oo.OrderId == id)
                              join f in db.FlightRepository.GetAll() on o.FlightId equals f.FlightId
                              join u in db.UserRepository.GetAll() on o.UserId equals u.Id
                              join c in db.CorporateRepository.GetAll() on f.CorporateId equals c.CorporateId
                              select new TicketInfoViewModel
                              {
                                  Flights = f,
                                  Orders = o,
                                  User = u,
                                  Corporates = c,
                                  Count = count,
                                  Passengers = db.PassengerRepository.GetAll(p => p.FlightId == o.FlightId && p.token == token).ToList()

                              }).FirstOrDefault();
            return View(entityList);
        }


    }
}