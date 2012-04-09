using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureClient.Models
{
    public class RoleInstance
    {
        public string RoleName { get; set; }
        public string InstanceName { get; set; }
        public string InstanceStatus { get; set; }
        public string InstanceUpgradeDomain  { get; set; }
        public string InstanceFaultDomain{ get; set; }
        public string InstanceSize { get; set; }
        public string InstanceStateDetails { get; set; }
        public string InstanceErrorCode { get; set; }
    }
}
