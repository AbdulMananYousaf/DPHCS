using System;
using System.Collections.Generic;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class PatientVital
    {
        public int VitalId { get; set; }
        public int PatientHealthRecordId { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }

        public virtual PatientHealthRecord PatientHealthRecord { get; set; }
    }
}
