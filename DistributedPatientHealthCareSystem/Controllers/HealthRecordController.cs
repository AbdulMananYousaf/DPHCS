using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DistributedPatientHealthCareSystem.DPHCSModels;

namespace DistributedPatientHealthCareSystem.Controllers
{
    public class HealthRecordController : Controller
    {
        private readonly DPHCSContext _context;

        public HealthRecordController(DPHCSContext context)
        {
            _context = context;    
        }

        // GET: HealthRecord
        public async Task<IActionResult> Index()
        {
            var dPHCSContext = _context.PatientHealthRecord.Include(p => p.DoctorEmployee).Include(p => p.Hospital).Include(p => p.Patient);
            return View(await dPHCSContext.ToListAsync());
        }

        // GET: HealthRecord/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientHealthRecord = await _context.PatientHealthRecord
                .Include(p => p.DoctorEmployee)
                .Include(p => p.Hospital)
                .Include(p => p.Patient)
                .SingleOrDefaultAsync(m => m.HealthRecordId == id);
            if (patientHealthRecord == null)
            {
                return NotFound();
            }

            return View(patientHealthRecord);
        }

        // GET: HealthRecord/Create
        public IActionResult Create()
        {
            ViewData["DoctorEmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "JoinDate");
            ViewData["HospitalId"] = new SelectList(_context.Hospital, "HospitalId", "Address");
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "RegDate");
            return View();
        }

        // POST: HealthRecord/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HealthRecordId,DoctorEmployeeId,HospitalId,PatientId")] PatientHealthRecord patientHealthRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patientHealthRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["DoctorEmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "JoinDate", patientHealthRecord.DoctorEmployeeId);
            ViewData["HospitalId"] = new SelectList(_context.Hospital, "HospitalId", "Address", patientHealthRecord.HospitalId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "RegDate", patientHealthRecord.PatientId);
            return View(patientHealthRecord);
        }

        // GET: HealthRecord/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientHealthRecord = await _context.PatientHealthRecord.SingleOrDefaultAsync(m => m.HealthRecordId == id);
            if (patientHealthRecord == null)
            {
                return NotFound();
            }
            ViewData["DoctorEmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "JoinDate", patientHealthRecord.DoctorEmployeeId);
            ViewData["HospitalId"] = new SelectList(_context.Hospital, "HospitalId", "Address", patientHealthRecord.HospitalId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "RegDate", patientHealthRecord.PatientId);
            return View(patientHealthRecord);
        }

        // POST: HealthRecord/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HealthRecordId,DoctorEmployeeId,HospitalId,PatientId")] PatientHealthRecord patientHealthRecord)
        {
            if (id != patientHealthRecord.HealthRecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientHealthRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientHealthRecordExists(patientHealthRecord.HealthRecordId))
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
            ViewData["DoctorEmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "JoinDate", patientHealthRecord.DoctorEmployeeId);
            ViewData["HospitalId"] = new SelectList(_context.Hospital, "HospitalId", "Address", patientHealthRecord.HospitalId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "RegDate", patientHealthRecord.PatientId);
            return View(patientHealthRecord);
        }

        // GET: HealthRecord/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientHealthRecord = await _context.PatientHealthRecord
                .Include(p => p.DoctorEmployee)
                .Include(p => p.Hospital)
                .Include(p => p.Patient)
                .SingleOrDefaultAsync(m => m.HealthRecordId == id);
            if (patientHealthRecord == null)
            {
                return NotFound();
            }

            return View(patientHealthRecord);
        }

        // POST: HealthRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientHealthRecord = await _context.PatientHealthRecord.SingleOrDefaultAsync(m => m.HealthRecordId == id);
            _context.PatientHealthRecord.Remove(patientHealthRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PatientHealthRecordExists(int id)
        {
            return _context.PatientHealthRecord.Any(e => e.HealthRecordId == id);
        }
    }
}
