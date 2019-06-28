using MockServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ApiServer.Tests
{
    public class BaseMock : IDisposable
    {
        public Mock Mock { get; }

        public BaseMock()
        {
            Mock = new Mock(BaseUrl);
        }

        public async void Dispose()
        {
            await Mock.ResetAsync();
        }

        /// <summary>
        /// MockServer base URL 
        /// </summary>
        public static string BaseUrl
        {
            get
            {
                return "http://localhost:1080";
            }
        }

        /// <summary>
        /// Return the path of the current folder
        /// </summary>
        public static string RootPath
        {
            get
            {
                return Directory.GetCurrentDirectory();
            }
        }


        /// <summary>
        /// Retrive the path from JSON
        /// </summary>
        /// <param name="json">JSON content</param>
        /// <returns>The current path</returns>
        public static string MockPath(string json)
        {
            JObject jsonContent = JsonConvert.DeserializeObject(json) as JObject;
            JToken jsonRequestPath = jsonContent.SelectToken("httpRequest.path");
            return jsonRequestPath.ToString();
        }

        /// <summary>
        /// Setup the mock session into JSON
        /// </summary>
        /// <param name="json">JSON content</param>
        /// <param name="mockSession">Session to be defined</param>
        /// <returns></returns>
        public static string MockSession(string json, string mockSession)
        {
            JObject jsonContent = JsonConvert.DeserializeObject(json) as JObject;
            JToken jsonSession = jsonContent.SelectToken("httpRequest.cookies.mockSession");
            jsonSession.Replace(mockSession);
            return JsonConvert.SerializeObject(jsonContent, Formatting.Indented);
        }
    }
}
