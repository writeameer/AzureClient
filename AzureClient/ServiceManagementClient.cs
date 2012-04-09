using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
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

        public List<T> ExecuteList<T>(RestRequest request) where T : new()
        {
            // Create HTTP header with API version
            var apiVersion = new RestSharp.Parameter
            {
                Name = "x-ms-version",
                Value = "2011-10-01",
                Type = ParameterType.HttpHeader
            };

            // Set base url for request and add auth headers
            var client = new RestClient{
                BaseUrl = _baseUrl,
                ClientCertificates = new X509CertificateCollection {_clientCert}
            };

            request.AddParameter(apiVersion);
            
            // Make Request 
            var response = client.Execute<List<T>>(request);
            Console.WriteLine(response.Content);
            return response.Data;
        }

        public string GetResponse(WebResponse response)
        {
            var stream = response.GetResponseStream();
            return new StreamReader(stream).ReadToEnd();
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
    }
}
