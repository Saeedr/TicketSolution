using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDataLayer.Model;

namespace TicketViewModel.SearchTicketViewModel
{
    public class SearchTicketViewModel
    {
        public Flight Flights { get; set; }
        public Corporate Corporates { get; set; }

        public int CountPassenger { get; set; }
    }
}
