using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket.Models;
using TicketViewModel.UserViewModel;

namespace Ticket.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {  
            return View();
        }

        public ActionResult GetAllUser()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var usersWithRoles = (from user in db.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      UserName = user.UserName,
                                      Email = user.Email,
                                      EmailConfirm = user.EmailConfirmed,
                                      RoleName = (from userRole in user.Roles join role in db.Roles on userRole.RoleId equals role.Id select role.Name).ToList()
                                  }).ToList().Select(u => new UserInfoViewModel()
                                  {
                                      UserId=u.UserId,
                                      UserName=u.UserName,
                                      Email=u.Email,
                                      EmailConfirm=u.EmailConfirm,
                                      RoleName=string.Join(",",u.RoleName)
                                  });

            return PartialView(usersWithRoles);
        }

        public ActionResult GetAllRole()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var roles = db.Roles.ToList();
            return PartialView(roles); 
        }

        public ActionResult CreateUser()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateUser(FormCollection form)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            string FullName = form["txtName"];
            string Email = form["txtEmail"];
            string UserName = form["txtUserName"];
            string password = form["txtPass"];

            var user = new ApplicationUser();
            user.Email = Email;
            user.UserName = UserName;
            user.FullName = FullName;
            userMgr.Create(user, password);


            return RedirectToAction("Index");
        }

        public ActionResult CreateRole()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateRole(FormCollection form)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string roleName = form["txtRole"];
            var roleMgr = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            if (!roleMgr.RoleExists(roleName))
            {
                var role = new IdentityRole(roleName);
                roleMgr.Create(role);
            }

            return RedirectToAction("Index");
        }

        public ActionResult AssigneRole()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var roleList = new SelectList(db.Roles.ToList(), "Name", "Name");
            var userList = new SelectList(db.Users.Where(u => u.Roles.Count == 0).ToList(), "Id", "UserName");

            ViewBag.Role = roleList;
            ViewBag.User = userList;
            return PartialView();
        }

        [HttpPost]
        public ActionResult AssigneRole(FormCollection form)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var roleName = form["Role"];
            var userId = form["User"];

            ApplicationUser user = db.Users.Where(u => u.Id == userId).FirstOrDefault();
            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            userMgr.AddToRole(userId, roleName);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult LogOutAdmin()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Default");
        }

    }
}