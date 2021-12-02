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

        // Download these images (link in prerequisites), or you can use any appropriate image on your local machine.
        private const string READ_TEXT_LOCAL_IMAGE = "printed_text.jpg";

        // <snippet_readtext_url>
        private const string READ_TEXT_URL_IMAGE = "https://intelligentkioskstore.blob.core.windows.net/visionapi/suggestedphotos/3.png";
        // </snippet_readtext_url>

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
        public static ComputerVisionClient Authenticate(string endpoint, string key)
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
        public static async Task<ReadOperationResult> ReadFileUrl(ComputerVisionClient client, string urlFile)
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
        // </snippet_readfileurl_3>

        /*
         * END - READ FILE - URL
         */


        // <snippet_read_local>
        /*
         * READ FILE - LOCAL
         */

        public static async Task ReadFileLocal(ComputerVisionClient client, string localFile)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("READ FILE FROM LOCAL");
            Console.WriteLine();

            // Read text from URL
            var textHeaders = await client.ReadInStreamAsync(File.OpenRead(localFile));
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);

            // <snippet_extract_response>
            // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            Console.WriteLine($"Reading text from local file {Path.GetFileName(localFile)}...");
            Console.WriteLine();
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));
            // </snippet_extract_response>

            // <snippet_extract_display>
            // Display the found text.
            Console.WriteLine();
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    Console.WriteLine(line.Text);
                }
            }
            Console.WriteLine();
        }
    }
}
