using BackEnd_GestaoFinanceira.Interfaces;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackEnd_GestaoFinanceira.Repositories
{
    public class ComputerVisionRepository : IComputerVisionRepository
    {
        // <snippet_vars>
        // Add your Computer Vision subscription key and endpoint
        static string subscriptionKey = "2c4a93d237734fb0bdcf14e4c180eae1";
        static string endpoint = "https://senai.cognitiveservices.azure.com/";
        // </snippet_vars>
	// </snippet_using_and_vars>



        public ReadOperationResult ReadFile(string url)
        {
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            return ReadFileUrl(client, url).Result;
        }



        // <snippet_auth>
        /*
         * AUTHENTICATE
         * Creates a Computer Vision client used by each example.
         */
        private static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }
        // </snippet_auth>
        /*
         * END - Authenticate
         */

        // <snippet_readfileurl_1>
        /*
         * READ FILE - URL 
         * Extracts text. 
         */
        private static async Task<ReadOperationResult> ReadFileUrl(ComputerVisionClient client, string urlFile)
        {
            // Read text from URL
            var textHeaders = await client.ReadAsync(urlFile);
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);
            // </snippet_readfileurl_1>
		
            // <snippet_readfileurl_2>
            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));
            // </snippet_readfileurl_2>

            return results;
        }
    }
}
