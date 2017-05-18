using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DistributedPatientHealthCareSystem.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DistributedPatientHealthCareSystem.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> roleManager=null;
        private UserManager<ApplicationUser> userManger=null;
        public RoleController(RoleManager<IdentityRole> _roleManager
                                ,UserManager<ApplicationUser> _userManger)
        {
            roleManager = _roleManager;
            userManger = _userManger;
        }
        // GET: /<controller>/
        public async  Task<IActionResult> Index()
        {

            

            IdentityRole b = new IdentityRole();
       
            IList<IdentityRole> RoleList = roleManager.Roles.ToList();

            ////
            IList<String> CountUserInRole = new List<String>();
            foreach (var item in RoleList) {
                var userList = await userManger.GetUsersInRoleAsync(item.Name);
                String count = userList.Count().ToString();
                CountUserInRole.Add(count);
                //if (countUsserInRole != null)
                //{
                //    String count= "User=" + countUsserInRole;

                //}
               
            }
            ViewBag.countList = CountUserInRole;

            return View(RoleList);
        }
        [HttpGet]
        public  IActionResult Create()
        {
           
            return View();


        }
        [HttpPost]
        public async Task<String> Create(String name)
        {
            if (name != null)
            {
                if (!await roleManager.RoleExistsAsync(name))
                {
                    var role = new IdentityRole(name);
                    await roleManager.CreateAsync(role);
                    return "Succesfully Added";
                }
                else
                {
                    return "Role Already Exist";
                }
            }
            return "Try Again";
        }

        public async Task<String> Delete(String id)
        {

            
            IdentityRole role = await roleManager.FindByIdAsync(id);
           
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return "Deleted";
                }
                else
                {
                    return "Not Deleted";
                }
            }
            else
            {
                return "Not Found";
            }

        }

        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                ViewBag.id = id;
                ViewBag.Name = role.Name;
                return View(role);
            }
            ViewBag.Count =0;
            
                return View("Index");
           
        }
        [HttpPost]
        public async Task<String> Edit(String id,String NewRoleName)
        {
            if (id!=null&&NewRoleName!=null)
            {
                IdentityRole role = await roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    role.Name = NewRoleName;
                    await roleManager.UpdateAsync(role);
                    //await roleManager.SetRoleNameAsync(role, NewRoleName);
                    return "Succesfully Edit";
                }
                else
                {
                    return "Not Found";
                }
            }
            return "Error! Empty Values";
            
           
        }


    }
}
