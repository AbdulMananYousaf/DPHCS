using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DistributedPatientHealthCareSystem.DPHCSModels;
using Microsoft.AspNetCore.Authorization;
using System.Dynamic;
using DistributedPatientHealthCareSystem.Helper;

namespace DistributedPatientHealthCareSystem.Controllers
{
    [Authorize(Roles ="Doctor")]
    public class DoctorController : Controller
    {
        private readonly DPHCSContext _context;
        ViewRenderService _ViewRenderService;
        public DoctorController(DPHCSContext context, ViewRenderService ViewRenderService)
        {
            
            _context = context;
            _ViewRenderService = ViewRenderService;
        }
        public IActionResult Index()
        {
            //dynamic x = new ExpandoObject();
            //x.Test = "Yes";
            //String path = "_PartialForSingleAppointmnet.cshtml";
            //_ViewRenderService.Render(path, AppointmentList);
            IList<Appointment> AppointmentList=_context.Appointment.Where(d => d.DoctorEmployeeId.ToString() == User.Identity.Name).ToList();
            AppointmentList.Count();
           
            return View(AppointmentList);
        }
    }
}