using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using AzureClient;
using AzureClient.ServiceRequests;
using NUnit.Framework;
using AzureClient.Utils;

namespace Tests
{
    [TestFixture]
    public class ServiceMgmtApiTests
    {
        readonly NameValueCollection _appSettings = ConfigurationManager.AppSettings;
        private readonly string _subscriptionId;
        private readonly string _mgmtCert;
        private readonly ServiceManagementClient _managementClient;

        public ServiceMgmtApiTests()
        {
            // Setup stuff in constructor
            _subscriptionId = _appSettings["subscriptionid"];
            _mgmtCert = System.IO.File.ReadAllText(@"C:\progs\secret\azurecert.txt");
            _managementClient = new ServiceManagementClient(_mgmtCert, _subscriptionId);
        }
        [Test]
        public void GetServices()
        {
            var hostedServices = _managementClient.GetHostedServices();

            foreach (var hostedService in hostedServices)
                Console.WriteLine(hostedService.ServiceName + " " + hostedService.Url);
        }

        [Test]
        public void ExportCertForFiddler()
        {
            var cert = new X509Certificate2(Convert.FromBase64String(_mgmtCert), "", X509KeyStorageFlags.Exportable);
            var certData = cert.Export(X509ContentType.Cert);
            var userProfile = Environment.GetEnvironmentVariable("userprofile");
            var fileName = String.Format(@"{0}\documents\Fiddler2\ClientCertificate.cer", userProfile);
            Console.WriteLine(fileName);
            
            File.WriteAllBytes(fileName, certData);
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
                Description = "affinity-group-name",
                Label = "affinity-group-name",
                Name = "affinity-group-name",
                Location = "Southeast Asia"
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
                ServiceName = "ameeristesting2",
                Label = "ameeristesting2",
                Description = "This is a test",
                //AffinityGroup = "aad4283b-921a-43e2-8566-5a3914396ba5",
                Location = "Southeast Asia"
            }.ToString();

            var response = _managementClient.ExecutePost("services/hostedservices", xmlRequest);
            Console.WriteLine(response);
        }

        [Test]
        public void GetDeployment()
        {
            var deployment = _managementClient.GetDeployment("cloudomanapi", "production");
            Console.WriteLine(deployment.Name);

            foreach(var roleInstance in deployment.RoleList)
            {
                Console.WriteLine(roleInstance.OsVersion + " " + roleInstance.RoleName);
            }
        }

        [Test]
        public void GetHostedServiceProperties()
        {
            var hostedService = _managementClient.GetHostedServiceProperties("cloudomanapi",true);
            Console.WriteLine(hostedService.ServiceName);
            foreach (var deployment in hostedService.Deployments)
            {
                Console.WriteLine(deployment.Name);
            }
        }

        [Test]
        public void CreateDeployment()
        {
            var base64ConfigFile = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <ServiceConfiguration serviceName=""EmptyWorker"" xmlns=""http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration"" osFamily=""2"" osVersion=""*"">
                  <Role name=""EmptyWorkerRole"">
                    <Instances count=""1"" />
                    <ConfigurationSettings>
                      <Setting name=""Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"" value=""UseDevelopmentStorage=true"" />
                      <Setting name=""Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled"" value=""true"" />
                      <Setting name=""Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername"" value=""admin"" />
                      <Setting name=""Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword"" value=""MIIBnQYJKoZIhvcNAQcDoIIBjjCCAYoCAQAxggFOMIIBSgIBADAyMB4xHDAaBgNVBAMME1dpbmRvd3MgQXp1cmUgVG9vbHMCEHqfmBYoV4uxS2eIuf6f1uIwDQYJKoZIhvcNAQEBBQAEggEAN4HgeWCTgX8J+7PdzaYEDcN4a5occpX3ph7WsuxHAm0TosAb2Dx7rYy+KfkxBJbKAtjT39xB5Ik5PCndZWMw5WDIAGmjIU5y7w7oVulrH/jAgjlUxkuvNJWVVt3kq92PGvr8Buches8ca5lhksIhlo7UnneQ6N/LxjEDJuqoE+lxxWrDnEFiHABPgmeHMty2dsrXmi4N6cNMDRZZX+383YbLzLuWOFDasvwH/kz5IxgYQJEQsA5udmLPRSDrWKGqR/lwAGy0XyB4vsV76RoGiAD7w039pIIOoGW0PibDovBc567kUsXHYMvE5mRdEG2zp0XMpcQwUjsM2H0f7hrG0jAzBgkqhkiG9w0BBwEwFAYIKoZIhvcNAwcECGiZM1bE0hxTgBC6GGkWZe5xDg2dvBnPnGD3"" />
                      <Setting name=""Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration"" value=""2013-04-09T23:59:59.0000000+10:00"" />
                      <Setting name=""Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled"" value=""true"" />
                    </ConfigurationSettings>
                    <Certificates>
                      <Certificate name=""Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption"" thumbprint=""B53E2ADE5F1626B33A79CC10D57AC5159B2F6374"" thumbprintAlgorithm=""sha1"" />
                    </Certificates>
                  </Role>
                </ServiceConfiguration>
            ".ToBase64();



            var name = "AmeerDeploymentFromApi";
            var xmlRequest = new CreateDeploymentRequest {
                DeploymentName  = name,
                PackageUrlInBlobStorage = "https://ameerdeen.blob.core.windows.net/public/EmptyWorker.cspkg",
                Label = name,
                Configuration   = base64ConfigFile,
                StartDeployment = true,
                TreatWarningsAsError = false
            }.ToString();

            var serviceName = "ameeristesting2";
            var deploymentSlotName = "production";
            var resourceName = "services/hostedservices/" + serviceName + "/deploymentslots/" + deploymentSlotName;

            var response = _managementClient.ExecutePost(resourceName, xmlRequest);
            Console.WriteLine(xmlRequest);
            Console.WriteLine(response);

        }
    }
}
