using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AzureClient.Models;
using RestSharp;

namespace AzureClient
{
    public partial class ServiceManagementClient
    {

        private static string _subscriptionId;
        private static string _baseUrl;
        private static X509Certificate2 _clientCert;

        //readonly string _managementCertificate = AppSettings["MgmtCert"];

        public ServiceManagementClient(string mgmtCert, string subscriptionId, string mgmtUrl = "https://management.core.windows.net")
        {
            _subscriptionId = subscriptionId;

            // Build ApiUrl
            _baseUrl = String.Format("{0}/{1}/", mgmtUrl, _subscriptionId);

            // Decode Client Certificate
            _clientCert = new X509Certificate2(Convert.FromBase64String(mgmtCert));

        }
        public List<HostedService> GetHostedServices()
        {
            var request = new RestRequest{
                Resource = "services/hostedservices"
            };

            return ExecuteList<HostedService>(request);
        }

        public List<Location> ListLocations()
        {
            var request = new RestRequest
            {
                Resource = "locations"
            };

            return ExecuteList<Location>(request);
        }

        public List<AffinityGroup> ListAffinityGroups()
        {
            var request = new RestRequest
            {
                Resource = "affinitygroups"
            };

            return ExecuteList<AffinityGroup>(request);
        }

        public Deployment GetDeployment(string serviceName, string deploymentSlot)
        {
            var resourceName = string.Format("/services/hostedservices/{0}/deploymentslots/{1}", serviceName, deploymentSlot);
            var request = new RestRequest { Resource = resourceName };
            return Execute<Deployment>(request);
        }
    }
}
