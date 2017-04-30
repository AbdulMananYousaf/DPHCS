using System;
using System.Collections.Generic;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class PatientHealthRecord
    {
        public PatientHealthRecord()
        {
            LabTestOrder = new HashSet<LabTestOrder>();
            LabTestResult = new HashSet<LabTestResult>();
            PatientAllergie = new HashSet<PatientAllergie>();
            PatientPrescription = new HashSet<PatientPrescription>();
            PatientVital = new HashSet<PatientVital>();
            TestBloodGroup = new HashSet<TestBloodGroup>();
        }

        public int HealthRecordId { get; set; }
        public int DoctorEmployeeId { get; set; }
        public int HospitalId { get; set; }
        public int PatientId { get; set; }

        public virtual ICollection<LabTestOrder> LabTestOrder { get; set; }
        public virtual ICollection<LabTestResult> LabTestResult { get; set; }
        public virtual ICollection<PatientAllergie> PatientAllergie { get; set; }
        public virtual ICollection<PatientPrescription> PatientPrescription { get; set; }
        public virtual ICollection<PatientVital> PatientVital { get; set; }
        public virtual ICollection<TestBloodGroup> TestBloodGroup { get; set; }
        public virtual Employee DoctorEmployee { get; set; }
        public virtual Hospital Hospital { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
