//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApp
{
    using System;
    using System.Collections.Generic;
    
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
