﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Diff.Data.Models
{
    public class DiffSegment
    {
        public Guid Id { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
    }
}
