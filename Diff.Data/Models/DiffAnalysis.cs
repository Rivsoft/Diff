using System;
using System.Collections.Generic;
using System.Text;

namespace Diff.Data.Models
{
    public class DiffAnalysis
    {
        public Guid Id { get; set; }
        public byte[] Left { get; set; }
        public byte[] Right { get; set; }
        //public bool AreEqualSize { get; set; }
        //public DiffSegment[] Segments { get; set; }
    }
}
