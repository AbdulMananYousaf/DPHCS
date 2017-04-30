using System;
using System.Collections.Generic;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class TestBloodGroup
    {
        public int BloodTestTestId { get; set; }
        public int LabEmployeeId { get; set; }
        public string GeneratedDate { get; set; }
        public int HealthRecordId { get; set; }
        public string Result { get; set; }

        public virtual PatientHealthRecord HealthRecord { get; set; }
        public virtual Employee LabEmployee { get; set; }
    }
}
