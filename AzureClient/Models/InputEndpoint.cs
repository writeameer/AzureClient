using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureClient.Models
{
    public class InputEndpoint
    {
        public string RoleName { get; set; }
        public string Vip { get; set; }
        public string Port { get; set; }
    }
}
