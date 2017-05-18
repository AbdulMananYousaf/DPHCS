using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using DistributedPatientHealthCareSystem.DPHCSModels;
using System;
using Microsoft.AspNetCore.Identity;
using DistributedPatientHealthCareSystem.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using DistributedPatientHealthCareSystem.Hubs;

namespace DistributedPatientHealthCareSystem.Controllers
{

    public class UserAccountController : Controller
    {

        private DPHCSContext _context = null;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _externalCookieScheme;

        public UserAccountController(DPHCSContext context, SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager, IOptions<IdentityCookieOptions> identityCookieOptions)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {

            //Add Admin For DPHCS
            //UserAccount AdminUserAccount = new UserAccount();
            //AdminUserAccount.Password = "P@kistan3@";
            //AdminUserAccount.Role = "Admin";
            //await _context.AddAsync(AdminUserAccount);
            //await _context.SaveChangesAsync();
            //var user = new ApplicationUser { Id = AdminUserAccount.UserAccountId.ToString(), UserName = AdminUserAccount.UserAccountId.ToString() };
            //var result = await _userManager.CreateAsync(user, AdminUserAccount.Password);
            //await _userManager.AddToRoleAsync(user, "Admin");
            //Admin Added
           
        
            if (User.Identity.Name != null)
            {
                var UA = _context.UserAccount.FirstOrDefault(u => u.UserAccountId == int.Parse(User.Identity.Name));

                switch (UA.Role)
                {
                    case "Admin":
                        return RedirectToAction("Index", "Employee");
                    case "Laboratory Technician":
                        return RedirectToAction("WelcomeLaboratory");
                    case "Receptionist":
                        return RedirectToAction("Index", "Patient");
                    case "Doctor":
                        return RedirectToAction("Index", "Home");
                    case "Patient":
                        return RedirectToAction("Index", "Home");
                    default:
                        break;
                }

            }
           
            await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(UserAccount model, string returnUrl)
        {
            String a=User.Identity.Name;
            
            var user = await _userManager.FindByIdAsync(model.UserAccountId.ToString());
         
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var UA =_context.UserAccount.FirstOrDefault(u => u.UserAccountId == model.UserAccountId);
              
                switch (UA.Role)
                {
                    case "Admin":
                        return RedirectToAction("Index", "Employee");
                    case "Laboratory Technician":
                        return RedirectToAction("WelcomeLaboratory");
                    case "Receptionist":
                        return RedirectToAction("Index","Patient");
                    case "Doctor":
                        return RedirectToAction("Index", "Home");
                    case "Patient":
                        return RedirectToAction("Index", "Home");
                    default:
                        break;
                }

            }
            else
            {
                ModelState.AddModelError("", "Username or password is wrong.");
            }
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            throw new NotImplementedException();
        }

        public IActionResult Welcome(){


            String Role = HttpContext.Session.GetString("Role");
            switch(Role)
            {
                case "Admin":
                    return RedirectToAction("WelcomeAdmin", "UserAccount");

                case "patient":
                    return RedirectToAction("WelcomePatient");
                case "doctor":
                    return RedirectToAction("WelcomeDoctor");

                default:
                    return View("Login");
            };

           
        }
        public IActionResult WelcomeAdmin()
        {
            if (HttpContext.Session.GetString("UserAccountId") != null)
            {
                ViewBag.UserAccountId = HttpContext.Session.GetString("UserAccountId");
                ViewBag.UserName = HttpContext.Session.GetString("Name");
                ViewBag.Role = HttpContext.Session.GetString("Role");
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public IActionResult WelcomeDoctor()
        {
            if (HttpContext.Session.GetString("UserAccountId") != null)
            {
                ViewBag.UserAccountId = HttpContext.Session.GetString("UserAccountId");
                ViewBag.Name = HttpContext.Session.GetString("Name");
                ViewBag.Role = HttpContext.Session.GetString("Role");
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("index","home");
        }
    }
}