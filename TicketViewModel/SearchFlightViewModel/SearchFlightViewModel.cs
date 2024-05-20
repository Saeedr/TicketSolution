using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDataLayer.Model;

namespace TicketViewModel.SearchFlightViewModel
{
    public class SearchFlightViewModel
    {
        [Display(Name="مبدا")]
        public City First_Location { get; set; }

        [Display(Name = "مقصد")]
        public City Second_Location { get; set; }

        [Display(Name = "تاریخ پرواز")]
        public string MoveDate { get; set; }

        [Display(Name = "تعداد مسافر")]
        public int PassengerCount { get; set; }
        [Display(Name = "نام شرکت")]
        public int CorporateId { get; set; }
        [Display(Name = " کلاس پروازی")]
        public FlightClass FlightClass { get; set; }
        [Display(Name = " نوع پرواز")]
        public TicketType TicketType { get; set; }


    }
}
