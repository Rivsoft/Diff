using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diff.API.DTOs
{
    public class GetDiffSegmentForResultDTO
    {
        public int Offset { get; set; }
        public int Length { get; set; }
    }
}
