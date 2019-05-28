using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SmartTicket.Business;
using SmartTicket.Business.Result;
using SmartTicket.DataAccess.EntityFramework;
using SmartTicket.Entities;

namespace SmartTicket.WebAppUI.Controllers
{
    public class AccountController : Controller
    {
        UserManager us = new UserManager();

        [AllowAnonymous]
        public ActionResult Login()
        {
            //Bakılacak burası nasıl olacak
            if (string.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                FormsAuthentication.SignOut();
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                return View(model);

            }

            BusinessLayerResult<User> res = us.Login(model);
            if (res.Errors.Count > 0)
            {
                res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                return View(model);
            }

            return RedirectToAction("Index", "Home", new { area = res.Result.Role.ToString() });


        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User model)
        {

            if (ModelState.IsValid)
            {
                BusinessLayerResult<User> res = us.Register(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                us.Insert(res.Result);
                return RedirectToAction("Index", "Home", new { area = res.Result.Role.ToString() });
            }
            return View();

        }
        public ActionResult ForgetPassword(User model)
        {

            return View(model);
        }
        public ActionResult ConfirmEmail(int id)
        {
            ViewBag.id = id;
            return View();
        }

    }
}