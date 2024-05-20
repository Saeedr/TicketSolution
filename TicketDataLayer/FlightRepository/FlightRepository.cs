using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Models;
using TicketDataLayer.GenericRepository;
using TicketDataLayer.Model;

namespace TicketDataLayer.FlightRepository
{
    public class FlightRepository : GenericRepository<Flight>
    {
        public FlightRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void UpdateFlightCountPassenger(Flight entity, int count, bool isSelectedFlight)
        {
            if (isSelectedFlight == true)
            {
                entity.Capacity -= count;
            }
            else
            {
                entity.Capacity += count;
            }
            base.Update(entity);
            base.Save();

        }
    }
}
