using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;
using AzureClient;
using AzureClient.Models;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class HostedServicesTests
    {
        readonly NameValueCollection _appSettings = ConfigurationManager.AppSettings;
        private readonly string _subscriptionId;
        private readonly string _mgmtCert;

        public HostedServicesTests()
        {
            _subscriptionId = _appSettings["subscriptionid"];
            //_mgmtCert = _appSettings["MgmtCert"]
            _mgmtCert = System.IO.File.ReadAllText(@"C:\progs\secret\azurecert.txt");
        }
        [Test]
        public void GetServices()
        {
            var client = new HostedServiceClient(_mgmtCert, _subscriptionId);
            var hostedServices = client.GetHostedServices();

            foreach (var hostedService in hostedServices)
            {
                Console.WriteLine(hostedService.ServiceName + " " + hostedService.Url);
            }
        }

        [Test]
        public void CreateHostedService()
        {

            var client = new HostedServiceClient(_mgmtCert, _subscriptionId);


            var serviceRequest = new CreateHostedServiceRequest
            {
                ServiceName = "service-name",
                Label = "base64-encoded-service-label",
                Description = "This is a test",
                AffinityGroup = "Anywhere US"
            };


            var request = new CreateHostedServiceRequest();
            Console.WriteLine(serviceRequest.ToString());

            client.CreateHostedService(serviceRequest.ToString());
           


        }
    }
}
