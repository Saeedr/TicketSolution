using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDataLayer.Model
{
    public enum TicketType
    {
        [Display(Name ="سیستمی")]
        سیستمی=1,
        [Display(Name = "چارتر")]
        چارتر =2
    }
}
