using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDataLayer.Model
{
    public class Corporate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="شرکت هواپیمایی")]
        public int CorporateId { get; set; }

        [Required]
        [Display(Name ="نام شرکت هواپیمایی")]
        public string CorporateName { get; set; }

        [Display(Name = "تصویر")]
        public string CorporateImage { get; set; }

        public ICollection<Flight> Flights { get; set; }

    }
}
