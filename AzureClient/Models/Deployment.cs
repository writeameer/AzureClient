using System.Collections.Generic;

namespace AzureClient.Models
{
    public class Deployment
    {
        public string Name { get; set; }
        public string DeploymentSlot { get; set; }
        public string PrivateId { get; set; }
        public string Status { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public string Configuration { get; set; }
        public List<RoleInstance> RoleInstanceList { get; set; }
        public UpgradeStatus UpgradeStatus { get; set; }
        public int UpgradeDomainCount { get; set; }
        public List<Role> RoleList { get; set; }
        public string SdkVersion { get; set; }
        public List<InputEndpoint> InputEndpointList { get; set; }
        public bool Locked { get; set; }
        public bool RollbackAllowed { get; set; }
    }
}
