using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class Employee
    {
        public Employee()
        {
            AppointmentDoctorEmployee = new HashSet<Appointment>();
            AppointmentReceptionistEmployeet = new HashSet<Appointment>();
            PatientHealthRecord = new HashSet<PatientHealthRecord>();
            TestBloodGroup = new HashSet<TestBloodGroup>();
        }
       
        public int EmployeeId { get; set; }
        public string JoinDate { get; set; }
       
        [Required]
        public int? Salary { get; set; }
        public string Spectialization { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Appointment> AppointmentDoctorEmployee { get; set; }
        public virtual ICollection<Appointment> AppointmentReceptionistEmployeet { get; set; }
        public virtual ICollection<PatientHealthRecord> PatientHealthRecord { get; set; }
        public virtual ICollection<TestBloodGroup> TestBloodGroup { get; set; }
        public virtual Person EmployeeNavigation { get; set; }
    }
}
