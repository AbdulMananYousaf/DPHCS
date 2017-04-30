using System;
using System.Collections.Generic;

namespace DistributedPatientHealthCareSystem.DPHCSModels
{
    public partial class UserAccount
    {
        public int UserAccountId { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public virtual Person Person { get; set; }
    }
}
