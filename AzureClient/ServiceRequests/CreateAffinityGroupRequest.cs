using System;
using System.Text;

namespace AzureClient.ServiceRequests
{
    public class CreateAffinityGroupRequest
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


        public string Name { get; set; }
        public string Location { get; set; }
        public string Description{ get; set; }

        public override string ToString()
        {
            var requestBody = @"
                <?xml version=""1.0"" encoding=""utf-8""?>
                <CreateAffinityGroup  xmlns=""http://schemas.microsoft.com/windowsazure"">
      <Name>#name#</Name>
      <Label>#label#</Label>
      <Description>#description#</Description>
      <Location>#location#</Location>   
      </CreateAffinityGroup>
            ";
            requestBody = requestBody.Replace("#name#", Name);
            requestBody = requestBody.Replace("#label#", Label);
            requestBody = requestBody.Replace("#description#", Description);
            requestBody = requestBody.Replace("#location#", Location);
            requestBody = requestBody.Replace("                ", "");
            requestBody = requestBody.Replace("\n\n", "");
            requestBody = requestBody.Replace("\n<?", "<?");
            //requestBody = requestBody.Replace("\r\n", "");
            return requestBody;
        }
    }
}