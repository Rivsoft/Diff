using Diff.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diff.Core
{
    public class DiffResult : IDiffResult
    {
        public byte[] Left { get; private set; }
        public byte[] Right { get; private set; }
        public List<IDiffSegment> Segments { get; private set; }

        public bool AreEqualSize => Left?.Length == Right?.Length;

        public DiffResult(byte[] left, byte[] right, List<IDiffSegment> segments)
        {
            Left = left;
            Right = right;
            Segments = segments;
        }
    }
}
