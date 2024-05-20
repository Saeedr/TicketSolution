using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Models;
using TicketDataLayer.Model;

namespace TicketViewModel.TicketInfoViewModel
{
    public class TicketInfoViewModel
    {
        public Flight Flights { get; set; }
        public Corporate Corporates { get; set; }
        public List<Passenger> Passengers { get; set; }
        public Order Orders { get; set; }
        public ApplicationUser User { get; set; }
        public int Count { get; set; }
    }
}
