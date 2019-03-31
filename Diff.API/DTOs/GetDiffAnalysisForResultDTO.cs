using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diff.API.DTOs
{
    public class GetDiffAnalysisForResultDTO
    {
        public Guid Id { get; set; }
        public byte[] Left { get; set; }
        public byte[] Right { get; set; }
        public GetDiffSegmentForResultDTO[] Segments { get; set; }
        public bool Analized { get; set; }
    }
}
