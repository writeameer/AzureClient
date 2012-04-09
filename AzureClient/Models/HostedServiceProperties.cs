using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureClient.Models
{
    public class HostedServiceProperties
    {
        public string Description { get; set; }
        public string Location { get; set; }
        public string AffinityGroup { get; set; }
        public string Label { get; set; }
    }
}
