using System;
using System.Collections.Generic;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class Hospital
    {
        public Hospital()
        {
            PatientHealthRecord = new HashSet<PatientHealthRecord>();
        }

        public int HospitalId { get; set; }
        public string Address { get; set; }
        public string Api { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string WebSite { get; set; }

        public virtual ICollection<PatientHealthRecord> PatientHealthRecord { get; set; }
    }
}
