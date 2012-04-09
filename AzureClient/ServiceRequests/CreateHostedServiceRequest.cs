using System;
using System.Text;

namespace AzureClient.ServiceRequests
{
    public class CreateHostedServiceRequest
    {
        private string _label;

        public string Label { 
            get { return _label; }
            set
            {
                var encoder = new ASCIIEncoding();
                var stringInBytes = encoder.GetBytes(value);
                _label = Convert.ToBase64String(stringInBytes);
            }
        }


        public string ServiceName { get; set; }
        
        public string Description { get; set; }
        public string Location { get; set; }
        public string AffinityGroup { get; set; }

        public override string ToString()
        {
            var requestBody=@"
            <?xml version=""1.0"" encoding=""utf-8""?>
            <CreateHostedService xmlns=""http://schemas.microsoft.com/windowsazure"">
              <ServiceName>#servicename#</ServiceName>
              <Label>#label#</Label>
              <Description>#description#</Description>
              <Location>#location#</Location>
              <!-- <AffinityGroup>#affinitygroup#</AffinityGroup> -->
            </CreateHostedService>
            ";
            requestBody = requestBody.Replace("#servicename#", ServiceName);
            requestBody = requestBody.Replace("#label#", Label);
            requestBody = requestBody.Replace("#description#", Description);
            requestBody = requestBody.Replace("#location#", Location);
            requestBody = requestBody.Replace("#affinitygroup#", AffinityGroup);
            requestBody = requestBody.Replace("            ", "");
            requestBody = requestBody.Replace("\n\n", "");
            requestBody = requestBody.Replace("\r\n", "");
            return requestBody;
        }
    }
}