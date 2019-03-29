using Diff.Core.Interfaces;
using System;

namespace Diff.Core
{
    public class DiffManager : IDiffGenerator
    {
        public IDiffResult GenerateDiff(byte[] left, byte[] right)
        {
            if (left == null || left.Length == 0)
                throw new ArgumentNullException("left");

            if (right == null || right.Length == 0)
                throw new ArgumentNullException("left");

            return new DiffResult(left, right);
        }
    }
}
