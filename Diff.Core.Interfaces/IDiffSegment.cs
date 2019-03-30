using System;
using System.Collections.Generic;
using System.Text;

namespace Diff.Core.Interfaces
{
    public interface IDiffSegment
    {
        int Offset { get; }
        int Length { get; }
    }
}
