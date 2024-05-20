using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDataLayer.Model
{
    public enum FlightClass
    {
        [Display(Name = "اکونومی")]
        اکونومی =1,
        [Display(Name = "بیزینسی")]
        بیزینسی =2
    }
}
