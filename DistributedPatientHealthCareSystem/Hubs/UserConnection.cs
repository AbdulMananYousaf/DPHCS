using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedPatientHealthCareSystem.Hubs
{
    public class UserConnection
    {
        [Key]
        public string UserName { set; get; }
        public string ConnectionID { set; get; }
    }
}
