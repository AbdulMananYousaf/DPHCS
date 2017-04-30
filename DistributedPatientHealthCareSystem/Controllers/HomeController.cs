using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DistributedPatientHealthCareSystem.DPHCSModels;

namespace DistributedPatientHealthCareSystem.Controllers
{
    public class HomeController : Controller
    {
        private DPHCSContext _context = null;
        public HomeController(DPHCSContext context)
        {
            _context = context;

        }
        public IActionResult Index()
       {

          
            
            return View();
        }

        public IActionResult About()
        {
            if (HttpContext.Session.GetString("UserAccountId") != null)
            {
                ViewBag.UserAccountId = HttpContext.Session.GetString("UserAccountId");
                ViewBag.UserName = HttpContext.Session.GetString("Name");
                ViewBag.Role = HttpContext.Session.GetString("Role");
            }
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            if (HttpContext.Session.GetString("UserAccountId") != null)
            {
                ViewBag.UserAccountId = HttpContext.Session.GetString("UserAccountId");
                ViewBag.UserName = HttpContext.Session.GetString("Name");
                ViewBag.Role = HttpContext.Session.GetString("Role");
            }
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
