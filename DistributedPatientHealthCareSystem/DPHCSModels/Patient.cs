using System;
using System.Collections.Generic;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class Patient
    {
        public Patient()
        {
            Appointment = new HashSet<Appointment>();
            PatientHealthRecord = new HashSet<PatientHealthRecord>();
        }

        public int PatientId { get; set; }
        public string RegDate { get; set; }

        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<PatientHealthRecord> PatientHealthRecord { get; set; }
        public virtual Person PatientNavigation { get; set; }
    }
}
