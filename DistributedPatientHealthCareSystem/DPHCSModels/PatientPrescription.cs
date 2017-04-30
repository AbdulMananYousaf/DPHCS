using System;
using System.Collections.Generic;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class PatientPrescription
    {
        public int PrescriptionId { get; set; }
        public int PatientHealthRecordId { get; set; }
        public string MedicineName { get; set; }
        public string Quantity { get; set; }
        public string UsageDirections { get; set; }

        public virtual PatientHealthRecord PatientHealthRecord { get; set; }
    }
}
