using System;
using System.Collections.Generic;
using System.Text;

namespace Diff.Core.Interfaces
{
    public interface IDiffSegment
    {
        long Offset { get; }
        long Length { get; }
    }
}
