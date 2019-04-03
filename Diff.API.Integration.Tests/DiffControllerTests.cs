using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Diff.API.Integration.Tests
{
    [TestClass]
    public class DiffControllerTests
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private Guid _guid;

        public DiffControllerTests()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _guid = Guid.NewGuid();
        }

        [TestMethod]
        public async Task Should_ReturnSuccessfulAnalysis_When_PostingDistinctDiffArguments()
        {
            var client = _factory.CreateClient();

            var leftContent = "ew0KICAgICJuYW1lIjogIkpvaG4iLA0KICAgICJhZ2UiOiAzNSwNCiAgICAiY2FyIjogIlZvbHZvIg0KfQ==";
            var rightContent = "ew0KICAgICJuYW1lIjogIkpvaG4iLA0KICAgICJhZ2UiOiAzNSwNCiAgICAiY2FyIjogIlZvbHZvIg0KfQ==";

            var leftResponse = await PostLeftDiffArgument(client, _guid, leftContent);
            var rightResponse = await PostRightDiffArgument(client, _guid, rightContent);

            Assert.IsTrue(leftResponse.StatusCode == HttpStatusCode.Accepted);
            Assert.IsTrue(rightResponse.StatusCode == HttpStatusCode.Accepted);

            await Task.Delay(1000);

            var analysis = await GetDiffAnalysis(client, _guid);

            Assert.IsTrue(analysis.Id == _guid);
            Assert.IsTrue(analysis.AreEqual);
        }

        [TestMethod]
        public async Task Should_ReturnNotFound_When_RetrievingInexistentDiffAnalysis()
        {
            var client = _factory.CreateClient();

            var endpoint = $"/v1/diff/{_guid}";

            var response = await client.GetAsync(endpoint);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }

        private async Task<HttpResponseMessage> PostLeftDiffArgument(HttpClient client, Guid id, string content)
        {
            return await PostDiffArgument(client, id, content, "left");
        }

        private async Task<HttpResponseMessage> PostRightDiffArgument(HttpClient client, Guid id, string content)
        {
            return await PostDiffArgument(client, id, content, "right");
        }

        private async Task<HttpResponseMessage> PostDiffArgument(HttpClient client, Guid id, string content, string action)
        {
            var endpoint = $"/v1/diff/{id}/{action}";

            var response = await client.PostAsync(endpoint, new StringContent(content, Encoding.UTF8, "text/plain"));
            response.EnsureSuccessStatusCode();

            return response;
        }

        private async Task<DTOs.GetDiffAnalysisForResultDTO> GetDiffAnalysis(HttpClient client, Guid id)
        {
            var endpoint = $"/v1/diff/{id}";
            var response = await client.GetAsync(endpoint);

            var rawContent = await response.Content.ReadAsStringAsync();
            var analysis = JsonConvert.DeserializeObject<DTOs.GetDiffAnalysisForResultDTO>(rawContent);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.IsNotNull(analysis);

            return analysis;
        }
    }
}
