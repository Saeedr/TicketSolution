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
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = " تاریخ سفارش")]
        public DateTime Order_Date { get; set; }

        [Required]
        [Display(Name = "تعداد مسافر")]
        public int Passenger_Count { get; set; }

        [Required]
        [Display(Name = " کل مبلغ پرداختی")]
        public int AllPrice { get; set; }
        public bool IsFinalPay { get; set; }

        [Required]
        [Display(Name = "نام مسافر")]

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int FlightId { get; set; }
        public Flight Flight { get; set; }

    }
}
