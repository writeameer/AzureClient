using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureClient.Models
{
    public class UpgradeStatus
    {
        public string UpgradeType { get; set; }
        public string CurrentUpgradeDomainState { get; set; }
        public string CurrentUpgradeDomain { get; set; }
    }
}
