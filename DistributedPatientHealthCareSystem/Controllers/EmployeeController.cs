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
using DistributedPatientHealthCareSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace DistributedPatientHealthCareSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class EmployeeController : Controller
    {
        private readonly DPHCSContext _context;

        private UserManager<ApplicationUser> _userManager;

        public EmployeeController(DPHCSContext context, UserManager<ApplicationUser> userManager)
        {
           
            _context = context;
            _userManager = userManager;
        }

       
        // GET: Employee
        public async Task<IActionResult> Index()
        {
            
            ViewBag.Id= User.Identity.Name;
            var dPHCSContext = _context.Employee.Include(e => e.EmployeeNavigation).Include(u=>u.EmployeeNavigation.PersonNavigation);
            IList<Employee> ILE = await dPHCSContext.ToListAsync();
            return View(await dPHCSContext.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            ViewBag.Role = HttpContext.Session.GetString("Role");
            if (id == null)
            {
                
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.EmployeeNavigation).Include(u=>u.EmployeeNavigation.PersonNavigation)
                .SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Person, "PersonId", "PersonId");
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JoinDate,Salary,Spectialization,Status,EmployeeNavigation")] Employee employee)
        {
            


            ///Add Employee Loop
            //for (int i = 0; i < 20; i++)
            //{
            //    UserAccount UA1 = new UserAccount
            //    {
            //        UserAccountId = 0,
            //        Role = "Admin",
            //        Password = "Pakistan3@"
            //    };

            //    Person P1 = new Person
            //    {
            //        PersonNavigation = UA1,
            //        PersonId = 0,
            //        FirstName = "Abdul Manan",
            //        LastName = "Yousaf",
            //        Gender = "Male",
            //        Cnic = "13231450666",
            //        Email = "Shani@gmail.com",
            //        Mobile = "03016495520",
            //        Address = "DAska",
            //        DoB = "10-10-1993",
            //        ProfilePic = "Url",
            //        Age = 20
            //    };
            //    Employee E1 = new Employee
            //    {
            //        EmployeeId = 0,

            //        EmployeeNavigation = P1,
            //        JoinDate = "4-4",
            //        Salary = 1234,
            //        Spectialization = "none",
            //        Status = "Available"
            //    };
            //    _context.Add(E1);
            //}
            if (ModelState.IsValid)
            {
                try {
                    String DefaultPassword = "Pakistan3@";
                    employee.EmployeeNavigation.PersonNavigation.Password = DefaultPassword;
                    employee.JoinDate = DateTime.Now.ToShortDateString();
                    if (employee.EmployeeNavigation.PersonNavigation.Role != "Doctor")
                    {
                        employee.Spectialization = "none";
                    }
                    else {
                       var DoctorSpecilization= _context.DoctorSpecializationList.Where(a => a.Text == employee.Spectialization);
                        if (DoctorSpecilization.Count()==0) {
                            DoctorSpecializationList DSL = new DoctorSpecializationList
                            {
                                Text = employee.Spectialization
                            };
                            _context.DoctorSpecializationList.Add(DSL);
                        }
                    }
                 
                    _context.Add(employee);

                    await _context.SaveChangesAsync();


                    var user = new ApplicationUser { Id = employee.EmployeeId.ToString(), UserName = employee.EmployeeId.ToString() };
                    var result = await _userManager.CreateAsync(user, DefaultPassword);
                    await _userManager.AddToRoleAsync(user, employee.EmployeeNavigation.PersonNavigation.Role);
                    return RedirectToAction("Index");
                }
                catch(Exception e)
                {
                    ViewBag.E = e;
                    return View(employee);
                }

            }
            else
            {
                //ViewData["EmployeeId"] = new SelectList(_context.Person, "PersonId", "Address", employee.EmployeeId);
                return View(employee);
            }



        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Employee employee = await _context.Employee.SingleOrDefaultAsync(m => m.EmployeeId == id);
                Person person = await _context.Person.SingleOrDefaultAsync(m => m.PersonId == employee.EmployeeId);
                UserAccount UA = await _context.UserAccount.SingleOrDefaultAsync(m => m.UserAccountId == id);


                employee.EmployeeNavigation.PersonNavigation = UA;
                employee.EmployeeNavigation = person;
                return View(employee);
            }
            catch
            {
                return View();
            }
            //if (employee == null)
            //{
            //    return NotFound();
            //}
            //ViewData["EmployeeId"] = new SelectList(_context.Person, "PersonId", "Address", employee.EmployeeId);
           
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,JoinDate,Salary,Spectialization,Status,EmployeeNavigation")] Employee employee)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                  
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            ViewData["EmployeeId"] = new SelectList(_context.Person, "PersonId", "Address", employee.EmployeeId);
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.EmployeeNavigation)
                .SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)

        {
            ////Deletion for oidentity tables s
            //var user = await _userManager.FindByIdAsync(id.ToString());

            IdentityResult rc = new IdentityResult();

            if ((_userManager != null))
            {
                var user = await _userManager.FindByNameAsync(id.ToString());
                var logins = user.Logins;
                var rolesForUser = await _userManager.GetRolesAsync(user);

                using (var transaction = _context.Database.BeginTransaction())
                {
                    foreach (var login in logins.ToList())
                    {
                        await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                    }

                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            // item should be the name of the role
                            var result = await _userManager.RemoveFromRoleAsync(user, item);
                        }
                    }
                    rc = await _userManager.DeleteAsync(user);
                    transaction.Commit();
                }
            }
            /////End Identity Deletion

            

            var employee = await _context.Employee.SingleOrDefaultAsync(m => m.EmployeeId == id);
            Person person = await _context.Person.FirstOrDefaultAsync(p => p.PersonId == employee.EmployeeId);
            UserAccount UA = await _context.UserAccount.FirstOrDefaultAsync(u => u.UserAccountId == person.PersonId);
           
            _context.Employee.Remove(employee);
            _context.Person.Remove(person);
            _context.UserAccount.Remove(UA);
           
            _context.SaveChanges();
            if (employee.EmployeeNavigation.PersonNavigation.Role == "Doctor")
            {
               var employeeResult =_context.Employee.Where(e => e.Spectialization == employee.Spectialization);
                if (employeeResult.Count() == 0) {
                   var SpecializationListResult= _context.DoctorSpecializationList.FirstOrDefault(s => s.Text == employee.Spectialization);

                    _context.DoctorSpecializationList.Remove(SpecializationListResult);
                    _context.SaveChanges();

                }
            }
            return RedirectToAction("Index");
        }

        private bool EmployeeExists(int id)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
    }
}
