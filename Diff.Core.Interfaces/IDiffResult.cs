using System;
using System.Collections.Generic;
using System.Text;

namespace Diff.Core.Interfaces
{
    public interface IDiffResult
    {
        byte[] Left { get; }
        byte[] Right { get; }
        bool AreEqualSize { get; }
        IList<IDiffSegment> Segments { get; }        
    }
}
