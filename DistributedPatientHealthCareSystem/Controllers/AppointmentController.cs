using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DistributedPatientHealthCareSystem.DPHCSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using DistributedPatientHealthCareSystem.Hubs;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Dynamic;
using DistributedPatientHealthCareSystem.Helper;

namespace DistributedPatientHealthCareSystem.Controllers
{

    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Receptionist")]
    public class AppointmentController : Controller
    {
        private readonly DPHCSContext _context;
        private IConnectionManager _connectionManager { get; set; }

        protected ICompositeViewEngine viewEngine;

        ViewRenderService _ViewRenderService;

        public AppointmentController(DPHCSContext context, IConnectionManager connectionManager,
                                       ViewRenderService ViewRenderService
            )
        {
            _connectionManager = connectionManager;
            _context = context;
            _ViewRenderService = ViewRenderService;

        }


        // GET: Appointment
        public async Task<IActionResult> Index()
        {
            

            var Checkonline = _context.UserConnection.FirstOrDefault(c => c.UserName == "87");
            IHubContext context = Startup.ConnectionManager.GetHubContext<ChatHub>();
            await context.Clients.Client(Checkonline.ConnectionID).broadcastMessage("123","123");

            var dPHCSContext = _context.Appointment.Include(a => a.DoctorEmployee).Include(a => a.Patient).Include(a => a.ReceptionistEmployeet);

            return View(await dPHCSContext.ToListAsync());
        }



        // GET: Appointment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var appointment = await _context.Appointment
                .Include(a => a.DoctorEmployee)
                .Include(a => a.Patient)
                .Include(a => a.ReceptionistEmployeet)
                .SingleOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointment/Create
        public IActionResult Create(String SelectedSpecilization)
        {
            ViewData["DoctorSpecializationList"] = new SelectList(_context.DoctorSpecializationList, "Text", "Text");

            ViewData["DoctorList"] = new SelectList(
            _context.Person.Where(a => a.PersonNavigation.Role == "Doctor" && a.Employee.Spectialization == "Allergy Specialist"
            ).Select(a => new { a.PersonId, a.FirstName }).ToList(), "PersonId", "FirstName");


            //var DoctorList = (from p in _context.Person
            //                  join ua in _context.UserAccount on p.PersonId equals ua.UserAccountId
            //                  where ua.Role == "Doctor"
            //                  select p).ToList();

            //var DoctorwithSpecificSpecialization = (from p in _context.Person
            //                  join e in _context.Employee on p.PersonId equals e.EmployeeId
            //                  where e.Spectialization == "Allergy Specialist"
            //                  select p).ToList();

            //var avilableRoomWithBed = (from r in db.Tbl_Room
            //                           join s in db.Tbl_Bed on r.Rm_No equals s.Rm_No
            //                           where s.Status == 0
            //                           select new Tbl_Room()
            //                           {
            //                               Rm_No = r.Rm_No,
            //                               Rm_Name = r.Rm_Name,
            //                               Floar = r.Floar,
            //                               RentPerBed = r.RentPerBed
            //                           }).ToList();

            //ViewData["DoctorList"] = new SelectList(_context.Person.Where(a => a.Employee.EmployeeId == a.PersonId && a.PersonNavigation.Role == "Doctor").Select(a => new { a.PersonId, a.FirstName }).ToList(), "PersonId","FirstName");





            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "PatientId");
            ViewData["ReceptionistEmployeetId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId");

            return View();
        }

        public IActionResult _PartialAppointmentDoctor(string DoctorSpecilization)
        {



            ViewData["DoctorList"] = new SelectList(
               _context.Person.Where(a => a.PersonNavigation.Role == "Doctor" && a.Employee.Spectialization == DoctorSpecilization
               ).Select(a => new { a.PersonId, a.FirstName }).ToList(), "PersonId", "FirstName");

            //List<string> StatesList = new List<string>();
            //switch (DoctorSpecilization)
            //{
            //    case "India":
            //        StatesList.Add("New Delhi");
            //        StatesList.Add("Mumbai");
            //        StatesList.Add("Kolkata");
            //        StatesList.Add("Chennai");
            //        break;
            //    case "Australia":
            //        StatesList.Add("Canberra");
            //        StatesList.Add("Melbourne");
            //        StatesList.Add("Perth");
            //        StatesList.Add("Sydney");
            //        break;
            //    case "America":
            //        StatesList.Add("California");
            //        StatesList.Add("Florida");
            //        StatesList.Add("New York");
            //        StatesList.Add("Washignton");
            //        break;
            //    case "North Africa":
            //        StatesList.Add("Tunisia");
            //        StatesList.Add("Libya");
            //        StatesList.Add("Morocco");
            //        StatesList.Add("Sudan");
            //        break;
            //}
            //var DoctorList = JsonConvert.SerializeObject(ViewData["DoctorList"]);
            ////return DoctorList;



            //return Json(DoctorList, JsonRequestBehavior.AllowGet);
            return View(ViewData["DoctorList"]);
        }


        // POST: Appointment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Discription,DoctorEmployeeId,HeldDate,PatientId,VisitReson,OfficeAddress")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.AppointmentStatus = "Pending";

                var lastapp = ""; /*_context.Appointment.LastOrDefault().HeldDate;*/
                if (lastapp == "")
                {
                    appointment.PatientLastVisitDate = "No Visit";
                }
                else
                {
                    appointment.PatientLastVisitDate = lastapp;
                }
                appointment.SetedDate = DateTime.Now.ToLongDateString();
                appointment.ReceptionistEmployeetId = Convert.ToInt32(User.Identity.Name);

                _context.Appointment.Add(appointment);
                _context.SaveChanges();


                var Checkonline = _context.UserConnection.FirstOrDefault(c => c.UserName == appointment.DoctorEmployeeId.ToString());

                if (Checkonline != null)
                {
                    String PartialViewPath = "\\Views\\Doctor\\_PartialForSingleAppointmnet.cshtml";
                    List<Appointment> NewAppointment = new List<Appointment>();
                    Appointment newAppointment = _context.Appointment.FirstOrDefault(a => a.AppointmentId == appointment.AppointmentId);

                    NewAppointment.Add(newAppointment);

                    String AppendNewAppointment = _ViewRenderService.Render(PartialViewPath, NewAppointment);
                    String DecodedString = System.Net.WebUtility.HtmlDecode(AppendNewAppointment);

                    IHubContext context = Startup.ConnectionManager.GetHubContext<ChatHub>();
                    await context.Clients.Client(Checkonline.ConnectionID).broadcastMessage(AppendNewAppointment);

                }
                try
                {

                    return RedirectToAction("Index");
                    //var url = Url.Action("Index");
                    //return Content($"Go check out {url}, it's really great.");
                    /*RedirectToRoute("Appointment/Index")*/
                }
                catch (NullReferenceException ex)
                {
                    String e = ex.Message;
                }

            }
            ViewData["DoctorEmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId", appointment.DoctorEmployeeId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "PatientId", appointment.PatientId);
            ViewData["ReceptionistEmployeetId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId", appointment.ReceptionistEmployeetId);
            return View(appointment);

        }

        // GET: Appointment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.SingleOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["DoctorEmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "JoinDate", appointment.DoctorEmployeeId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "RegDate", appointment.PatientId);
            ViewData["ReceptionistEmployeetId"] = new SelectList(_context.Employee, "EmployeeId", "JoinDate", appointment.ReceptionistEmployeetId);
            return View(appointment);
        }

        // POST: Appointment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,AppointmentStatus,Discription,DoctorEmployeeId,HeldDate,PatientId,ReceptionistEmployeetId,SetedDate,VisitReson")] Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["DoctorEmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "JoinDate", appointment.DoctorEmployeeId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "RegDate", appointment.PatientId);
            ViewData["ReceptionistEmployeetId"] = new SelectList(_context.Employee, "EmployeeId", "JoinDate", appointment.ReceptionistEmployeetId);
            return View(appointment);
        }

        // GET: Appointment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.DoctorEmployee)
                .Include(a => a.Patient)
                .Include(a => a.ReceptionistEmployeet)
                .SingleOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointment.SingleOrDefaultAsync(m => m.AppointmentId == id);
            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointment.Any(e => e.AppointmentId == id);
        }


    }
}
