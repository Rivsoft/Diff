using System;
using System.Collections.Generic;
using System.Text;

namespace Diff.Core.Integration.Messages
{
    public class PerformDiffAnalysisMessage
    {
        public Guid Id { get; private set; }

        public PerformDiffAnalysisMessage(Guid id)
        {
            Id = id;
        }
    }
}
