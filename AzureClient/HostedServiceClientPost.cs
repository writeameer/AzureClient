using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AzureClient.Models;
using RestSharp;
using System.Net;


namespace AzureClient
{
    public partial class HostedServiceClient
    {


        public string CreateHostedService(string serviceRequest)
        {
            // Create Web Request
            var request = (HttpWebRequest)WebRequest.Create(_baseUrl);
            request.Headers["x-ms-version"] = "2011-10-01";
            request.ContentType = "application/xml";
            request.Method = "POST";

            // Add client cert to request
            request.ClientCertificates.Add(_clientCert);

            //  Add content length to request
            var postData = Encoding.UTF8.GetBytes(serviceRequest);
            request.ContentLength = postData.Length;

            // Add post data to request
            var dataStream = request.GetRequestStream();
            dataStream.Write(postData, 0, postData.Length);
            dataStream.Close();

            // Make request
            var response = request.GetResponse();
            

            return GetResponse(response);
        }

        public string GetResponse(WebResponse response )
        {
            var stream = response.GetResponseStream();
            return new StreamReader(stream).ReadToEnd();
        }
    }
}
