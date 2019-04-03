using AutoMapper;
using Diff.API.Controllers;
using Diff.API.DTOs;
using Diff.API.Helpers;
using Diff.Data.Models;
using Diff.Data.Repositories;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diff.API.Tests
{
    [TestClass]
    public class DiffControllerTests
    {
        // Global diff controller to be used on all tests
        private DiffController _diffController;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockedBus = new Mock<IBus>();

            var mockedMapper = SetupMockMapper();

            var mockedDiffRepo = new Mock<IDiffAnalysisRepository>();
            mockedDiffRepo
                .Setup(x => x.GetAnalysis(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) =>
                {
                    return GetSampleAnalysis().FirstOrDefault(a => a.Id == id);
                });

            // Mock the Url property of the controller
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("/v1/diff/");

            // Initialize the diff controller used on all tests
            _diffController = new DiffController(mockedBus.Object, mockedMapper.Object, mockedDiffRepo.Object)
            {
                Url = mockUrlHelper.Object
            };
        }

        private static Mock<IMapper> SetupMockMapper()
        {
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<GetDiffAnalysisForResultDTO>(It.IsAny<DiffAnalysis>()))
                .Returns((DiffAnalysis source) =>
                {
                    return new GetDiffAnalysisForResultDTO
                    {
                        Id = source.Id,
                        AreEqual = source.Left.Length == source.Right.Length && source.Segments.Count == 0,
                        AreEqualSize = source.Left.Length == source.Right.Length,
                        Segments = source.Segments.Select(x => new GetDiffSegmentForResultDTO { Length = x.Length, Offset = x.Offset }).ToArray()
                    };
                });

            return mockMapper;
        }

        private List<DiffAnalysis> GetSampleAnalysis()
        {
            return new List<DiffAnalysis>
            {
                new DiffAnalysis
                {
                    Id = Guid.Parse("50847358-8DDE-47E1-950D-ED04BB9720CA"),
                    Analyzed = false,
                    Left = null,
                    Right = null
                },
                new DiffAnalysis
                {
                    Id = Guid.Parse("20D4DC4F-2805-4D47-B658-D19BFF5C4A43"),
                    Analyzed = true,
                    Left = Base64Helper.ConvertBase64String("ew0KICAgICJuYW1lIjogIkpvaG4iLA0KICAgICJhZ2UiOiAzMCwNCiAgICAiY2FyIjogbnVsbA0KfQ=="),
                    Right = Base64Helper.ConvertBase64String("ew0KICAgICJuYW1lIjogIkpvaG4iLA0KICAgICJhZ2UiOiAzMCwNCiAgICAiY2FyIjogbnVsbA0KfQ==")
                },
            };
        }

        #region AddLeft action tests
        [TestMethod]
        public async Task AddLeft_Should_ReturnBadRequest_When_NullInput()
        {
            var result = await _diffController.AddLeft(Guid.Empty, null) as BadRequestResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task AddLeft_Should_ReturnBadRequest_When_NonBase64Input()
        {
            var guid = Guid.NewGuid();
            var result = await _diffController.AddLeft(guid, "Test!") as BadRequestResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task AddLeft_Should_ReturnAccepted_When_ValidInput()
        {
            var guid = Guid.NewGuid();
            var input = Convert.ToBase64String(Encoding.UTF8.GetBytes("TestString"));
            var result = await _diffController.AddRight(guid, input) as AcceptedResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(202, result.StatusCode);
        }
        #endregion

        #region AddRight action tests
        [TestMethod]
        public async Task AddRight_Should_ReturnBadRequest_When_NullInput()
        {
            var result = await _diffController.AddRight(Guid.Empty, null) as BadRequestResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task AddRight_Should_ReturnBadRequest_When_NonBase64Input()
        {
            var guid = Guid.NewGuid();
            var result = await _diffController.AddRight(guid, "Test!") as BadRequestResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task AddRight_Should_ReturnAccepted_When_ValidInput()
        {
            var guid = Guid.NewGuid();
            var input = Convert.ToBase64String(Encoding.UTF8.GetBytes("TestString"));
            var result = await _diffController.AddRight(guid, input) as AcceptedResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(202, result.StatusCode);
        }
        #endregion

        #region GetDiffResult action tests
        [TestMethod]
        public async Task GetDiffResult_Should_ReturnNotFound_When_GuidIsEmpty()
        {
            var result = await _diffController.GetDiffResult(Guid.Empty) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task GetDiffResult_Should_ReturnValidModel_When_AnalysisIsFound()
        {
            var guid = new Guid("20D4DC4F-2805-4D47-B658-D19BFF5C4A43");
            var response = await _diffController.GetDiffResult(guid) as OkObjectResult;
            var analysis = response.Value as GetDiffAnalysisForResultDTO;

            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);
            Assert.IsNotNull(analysis);
            Assert.AreEqual(guid, analysis.Id);
        }
        #endregion
    }
}
