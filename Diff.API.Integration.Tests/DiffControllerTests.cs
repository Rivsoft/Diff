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
        private WebApplicationFactory<Startup> _factory;
        private Guid _guid;

        [TestInitialize]
        public void TestInitialize()
        {
            _factory = new WebApplicationFactory<Startup>();
            _guid = Guid.NewGuid();
        }

        [TestMethod]
        public async Task Should_ReturnAreEqual_When_PostingEqualDiffArguments()
        {
            var client = _factory.CreateClient();

            var leftContent = "ew0KICAgICJuYW1lIjogIkpvaG4iLA0KICAgICJhZ2UiOiAzNSwNCiAgICAiY2FyIjogIlZvbHZvIg0KfQ==";
            var rightContent = "ew0KICAgICJuYW1lIjogIkpvaG4iLA0KICAgICJhZ2UiOiAzNSwNCiAgICAiY2FyIjogIlZvbHZvIg0KfQ==";

            var leftResponse = await PostLeftDiffArgument(client, _guid, leftContent);
            var rightResponse = await PostRightDiffArgument(client, _guid, rightContent);

            Assert.IsTrue(leftResponse.StatusCode == HttpStatusCode.Accepted);
            Assert.IsTrue(rightResponse.StatusCode == HttpStatusCode.Accepted);

            var analysis = await GetDiffAnalysis(client, _guid);

            Assert.IsTrue(analysis.Id == _guid);
            Assert.IsTrue(analysis.Analyzed);
            Assert.IsTrue(analysis.AreEqual);
            Assert.IsTrue(analysis.AreEqualSize);
        }

        [TestMethod]
        public async Task Should_ReturnAreEqualFalse_When_PostingDistinctDiffArguments()
        {
            var client = _factory.CreateClient();

            var leftContent = "ew0KICAgICJuYW1lIjogIkpvaG4iLA0KICAgICJhZ2UiOiAzNSwNCiAgICAiY2FyIjogIlZvbHZvIg0KfQ==";
            var rightContent = "ew0KICAgICJuYW1lIjogIkthcmwiLA0KICAgICJhZ2UiOiAzMiwNCiAgICAiY2FyIjogIk1hemRhIg0KfQ==";

            var leftResponse = await PostLeftDiffArgument(client, _guid, leftContent);
            var rightResponse = await PostRightDiffArgument(client, _guid, rightContent);

            Assert.IsTrue(leftResponse.StatusCode == HttpStatusCode.Accepted);
            Assert.IsTrue(rightResponse.StatusCode == HttpStatusCode.Accepted);

            var analysis = await GetDiffAnalysis(client, _guid);

            Assert.IsTrue(analysis.Id == _guid);
            Assert.IsTrue(analysis.Analyzed);
            Assert.IsFalse(analysis.AreEqual);
            Assert.IsTrue(analysis.AreEqualSize);
            Assert.IsTrue(analysis.Segments.Length == 3);
        }

        [TestMethod]
        public async Task Should_ReturnAreEqualSizeFalse_When_PostingNotEqualSizeDiffArguments()
        {
            var client = _factory.CreateClient();

            var leftContent = "ew0KICAgICJuYW1lIjogIkpvaG4iLA0KICAgICJhZ2UiOiAzNSwNCiAgICAiY2FyIjogIlZvbHZvIg0KfQ==";
            var rightContent = "ew0KICAgICJuYW1lIjogIk1hcmlhbm8iLA0KICAgICJhZ2UiOiAzNywNCiAgICAiY2FyIjogIkZpYXQiDQp9";

            var leftResponse = await PostLeftDiffArgument(client, _guid, leftContent);
            var rightResponse = await PostRightDiffArgument(client, _guid, rightContent);

            Assert.IsTrue(leftResponse.StatusCode == HttpStatusCode.Accepted);
            Assert.IsTrue(rightResponse.StatusCode == HttpStatusCode.Accepted);

            var analysis = await GetDiffAnalysis(client, _guid);

            Assert.IsTrue(analysis.Id == _guid);
            Assert.IsTrue(analysis.Analyzed);
            Assert.IsFalse(analysis.AreEqual);
            Assert.IsFalse(analysis.AreEqualSize);
            Assert.IsTrue(analysis.Segments.Length == 0);
        }

        [TestMethod]
        public async Task Should_ReturnNotAnalized_When_PostingOnlyOneDiffArgument()
        {
            var client = _factory.CreateClient();

            var leftContent = "ew0KICAgICJuYW1lIjogIkpvaG4iLA0KICAgICJhZ2UiOiAzNSwNCiAgICAiY2FyIjogIlZvbHZvIg0KfQ==";

            var leftResponse = await PostLeftDiffArgument(client, _guid, leftContent);

            Assert.IsTrue(leftResponse.StatusCode == HttpStatusCode.Accepted);

            var analysis = await GetDiffAnalysis(client, _guid);

            Assert.IsTrue(analysis.Id == _guid);
            Assert.IsFalse(analysis.Analyzed);
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
            await Task.Delay(5000);

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
