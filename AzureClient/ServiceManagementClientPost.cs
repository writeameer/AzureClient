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
    public partial class ServiceManagementClient
    {

        public string ExecutePost(string resource, string xmlRequest)
        {
            // Create Web Request
            var requestUri = _baseUrl + "services/hostedservices";
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Headers["x-ms-version"] = "2011-10-01";
            request.ContentType = "application/xml";
            request.Method = "POST";

            // Add client cert to request
            request.ClientCertificates.Add(_clientCert);

            //  Add content length to request
            var postData = Encoding.UTF8.GetBytes(xmlRequest);
            request.ContentLength = postData.Length;

            // Add post data to request
            var dataStream = request.GetRequestStream();
            dataStream.Write(postData, 0, postData.Length);
            dataStream.Close();

            // Make request
            WebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException webException)
            {
                response = (HttpWebResponse)webException.Response;
            }

            var responseText = GetResponse(response);

            return responseText;
        }

    }
}
