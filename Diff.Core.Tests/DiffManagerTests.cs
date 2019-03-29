using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Diff.Core.Tests
{
    [TestClass]
    public class DiffManagerTests
    {
        [TestMethod]
        public void Should_ThrowException_When_MissingRequiredArguments()
        {
            var diffManager = new DiffManager();
            var left = GetTestSample();
            var right = GetTestSample();

            Assert.ThrowsException<ArgumentNullException>(() => diffManager.GenerateDiff(left, right));
        }
    }
}
