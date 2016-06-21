using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ComputerVisionAPIv1.Infrastructure.Contracts;
using ComputerVisionAPIv1.Infrastructure.Models;
using System.Threading.Tasks;

namespace ComputerVisionAPIv1.Infrastructure
{
    public class CognitiveService : ICognitiveService
    {
        private string uri;
        private string subscriptionKey;
        private string contentType;

        public CognitiveService(string url, string subscriptionKey, string contentType)
        {
            this.uri = ($"{url}visualFeatures=Tags");
            this.subscriptionKey = subscriptionKey;
            this.contentType = contentType;
        }

        public async Task<Image> ProcessImage(string imageUrl)
        {
            // Instantiate a HTTP Client
            var client = new HttpClient();

            // Pass subscription key thru the HTTP Request Header
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Format Request body
            byte[] byteData = Encoding.UTF8.GetBytes($"{{\"url\": \"{imageUrl}\"}}");

            using (var content = new ByteArrayContent(byteData))
            {
                // Specify Request body Content-Type
                content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                // Send Post Request
                HttpResponseMessage response = await client.PostAsync(uri, content);

                // Read Response body into the image model
                return await response.Content.ReadAsAsync<Image>();
            }

        }
    }
}
