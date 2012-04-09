using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;
using AzureClient;
using AzureClient.Models;
using AzureClient.ServiceRequests;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class HostedServicesTests
    {
        readonly NameValueCollection _appSettings = ConfigurationManager.AppSettings;
        private readonly string _subscriptionId;
        private readonly string _mgmtCert;
        private readonly ServiceManagementClient _managementClient;

        public HostedServicesTests()
        {
            // Setup stuff in constructor

            //_mgmtCert = _appSettings["MgmtCert"]

            _subscriptionId = _appSettings["subscriptionid"];
            _mgmtCert = System.IO.File.ReadAllText(@"C:\progs\secret\azurecert.txt");
            _managementClient = new ServiceManagementClient(_mgmtCert, _subscriptionId);

        }
        [Test]
        public void GetServices()
        {
            var hostedServices = _managementClient.GetHostedServices();

            foreach (var hostedService in hostedServices)
            {
                Console.WriteLine(hostedService.ServiceName + " " + hostedService.Url);
            }
        }

        [Test]
        public void ExportCertForFiddler()
        {
            var cspParameters = new CspParameters {KeyContainerName = "KeyContainer"};
            var rsa = new RSACryptoServiceProvider(cspParameters);

            var cert = new X509Certificate2(Convert.FromBase64String(_mgmtCert), "", X509KeyStorageFlags.Exportable);
            
            var certData = cert.Export(X509ContentType.Cert);
            File.WriteAllBytes(@"c:\users\ameer\documents\Fiddler2\ClientCertificate.cer", certData);
        }

        [Test]
        public void ListLocations()
        {
            var locations = _managementClient.ListLocations();
            foreach (var location in locations)
            {
                Console.WriteLine(location.Name + "," + location.DisplayName);
            }
        }

        [Test]
        public void ListAffinityGroups()
        {
            var locations = _managementClient.ListAffinityGroups();
            foreach (var location in locations)
            {
                Console.WriteLine(location.Name + "," + location.Description);
            }
        }

        [Test]
        public void CreateAffinityGroup()
        {
            var affinityGroupRequest = new CreateAffinityGroupRequest {
                //Description = "affinity-group-name",
                Label = "affinity-group-name",
                Name = "affinity-group-name",
                Location = "Anywhere US"
            }.ToString();

            Console.WriteLine(affinityGroupRequest);
            var response = _managementClient.ExecutePost("affinitygroups", affinityGroupRequest);
            Console.WriteLine(response);
        }

        [Test]
        public void CreateHostedService()
        {


            var xmlRequest = new CreateHostedServiceRequest
            {
                ServiceName = "ameeristesting",
                Label = "base64-encoded-service-label",
                Description = "This is a test",
                //AffinityGroup = "Anywhere",
                Location = "Anywhere"
            }.ToString();

            var response = _managementClient.ExecutePost("services/hostedservices", xmlRequest);
            Console.WriteLine(response);


        }
    }
}
