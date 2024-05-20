using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDataLayer.Model;

namespace TicketViewModel.OrderInfoViewModel
{
    public class OrderInfoViewModel
    {
        public Flight Flights { get; set; }
        public Order Orders { get; set; }
        public List<Passenger> Passengers { get; set; }
    }
}
