using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureClient.Utils
{
    public static class StringExtensions
    {
        public static string ToBase64(this string myString)
        {
            var toEncodeAsBytes = Encoding.ASCII.GetBytes(myString);
            return  Convert.ToBase64String(toEncodeAsBytes);
        }
    }
}
