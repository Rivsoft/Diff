using System;
using System.Collections.Generic;
using System.Text;

namespace Diff.Data.Models
{
    /// <summary>
    /// Represents the data model entity that will store all the details of a diff analysis.
    /// </summary>
    public class DiffAnalysis
    {
        public Guid Id { get; set; }
        public byte[] Left { get; set; }
        public byte[] Right { get; set; }
        //public bool AreEqualSize { get; set; }
        public ICollection<DiffSegment> Segments { get; } = new List<DiffSegment>();
        public bool Analized { get; set; }
    }
}
