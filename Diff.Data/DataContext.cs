using System;
using Diff.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Diff.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new DiffAnalysisConfiguration());
        }

        public DbSet<DiffAnalysis> DiffAnalysis { get; set; }
    }
}
