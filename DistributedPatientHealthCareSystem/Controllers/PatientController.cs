using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DistributedPatientHealthCareSystem.DPHCSModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace DistributedPatientHealthCareSystem.Controllers
{
    
    //[Authorize(Roles ="Receptionist")]
    [Authorize(Roles ="Receptionist,Doctor")]
    public class PatientController : Controller
    {
        private readonly DPHCSContext _context;

        public PatientController(DPHCSContext context)
        {
            _context = context;    
        }

        // GET: Patient
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Doctor")) {

                var phr = _context.PatientHealthRecord.Where(d => d.DoctorEmployeeId.ToString() == User.Identity.Name);
                var PatientList = new List<Patient>();
                foreach (var item in phr)
                {
                    if (item.DoctorEmployeeId.ToString()==User.Identity.Name) {
                        var r=PatientList.Find(p=>p.PatientId== item.PatientId);
                        if (r == null)
                        {
                            var patient = _context.Patient.Include(p => p.PatientNavigation).FirstOrDefault(p => p.PatientId == item.PatientId);
                            PatientList.Add(patient);
                        }
                       
                    }
                }
                
                return View(PatientList);
            }
            ViewBag.Role = HttpContext.Session.GetString("Role");
            var dPHCSContext = _context.Patient.Include(p => p.PatientNavigation);
            return View(await dPHCSContext.ToListAsync());
        }

        // GET: Patient/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .Include(p => p.PatientNavigation)
                .SingleOrDefaultAsync(m => m.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patient/Create
        public IActionResult Create()
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            ViewData["PatientId"] = new SelectList(_context.Person, "PersonId", "Address");
            return View();
        }

        // POST: Patient/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Receptionist")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,RegDate,PatientNavigation")] Patient patient)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            if (ModelState.IsValid)
            {
                UserAccount UA = new UserAccount();
                UA.Role = "Patient";
                UA.Password = patient.PatientNavigation.Email;
                patient.RegDate = DateTime.Now.ToShortDateString();
                patient.PatientNavigation.PersonNavigation = UA;
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["PatientId"] = new SelectList(_context.Person, "PersonId", "Address", patient.PatientId);
            return View(patient);
        }

        [Authorize(Roles = "Receptionist")]
        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            if (id == null)
            {
                return NotFound();
            }
            var person = await _context.Person.SingleOrDefaultAsync(m => m.PersonId == id);

            var patient = await _context.Patient.SingleOrDefaultAsync(m => m.PatientId == id);
            patient.PatientNavigation = person; 
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Person, "PersonId", "Address", patient.PatientId);
            return View(patient);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Receptionist")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientId,RegDate,PatientNavigation")] Patient patient)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            if (id != patient.PatientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    patient.PatientNavigation.PersonId = patient.PatientId;

                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.PatientId))
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
            ViewData["PatientId"] = new SelectList(_context.Person, "PersonId", "Address", patient.PatientId);
            return View(patient);
        }
        [Authorize(Roles = "Receptionist")]
        // GET: Patient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .Include(p => p.PatientNavigation)
                .SingleOrDefaultAsync(m => m.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            var patient = await _context.Patient.SingleOrDefaultAsync(m => m.PatientId == id);
            var UA= await _context.UserAccount.SingleOrDefaultAsync(m => m.UserAccountId == id);
            var person = await _context.Person.SingleOrDefaultAsync(m => m.PersonId == id);

            _context.Patient.Remove(patient);
            _context.Person.Remove(person);
            _context.UserAccount.Remove(UA);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.PatientId == id);
        }
    }
}
