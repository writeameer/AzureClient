using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elastacloud.AzureManagement.Fluent;

namespace FluentAzure
{
    class Program
    {
        static void Main(string[] args)
        {
            var subscriptionManager = new SubscriptionManager("3402db5ef0ea46eaa151bb2e3e99789b");

            var hostedService = subscriptionManager.GetDeploymentManager()
                .AddCertificateFromStore("b4a9e657604dc8b89721f063a1b3ffe74d444b27");

            
        }
    }
}
