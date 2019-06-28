using MockServer;
using MockServer.Tests.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiServer.Tests
{
    public class MockTest : IClassFixture<BaseMock>
    {
        public Mock Mock { get; }

        private readonly string MockSessionId;

        public MockTest(BaseMock baseMock)
        {
            Mock = baseMock.Mock;
            MockSessionId = Guid.NewGuid().ToString();
        }

        [Fact]
        public async Task CreateAExpectationAsync_Get_UsingString()
        {
            // Submit the expectation
            var json = @"{'httpRequest' : {'path' : '/some/path'},'httpResponse' : {'body' : 'some_response_body'} }";
            var expectationResponse = await Mock.ExpectationAsync(json);
            Assert.Equal(HttpStatusCode.Created, expectationResponse.StatusCode);
            // Make the request
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Mock.BaseAddress);
                var response = await httpClient.GetAsync($"/some/path");
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task CreateAExpectationAsync_Get_UsingJson()
        {
            // Submit the expectation
            string json = File.ReadAllText($"{BaseMock.RootPath}/Mock/sampleGet.json");
            var expectationResponse = await Mock.ExpectationAsync(json);
            Assert.Equal(HttpStatusCode.Created, expectationResponse.StatusCode);
            // Make the request
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Mock.BaseAddress);
                using (var response = httpClient.GetAsync($"/api/product").Result)
                {
                    response.EnsureSuccessStatusCode();
                    var apiResponseContent = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<List<Product>>(apiResponseContent);
                    Assert.Equal(2, products.Count);
                }
            }
        }

        [Fact]
        public async Task CreateAExpectationAsync_Get_UniqueSession()
        {
            // Submit the expectation
            string json = File.ReadAllText($"{BaseMock.RootPath}/Mock/sampleSession.json");
            // Add unique session
            json = BaseMock.MockSession(json, MockSessionId);
            var expectationResponse = await Mock.ExpectationAsync(json);
            Assert.Equal(HttpStatusCode.Created, expectationResponse.StatusCode);
            // Make the request
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var httpClient = new HttpClient(handler))
            {
                httpClient.BaseAddress = new Uri(Mock.BaseAddress);
                cookieContainer.Add(new Uri(BaseMock.BaseUrl), new Cookie("mockSession", MockSessionId));
                using (var response = httpClient.GetAsync($"/api/product").Result)
                {
                    response.EnsureSuccessStatusCode();
                    var apiResponseContent = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<List<Product>>(apiResponseContent);
                    Assert.Single(products);
                }
            }
        }

        [Fact]
        public async Task CreateAExpectationAsync_Post_UsingJson()
        {
            // Submit the expectation
            string json = File.ReadAllText($"{BaseMock.RootPath}/Mock/samplePost.json");
            var expectationResponse = await Mock.ExpectationAsync(json);
            Assert.Equal(HttpStatusCode.Created, expectationResponse.StatusCode);

            var product = new Product
            {
                Id = 3,
                Description = "New Product",
                Value = 12.50
            };
            // Make the request
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Mock.BaseAddress);
                var response = await httpClient.PostAsync($"/api/product",
                    new StringContent(JsonConvert.SerializeObject(product),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json")
                );
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                var apiResponseContent = await response.Content.ReadAsStringAsync();
                Assert.Equal("done", apiResponseContent);
            }
        }

        [Fact]
        public async Task CreateAExpectationAsync_Put_UsingJson()
        {
            // Submit the expectation
            string json = File.ReadAllText($"{BaseMock.RootPath}/Mock/samplePut.json");
            var expectationResponse = await Mock.ExpectationAsync(json);
            Assert.Equal(HttpStatusCode.Created, expectationResponse.StatusCode);

            var product = new Product
            {
                Id = 3,
                Description = "New Product",
                Value = 12.50
            };
            // Make the request
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Mock.BaseAddress);
                var response = await httpClient.PutAsync($"/api/product",
                    new StringContent(JsonConvert.SerializeObject(product),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json")
                );
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                var apiResponseContent = await response.Content.ReadAsStringAsync();
                Assert.Equal("done", apiResponseContent);
            }
        }

        [Fact]
        public async Task CreateAExpectationAsync_Delete_UsingJson()
        {
            // Submit the expectation
            string json = File.ReadAllText($"{BaseMock.RootPath}/Mock/sampleDelete.json");
            var expectationResponse = await Mock.ExpectationAsync(json);
            Assert.Equal(HttpStatusCode.Created, expectationResponse.StatusCode);
            // Make the request
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Mock.BaseAddress);
                var response = await httpClient.DeleteAsync($"/api/product/3");
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var apiResponseContent = await response.Content.ReadAsStringAsync();
                Assert.Equal("Removed", apiResponseContent);
            }
        }

        [Fact]
        public async Task CreateAExpectationAsync_ClearAsync()
        {
            // Submit the expectation
            var json = @"{'httpRequest' : {'path' : '/some/path'},'httpResponse' : {'body' : 'some_response_body'} }";
            var expectationResponse = await Mock.ExpectationAsync(json);
            Assert.Equal(HttpStatusCode.Created, expectationResponse.StatusCode);
            // Make the request
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Mock.BaseAddress);
                var response = await httpClient.GetAsync($"/some/path");
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
            // Remove the current expectation
            expectationResponse = await Mock.ClearAsync(BaseMock.MockPath(json));
            Assert.Equal(HttpStatusCode.OK, expectationResponse.StatusCode);
            // Make the request
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Mock.BaseAddress);
                var response = await httpClient.GetAsync($"/some/path");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}
