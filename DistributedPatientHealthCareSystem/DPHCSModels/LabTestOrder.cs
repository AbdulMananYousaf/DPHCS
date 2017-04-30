using System;
using System.Collections.Generic;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class LabTestOrder
    {
        public LabTestOrder()
        {
            LabTestResult = new HashSet<LabTestResult>();
        }

        public int LabTestOrderId { get; set; }
        public string Discription { get; set; }
        public int PatientHealthRecordId { get; set; }
        public string Status { get; set; }
        public string TestTableName { get; set; }

        public virtual ICollection<LabTestResult> LabTestResult { get; set; }
        public virtual PatientHealthRecord PatientHealthRecord { get; set; }
    }
}
