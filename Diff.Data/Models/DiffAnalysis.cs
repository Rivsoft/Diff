using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ICollection<DiffSegment> Segments { get; set; } = new List<DiffSegment>();
        public bool Analyzed { get; set; }
    }
}
