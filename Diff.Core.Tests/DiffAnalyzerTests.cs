using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace Diff.Core.Tests
{
    [TestClass]
    public class DiffAnalyzerTests
    {
        //Global diff manager to be used on all tests
        private DiffAnalyzer _diffAnalyzer;

        [TestInitialize]
        public void TestInitialize()
        {
            //Initialize the global diff manager used on all tests
            _diffAnalyzer = new DiffAnalyzer();
        }

        [TestMethod]
        public void Should_ThrowException_When_NullLeftArgument()
        {
            byte[] left = null;
            byte[] right = GetTestSample();

            Assert.ThrowsException<ArgumentNullException>(() => _diffAnalyzer.GenerateDiff(left, right));
        }

        [TestMethod]
        public void Should_ThrowException_When_NullRightArgument()
        {
            byte[] left = GetTestSample();
            byte[] right = null;

            Assert.ThrowsException<ArgumentNullException>(() => _diffAnalyzer.GenerateDiff(left, right));
        }

        [TestMethod]
        public void Should_ThrowException_When_NullBothArguments()
        {
            byte[] left = null;
            byte[] right = null;

            Assert.ThrowsException<ArgumentNullException>(() => _diffAnalyzer.GenerateDiff(left, right));
        }

        [TestMethod]
        public void Should_ThrowException_When_BothRequiredArgumentsAreEmpty()
        {
            byte[] left = new byte[] { };
            byte[] right = new byte[] { };

            Assert.ThrowsException<ArgumentNullException>(() => _diffAnalyzer.GenerateDiff(left, right));
        }

        [TestMethod]
        public void Should_ReturnAreEqualSizeTrue_When_ArgumentsAreEqual()
        {
            var left = GetTestSample();
            var right = GetTestSample();

            var result = _diffAnalyzer.GenerateDiff(left, right);

            Assert.IsTrue(result.AreEqualSize);
        }

        [TestMethod]
        public void Should_ReturnAreEqualSizeTrue_When_ArgumentsAreSameSizeButDifferentContent()
        {
            var left = GetTestSample("TestA");
            var right = GetTestSample("TestB");

            var result = _diffAnalyzer.GenerateDiff(left, right);

            Assert.IsTrue(result.AreEqualSize);
        }

        [TestMethod]
        public void Should_ReturnAreEqualSizeFalse_When_ArgumentsAreDifferentSize()
        {
            var left = GetTestSample("TestA");
            var right = GetTestSample("TestLength");

            var result = _diffAnalyzer.GenerateDiff(left, right);

            Assert.IsFalse(result.AreEqualSize);
        }

        [TestMethod]
        public void Should_ReturnOneDiffSegment_When_ArgumentsAreDifferentInOneSegment()
        {
            var left = GetTestSample("TestA");
            var right = GetTestSample("TestB");

            var result = _diffAnalyzer.GenerateDiff(left, right);

            Assert.IsTrue(result.Segments.Count == 1);
            Assert.IsTrue(result.Segments[0].Offset == 4);
            Assert.IsTrue(result.Segments[0].Length == 1);
        }

        [TestMethod]
        public void Should_ReturnOneDiffSegment_When_ArgumentsAreDifferentInOneSegmentCoveringEntireLength()
        {
            var left = GetTestSample("AAAAA");
            var right = GetTestSample("BBBBB");

            var result = _diffAnalyzer.GenerateDiff(left, right);

            Assert.IsTrue(result.Segments.Count == 1);
            Assert.IsTrue(result.Segments[0].Offset == 0);
            Assert.IsTrue(result.Segments[0].Length == 5);
        }

        [TestMethod]
        public void Should_ReturnThreeDiffSegment_When_ArgumentsAreDifferentInThreeSingleByteSegmentsAtStartMiddleAndEnd()
        {
            var left = GetTestSample("AFirstANameA");
            var right = GetTestSample("BFirstBNameB");

            var result = _diffAnalyzer.GenerateDiff(left, right);

            Assert.IsTrue(result.Segments.Count == 3);
            Assert.IsTrue(result.Segments[0].Offset == 0);
            Assert.IsTrue(result.Segments[0].Length == 1);
            Assert.IsTrue(result.Segments[1].Offset == 6);
            Assert.IsTrue(result.Segments[1].Length == 1);
            Assert.IsTrue(result.Segments[2].Offset == 11);
            Assert.IsTrue(result.Segments[2].Length == 1);
        }

        [TestMethod]
        public void Should_ReturnThreeDiffSegment_When_ArgumentsAreDifferentInThreeMultipleByteSegmentsAtStartMiddleAndEnd()
        {
            var left = GetTestSample("AAAFirstAANameAAAAA");
            var right = GetTestSample("BBBFirstBBNameBBBBB");

            var result = _diffAnalyzer.GenerateDiff(left, right);

            Assert.IsTrue(result.Segments.Count == 3);
            Assert.IsTrue(result.Segments[0].Offset == 0);
            Assert.IsTrue(result.Segments[0].Length == 3);
            Assert.IsTrue(result.Segments[1].Offset == 8);
            Assert.IsTrue(result.Segments[1].Length == 2);
            Assert.IsTrue(result.Segments[2].Offset == 14);
            Assert.IsTrue(result.Segments[2].Length == 5);
        }

        private byte[] GetTestSample(string sample = "TestSample")
        {
            return Encoding.UTF8.GetBytes(sample);
        }
    }
}
