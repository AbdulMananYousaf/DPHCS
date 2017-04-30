using System;
using System.Collections.Generic;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class PatientAllergie
    {
        public int AllergyId { get; set; }
        public string Discription { get; set; }
        public int PatientHealthRecordId { get; set; }
        public string Name { get; set; }

        public virtual PatientHealthRecord PatientHealthRecord { get; set; }
    }
}
