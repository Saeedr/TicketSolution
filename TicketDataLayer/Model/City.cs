using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDataLayer.Model
{
    public enum City
    {
        [Display(Name = "تهران")]
        تهران =021,
        [Display(Name = "اصفهان")]
        اصفهان =031,
        [Display(Name = "مشهد")]
        مشهد =051
    }
}
