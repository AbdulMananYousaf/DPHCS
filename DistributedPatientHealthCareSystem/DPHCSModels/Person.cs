using System;
using System.Collections.Generic;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Cnic { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string DoB { get; set; }
        public string ProfilePic { get; set; }
        public int? Age { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual UserAccount PersonNavigation { get; set; }
    }
}
