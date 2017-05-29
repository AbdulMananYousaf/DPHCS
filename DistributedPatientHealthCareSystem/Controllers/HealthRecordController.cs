using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DistributedPatientHealthCareSystem.DPHCSModels;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace DistributedPatientHealthCareSystem.Controllers
{
    [Authorize(Roles ="Doctor")]
    public class HealthRecordController : Controller
    {
        private readonly DPHCSContext _context;

        public HealthRecordController(DPHCSContext context)
        {
            _context = context;    
        }

        // GET: HealthRecord
        public IActionResult Index(int? id)
        {


            ViewBag.PatientId = id;
               var dPHCSContext = _context.PatientHealthRecord.Where(p=>p.PatientId==id).ToList();

            ////PatientPrescription p = new PatientPrescription();
            ////p.MedicineName = "MedicineName1";
            ////p.Quantity = "Quantity1";
            ////p.UsageDirections = "UsageDirections1";

            ////PatientPrescription p2 = new PatientPrescription();
            ////p2.MedicineName = "MedicineName2";
            ////p2.Quantity = "Quantity2";
            ////p2.UsageDirections = "UsageDirections2";

            ////PatientHealthRecord phr = new PatientHealthRecord();
            ////phr.HealthRecordId = 1;
            ////phr.DoctorEmployeeId = 87;
            ////phr.HospitalId = 4;
            ////phr.PatientId = 97;
            ////phr.PatientPrescription.Add(p);
            ////phr.PatientPrescription.Add(p2);

            ////dPHCSContext.Add(phr);
            //var dPHCSContext = _context.PatientHealthRecord.Include(p => p.DoctorEmployee).Include(p => p.Hospital).Include(p => p.Patient.PatientId);
            //var dPHCSContext = _context.PatientHealthRecord.Where(p=>p.PatientId==id);

            return View(dPHCSContext);
        }

        // GET: HealthRecord/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientHealthRecord = await _context.PatientHealthRecord
                .Include(p => p.Patient)
                .Include(p=>p.PatientPrescription)
                .Include(p => p.PatientAllergie)
                .SingleOrDefaultAsync(m => m.HealthRecordId == id);
            if (patientHealthRecord == null)
            {
                return NotFound();
            }
            ViewBag.HealthRecordId =patientHealthRecord.HealthRecordId;
            ViewBag.PatientId = patientHealthRecord.PatientId;
            return View("Details",patientHealthRecord);
        }

        // GET: HealthRecord/Create
        public IActionResult Create(int Id)
        {
            PatientHealthRecord phr = new PatientHealthRecord();
            phr.DoctorEmployeeId = int.Parse(User.Identity.Name);
            phr.HospitalId = 1;
            phr.PatientId = Id;

            _context.PatientHealthRecord.Add(phr);
            _context.SaveChanges();
            ViewBag.PatientId = Id;
            ViewBag.HealthRecordId = phr.HealthRecordId;
            //ViewData["DoctorEmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "JoinDate");
            //ViewData["HospitalId"] = new SelectList(_context.Hospital, "HospitalId", "Address");
            //ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "RegDate");
            return View("Details",phr);
        }

        // POST: HealthRecord/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public String Create(string JsonData,String tablename)
        {

            //PatientPrescription pp = new PatientPrescription();
            //pp = JsonConvert.DeserializeObject<PatientPrescription>(JsonData);
            //String ppJSon = JsonConvert.SerializeObject(pp);

            //PatientHealthRecord pp = new PatientHealthRecord();
            //pp = JsonConvert.DeserializeObject<PatientHealthRecord>(JsonData);
            //String ppJSon = JsonConvert.SerializeObject(pp);
            using (var tr = _context.Database.BeginTransaction()) {
                try
                {
                  
                    switch (tablename)
                    {
                        case "Prescription":
                            PatientPrescription presc = new PatientPrescription();
                            presc = JsonConvert.DeserializeObject<PatientPrescription>(JsonData);
                            if (presc.PrescriptionId == 0)
                            {
                                _context.PatientPrescription.Add(presc);
                            }
                            else {
                                _context.PatientPrescription.Update(presc);
                            }
                            _context.SaveChanges();
                            tr.Commit();
                            String pj = JsonConvert.SerializeObject(presc);
                            //return presc.PrescriptionId.ToString();
                            return pj;
                       
                        default:
                            return "0";

                    }
                   
                 
                   
                    //     int HRid = pp.HealthRecordId;
                    //pp = _context.PatientHealthRecord.Find(HRid);
                    //ppJSon = JsonConvert.SerializeObject(pp);
                }
                catch (Exception e) {
                    tr.Rollback();

                    return e.ToString();
                }
               
               
            }

           


            //if (ModelState.IsValid)
            //{
            //    _context.Add(patientHealthRecord);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //ViewData["DoctorEmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "JoinDate", patientHealthRecord.DoctorEmployeeId);
            //ViewData["HospitalId"] = new SelectList(_context.Hospital, "HospitalId", "Address", patientHealthRecord.HospitalId);
            //ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "RegDate", patientHealthRecord.PatientId);
            //return View(patientHealthRecord);

            //return ppJSon;
        }

        // GET: HealthRecord/Edit/5
        public String Edit(int? id,String tablename)
        {
            if (id == null)
            {
                return "0";
            }

            switch (tablename)
            {
                case "Prescription":
                   
                    var pp=_context.PatientPrescription.FirstOrDefault(p=>p.PrescriptionId==id);
                    JsonConvert.SerializeObject(pp);
                    String J= JsonConvert.SerializeObject(pp);
                    return J;
                default:
                    return "0";

            }
            //ViewData["DoctorEmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "JoinDate", patientHealthRecord.DoctorEmployeeId);
            ////ViewData["HospitalId"] = new SelectList(_context.Hospital, "HospitalId", "Address", patientHealthRecord.HospitalId);
            //ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "RegDate", patientHealthRecord.PatientId);
            //return View(patientHealthRecord);
        }

        // POST: HealthRecord/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("HealthRecordId,DoctorEmployeeId,HospitalId,PatientId")] PatientHealthRecord patientHealthRecord)
        //{
        //    if (id != patientHealthRecord.HealthRecordId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(patientHealthRecord);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PatientHealthRecordExists(patientHealthRecord.HealthRecordId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    ViewData["DoctorEmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "JoinDate", patientHealthRecord.DoctorEmployeeId);
        //    //ViewData["HospitalId"] = new SelectList(_context.Hospital, "HospitalId", "Address", patientHealthRecord.HospitalId);
        //    ViewData["PatientId"] = new SelectList(_context.Patient, "PatientId", "RegDate", patientHealthRecord.PatientId);
        //    return View(patientHealthRecord);
        //}

        // GET: HealthRecord/Delete/5
        public String Delete(int? id, String tablename)
        {
            if (id == 0)
            {
                return "0";
            }


            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {

                    switch (tablename)
                    {
                        case "Prescription":

                            if (id != 0)
                            {
                                var pre = _context.PatientPrescription.FirstOrDefault(p => p.PrescriptionId == id);
                                _context.PatientPrescription.Remove(pre);
                            }
                            else
                            {
                                return "0";
                            }
                            _context.SaveChanges();
                            tr.Commit();

                            return "1";
                        case "HealthRecord":
                            var pp1 = _context.PatientPrescription.Where(r => r.PatientHealthRecordId == id);
                            _context.PatientPrescription.RemoveRange(pp1);
                            _context.SaveChanges();
                            var phr = _context.PatientHealthRecord.SingleOrDefault(r => r.HealthRecordId == id);
                            _context.PatientHealthRecord.Remove(phr);
                            _context.SaveChanges();
                            tr.Commit();
                            return "1";
                        default:
                            return "0";

                    }



                    //     int HRid = pp.HealthRecordId;
                    //pp = _context.PatientHealthRecord.Find(HRid);
                    //ppJSon = JsonConvert.SerializeObject(pp);
                }
                catch (Exception e)
                {
                    tr.Rollback();

                    return e.ToString();
                }
            }
               
        }

        //// POST: HealthRecord/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var patientHealthRecord = await _context.PatientHealthRecord.SingleOrDefaultAsync(m => m.HealthRecordId == id);
        //    _context.PatientHealthRecord.Remove(patientHealthRecord);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        private bool PatientHealthRecordExists(int id)
        {
            return _context.PatientHealthRecord.Any(e => e.HealthRecordId == id);
        }
    }
}
