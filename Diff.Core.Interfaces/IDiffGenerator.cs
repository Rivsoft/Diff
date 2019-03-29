using System;

namespace Diff.Core.Interfaces
{
    public interface IDiffGenerator
    {
        IDiffResult GenerateDiff(byte[] left, byte[] right);
    }
}
