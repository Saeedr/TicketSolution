using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketViewModel.UserViewModel
{
    public class UserInfoViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool EmailConfirm { get; set; }
        public string RoleName { get; set; }
    }
}
