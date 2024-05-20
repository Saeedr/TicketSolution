using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Models;

namespace TicketDataLayer.Model
{
    public class Passenger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PassengerId { get; set; }

        [Required]
        [Display(Name ="نام مسافر")]
        public string PassengerName { get; set; }

        [Required]
        [Display(Name = "کد ملی مسافر")]
        public string NationalCode { get; set; }

        [Required]
        [Display(Name = "جنسیت مسافر")]
        public int Gender { get; set; }

        [Display(Name = " کلید ثیت مسافر ")]
        public string token { get; set; }

        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }


    }
}
