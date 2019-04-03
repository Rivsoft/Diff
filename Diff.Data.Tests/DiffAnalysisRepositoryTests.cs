using Diff.Data.Models;
using Diff.Data.Repositories;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Diff.Data.Tests
{
    [TestClass]
    public class DiffAnalysisRepositoryTests
    {
        private IDiffAnalysisRepository _analysisRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            var _testAnalysis = GetTestAnalysis();

            var _dataContext = new DbContextMock<DataContext>(new DbContextOptionsBuilder<DataContext>().Options);
            _dataContext.CreateDbSetMock(x => x.DiffAnalysis, _testAnalysis);

            _analysisRepo = new DiffAnalysisRepository(_dataContext.Object);
        }

        private List<DiffAnalysis> GetTestAnalysis()
        {
            return new List<DiffAnalysis>()
            {
                new DiffAnalysis
                {
                    Id = new Guid("735C5AE5-7054-487A-984A-A51C78F1E706"),
                    Analyzed = true,
                    Left = Encoding.UTF8.GetBytes("TestA"),
                    Right = Encoding.UTF8.GetBytes("TestB"),
                    Segments = new List<DiffSegment>() {new DiffSegment() { Id = Guid.NewGuid(), Length = 1, Offset = 4 } }
                },
                new DiffAnalysis
                {
                    Id = new Guid("16E96D77-4EC6-4D32-BB00-C8FE3F26C6FD"),
                    Analyzed = false,
                    Left = Encoding.UTF8.GetBytes("TestA")
                }
            };
        }

        [TestMethod]
        public async Task Should_ReturnNotNull_When_ExistingAnalysisFound()
        {
            var guid = new Guid("735C5AE5-7054-487A-984A-A51C78F1E706");

            var analysis = await _analysisRepo.GetAnalysis(guid);

            Assert.IsNotNull(analysis);
        }

        [TestMethod]
        public async Task Should_ReturnNull_When_AnalysisNotFound()
        {
            var guid = Guid.NewGuid();

            var analysis = await _analysisRepo.GetAnalysis(guid);

            Assert.IsNull(analysis);
        }

        [TestMethod]
        public async Task Should_RetrieveAnalysis_When_NewAnalysisAdded()
        {
            var guid = Guid.NewGuid();

            var newAnalysis = new DiffAnalysis()
            {
                Id = guid
            };

            _analysisRepo.Add(newAnalysis);
            await _analysisRepo.SaveAll();

            var analysis = await _analysisRepo.GetAnalysis(guid);

            Assert.IsNotNull(analysis);
        }
    }
}
