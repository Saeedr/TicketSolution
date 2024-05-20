using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Ticket.Models;

[assembly: OwinStartupAttribute(typeof(Ticket.Startup))]
namespace Ticket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoleAndUser();
        }

        public void CreateRoleAndUser()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleMgr = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            if (!roleMgr.RoleExists("Admin"))
            {
                var role = new IdentityRole("Admin");
                roleMgr.Create(role);

                var user = new ApplicationUser();
                user.UserName = "Saeedrajayi@gmail.com";
                user.Email = "Saeedrajayi@gmail.com";
                user.EmailConfirmed = true;
                user.FullName = "مدیریت سایت";
                string pw = "Admin@123";

                var newUser = userMgr.Create(user, pw);
                if (newUser.Succeeded)
                {
                    var result = userMgr.AddToRole(user.Id, "Admin");
                }
            }
        }
    }
}
