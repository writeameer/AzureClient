using System.Collections.Generic;
using System.Linq;

namespace AzureClient.Models
{
    public class HostedService
    {
        public string Url { get; set; }
        public string ServiceName { get; set; }
        public HostedServiceProperties HostedServiceProperties { get; set; }
        public List<Deployment> Deployments { get; set; }
    }
}
