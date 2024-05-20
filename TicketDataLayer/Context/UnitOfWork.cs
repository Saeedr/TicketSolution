using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Models;
using TicketDataLayer.GenericRepository;
using TicketDataLayer.Model;

namespace TicketDataLayer.Context
{
    public class UnitOfWork : IDisposable
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private GenericRepository<Corporate> corporateRepository;
        private GenericRepository<Flight> flightRepository;
        private GenericRepository<Passenger> passengerRepository;
        private GenericRepository<Order> orderRepository;
        private GenericRepository<Payment> paymentRepository;
        private FlightRepository.FlightRepository flightsRepository;
        private GenericRepository<ApplicationUser> userRepository;


        public GenericRepository<Corporate> CorporateRepository
        {
            get
            {
                if (corporateRepository == null)
                {
                    corporateRepository = new GenericRepository<Corporate>(db);
                }
                return corporateRepository;
            }
        }

        public GenericRepository<Flight> FlightRepository
        {
            get
            {
                if (flightRepository == null)
                {
                    flightRepository = new GenericRepository<Flight>(db);
                }
                return flightRepository;
            }
        }

        public GenericRepository<Passenger> PassengerRepository
        {
            get
            {
                if (passengerRepository == null)
                {
                    passengerRepository = new GenericRepository<Passenger>(db);
                }
                return passengerRepository;
            }
        }

        public GenericRepository<Order> OrderRepository
        {
            get
            {
                if (orderRepository == null)
                {
                    orderRepository = new GenericRepository<Order>(db);
                }
                return orderRepository;
            }
        }

        public GenericRepository<Payment> PaymentRepository
        {
            get
            {
                if (paymentRepository == null)
                {
                    paymentRepository = new GenericRepository<Payment>(db);
                }
                return paymentRepository;
            }
        }

        public FlightRepository.FlightRepository FlightsRepository
        {
            get
            {
                if (flightsRepository == null)
                {
                    flightsRepository = new FlightRepository.FlightRepository(db);
                }
                return flightsRepository;
            }
        }

        public GenericRepository<ApplicationUser> UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new GenericRepository<ApplicationUser>(db);
                }
                return userRepository;
            }
        }

        
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
