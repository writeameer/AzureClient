using System;
using System.Text;

namespace AzureClient.ServiceRequests
{
    public class CreateDeploymentRequest
    {
        private string _label;

        public string Label
        {
            get { return _label; }
            set
            {
                var encoder = new ASCIIEncoding();
                var stringInBytes = encoder.GetBytes(value);
                _label = Convert.ToBase64String(stringInBytes);
            }
        }


        public string DeploymentName { get; set; }
        public string PackageUrlInBlobStorage { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Configuration { get; set; }
        public bool StartDeployment { get; set; }
        public bool TreatWarningsAsError { get; set; }
        public override string ToString()
        {
            var requestBody = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <CreateDeployment xmlns=""http://schemas.microsoft.com/windowsazure"">
                  <Name>#deploymentname#</Name>
                  <PackageUrl>#packageurlinblobstorage#</PackageUrl>
                  <Label>#label#</Label>
                  <Configuration>#configuration#</Configuration>
                  <StartDeployment>#startdeployment#</StartDeployment>
                  <TreatWarningsAsError>#treatwarningsaserror#</TreatWarningsAsError>
                </CreateDeployment>
            ";
            requestBody = requestBody.Replace("#deploymentname#", DeploymentName);
            requestBody = requestBody.Replace("#packageurlinblobstorage#", PackageUrlInBlobStorage);
            requestBody = requestBody.Replace("#label#", Label);
            requestBody = requestBody.Replace("#configuration#", Configuration);
            requestBody = requestBody.Replace("#startdeployment#", StartDeployment.ToString().ToLower());
            requestBody = requestBody.Replace("#treatwarningsaserror#", TreatWarningsAsError.ToString().ToLower());
            requestBody = requestBody.Replace("                ", "");
            requestBody = requestBody.Replace("\n\n", "");
            requestBody = requestBody.Replace("\n<?", "<?");
            //requestBody = requestBody.Replace("\r\n", "");
            return requestBody;
        }
    }
}