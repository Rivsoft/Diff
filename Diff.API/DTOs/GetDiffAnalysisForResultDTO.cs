using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diff.API.DTOs
{
    public class GetDiffAnalysisForResultDTO
    {
        public Guid Id { get; set; }
        public bool Analyzed { get; set; }
        public bool AreEqual { get; set; }
        public bool AreEqualSize { get; set; }
        public GetDiffSegmentForResultDTO[] Segments { get; set; }
    }
}
