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