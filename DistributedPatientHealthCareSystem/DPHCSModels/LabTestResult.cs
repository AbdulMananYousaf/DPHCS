using System;
using System.Collections.Generic;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class LabTestResult
    {
        public int TestResultId { get; set; }
        public int PatientHealthRecordId { get; set; }
        public int PatientId { get; set; }
        public int LabTestOrderId { get; set; }
        public string TestTableName { get; set; }
        public int TestTableRow { get; set; }

        public virtual LabTestOrder LabTestOrder { get; set; }
        public virtual PatientHealthRecord PatientHealthRecord { get; set; }
    }
}
