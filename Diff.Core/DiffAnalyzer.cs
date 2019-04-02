using Diff.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Diff.Core
{
    public class DiffAnalyzer : IDiffAnalyzer
    {
        public IDiffResult GenerateDiff(byte[] left, byte[] right)
        {
            if (left == null || left.Length == 0)
                throw new ArgumentNullException("left");

            if (right == null || right.Length == 0)
                throw new ArgumentNullException("right");

            if (left.Length != right.Length)
                return new DiffResult(left, right);

            //Return diff segments between both arrays
            var segments = CompareArrays(left, right);

            return new DiffResult(left, right, segments);
        }

        private List<IDiffSegment> CompareArrays(byte[] left, byte[] right)
        {
            var segments = new List<IDiffSegment>();
            int offset = -1, length = 0;

            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    if (offset == -1)
                        offset = i;

                    length++;
                }
                else
                {
                    if (offset != -1)
                    {
                        segments.Add(new DiffSegment(offset, length));
                        offset = -1;
                        length = 0;
                    }
                }
            }

            //Check for diff in the end of the arrays
            if (offset != -1)
                segments.Add(new DiffSegment(offset, length));

            return segments;
        }
    }
}
