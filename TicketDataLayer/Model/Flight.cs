using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDataLayer.Model
{
    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightId { get; set; }
        [Required]
        [Display(Name ="شماره پرواز")]
        public string Flight_Number { get; set; }
        [Required]
        [Display(Name = "ظرفیت بار مجاز")]
        public int CargoWeight { get; set; }

        [Required]
        [Display(Name = "کلاس پرواز")]
        public FlightClass Flight_Class { get; set; }

        [Required]
        [Display(Name = " ساعت حرکت")]
        public string Start_Houre { get; set; }

        [Required]
        [Display(Name = " تاریخ حرکت")]
        public String Start_Date { get; set; }

        [Required]
        [Display(Name = " نوع بلیط")]
        public TicketType Ticket_Type { get; set; }

        [Required]
        [Display(Name = " مبدأ")]
        public City Start_Location { get; set; }

        [Required]
        [Display(Name = " مقصد")]
        public City End_Location { get; set; }

        [Required]
        [Display(Name = " قیمت بلیط")]
        public Decimal Price { get; set; }

        [Required]
        [Display(Name = " ظرفیت پرواز")]
        public int Capacity { get; set; }

        [Display(Name = "  شرکت")]
        public int CorporateId { get; set; }
        [Display(Name = "  شرکت")]
        public Corporate Corporate { get; set; }


        public ICollection<Passenger> Passengers { get; set; }
        public ICollection<Order> Orders { get; set; }




    }
}
