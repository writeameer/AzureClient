using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using RestSharp;

namespace AzureClient
{
    partial class ServiceManagementClient
    {
        public T Execute<T>(RestRequest request) where T : new()
        {
            // Create HTTP header with API version
            var apiVersion = new RestSharp.Parameter
            {
                Name = "x-ms-version",
                Value = "2011-10-01",
                Type = ParameterType.HttpHeader
            };

            // Set base url for request and add auth headers
            var client = new RestClient
            {
                BaseUrl = _baseUrl,
                ClientCertificates = new X509CertificateCollection { _clientCert }
            };

            request.AddParameter(apiVersion);

            // Make Request 
            var response = client.Execute<T>(request);
            Console.WriteLine(response.Content);
            return response.Data;
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
            var client = new RestClient
            {
                BaseUrl = _baseUrl,
                ClientCertificates = new X509CertificateCollection { _clientCert }
            };

            request.AddParameter(apiVersion);

            // Make Request 
            var response = client.Execute<List<T>>(request);
            Console.WriteLine(response.Content);
            return response.Data;
        }

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
        public string GetResponse(WebResponse response)
        {
            var stream = response.GetResponseStream();
            return new StreamReader(stream).ReadToEnd();
        }


    }
}
