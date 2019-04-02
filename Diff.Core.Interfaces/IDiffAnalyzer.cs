using System;

namespace Diff.Core.Interfaces
{
    public interface IDiffAnalyzer
    {
        IDiffResult GenerateDiff(byte[] left, byte[] right);
    }
}
