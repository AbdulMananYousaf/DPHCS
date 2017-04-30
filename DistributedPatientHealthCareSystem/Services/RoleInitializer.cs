using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedPatientHealthCareSystem.Services
{
    public static class RoleInitializer
    {
        public static async Task Initialize(RoleManager<IdentityRole> roleManager) {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole("Admin");
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("Doctor"))
            {
                var role = new IdentityRole("Doctor");
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("Receptionist"))
            {
                var role = new IdentityRole("Receptionist");
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("Laboratory Technician"))
            {
                var role = new IdentityRole("Laboratory Technician");
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("Patient"))
            {
                var role = new IdentityRole("Patient");
                await roleManager.CreateAsync(role);
            }
        }
    }
}
