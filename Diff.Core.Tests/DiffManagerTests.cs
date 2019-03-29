using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace Diff.Core.Tests
{
    [TestClass]
    public class DiffManagerTests
    {
        //Global diff manager to be used on all tests
        private DiffManager _diffManager;

        [TestInitialize]
        public void TestInitialize()
        {
            //Initialize the global diff manager used on all tests
            _diffManager = new DiffManager();
        }

        [TestMethod]
        public void Should_ThrowException_When_NullLeftArgument()
        {
            byte[] left = null;
            byte[] right = GetTestSample();

            Assert.ThrowsException<ArgumentNullException>(() => _diffManager.GenerateDiff(left, right));
        }

        [TestMethod]
        public void Should_ThrowException_When_NullRightArgument()
        {
            byte[] left = GetTestSample();
            byte[] right = null;

            Assert.ThrowsException<ArgumentNullException>(() => _diffManager.GenerateDiff(left, right));
        }

        [TestMethod]
        public void Should_ThrowException_When_BothRequiredArgumentsAreEmpty()
        {
            byte[] left = new byte[] { };
            byte[] right = new byte[] { };

            Assert.ThrowsException<ArgumentNullException>(() => _diffManager.GenerateDiff(left, right));
        }

        [TestMethod]
        public void Should_ReturnAreEqualSizeTrue_When_DiffArgsAreEqual()
        {
            var left = GetTestSample();
            var right = GetTestSample();

            var result = _diffManager.GenerateDiff(left, right);

            Assert.IsTrue(result.AreEqualSize);
        }

        [TestMethod]
        public void Should_ReturnAreEqualSizeTrue_When_DiffArgsAreSameSizeButDifferentContent()
        {
            var left = GetTestSample("TestA");
            var right = GetTestSample("TestB");

            var result = _diffManager.GenerateDiff(left, right);

            Assert.IsTrue(result.AreEqualSize);
        }

        [TestMethod]
        public void Should_ReturnAreEqualSizeFalse_When_DiffArgsAreDifferentSize()
        {
            var left = GetTestSample("TestA");
            var right = GetTestSample("TestLength");

            var result = _diffManager.GenerateDiff(left, right);

            Assert.IsFalse(result.AreEqualSize);
        }

        private byte[] GetTestSample(string sample = "TestSample")
        {
            return Encoding.UTF8.GetBytes(sample);
        }
    }
}
