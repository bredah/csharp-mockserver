using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MockServer
{
    public class Mock
    {
        public string BaseAddress { get; }

        public Mock(string baseAddress)
        {
            this.BaseAddress = baseAddress;
        }

        /// <summary>
        /// Remove all expectations
        /// See <a href="http://www.mock-server.com/mock_server/clearing_and_resetting.html">Clearing & Resetting</a>
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ResetAsync()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseAddress);
                return await httpClient.PutAsync($"/mockserver/reset", null);
            }
        }

        /// <summary>
        /// Remove a specific expectation
        /// See <a href="http://www.mock-server.com/mock_server/clearing_and_resetting.html">Clearing & Resetting</a>
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ClearAsync(string path)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseAddress);
                return await httpClient.PutAsync($"/mockserver/clear",
                              new StringContent(@"{'path': '" + path + "'}",
                                  encoding: Encoding.UTF8,
                                  mediaType: "application/json"));
            }
        }

        /// <summary>
        /// Create a expectation
        /// /// See <a href="http://www.mock-server.com/mock_server/creating_expectations.html">Creating Expectations</a>
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ExpectationAsync(string expectation)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseAddress);
                return await httpClient.PutAsync($"/mockserver/expectation",
                    new StringContent(expectation, encoding: Encoding.UTF8, mediaType: "application/json"));
            }
        }
    }
}
