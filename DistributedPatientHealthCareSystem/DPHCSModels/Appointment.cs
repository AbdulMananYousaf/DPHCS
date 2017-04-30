using System;
using System.Collections.Generic;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class Appointment
    {
        public int AppointmentId { get; set; }
        public string AppointmentStatus { get; set; }
        public string Discription { get; set; }
        public int DoctorEmployeeId { get; set; }
        public string HeldDate { get; set; }
        public int PatientId { get; set; }
        public int ReceptionistEmployeetId { get; set; }
        public string SetedDate { get; set; }
        public string VisitReson { get; set; }
        public string PatientLastVisitDate { get; set; }

        public virtual Employee DoctorEmployee { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Employee ReceptionistEmployeet { get; set; }
    }
}
