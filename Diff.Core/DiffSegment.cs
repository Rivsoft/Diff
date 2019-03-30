using Diff.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diff.Core
{
    public class DiffSegment : IDiffSegment
    {
        public int Offset { get; private set; }
        public int Length { get; private set; }

        public DiffSegment(int offset, int length)
        {
            Offset = offset;
            Length = length;
        }
    }
}
