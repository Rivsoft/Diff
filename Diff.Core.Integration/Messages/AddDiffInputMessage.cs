using System;
using System.Collections.Generic;
using System.Text;

namespace Diff.Core.Integration.Messages
{
    public class AddDiffInputMessage
    {
        public Guid Id { get; set; }
        public byte[] Input { get; set; }
        public bool IsLeft { get; set; }
    }
}
