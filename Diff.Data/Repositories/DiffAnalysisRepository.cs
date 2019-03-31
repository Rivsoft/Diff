using Diff.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Diff.Data.Repositories
{
    public class DiffAnalysisRepository : IDiffAnalysisRepository
    {
        private readonly DataContext _context;

        public DiffAnalysisRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add a diff analysis to the DB context
        /// </summary>
        /// <param name="analysis">The diff analysis to be added</param>
        public void Add(DiffAnalysis analysis)
        {
            _context.Add(analysis);
        }

        /// <summary>
        /// Retrieve a diff analysis from the DB
        /// </summary>
        /// <param name="id">The GUID of the analysis to be retrieved</param>
        /// <returns></returns>
        public async Task<DiffAnalysis> GetAnalysis(Guid id)
        {
            var result = await _context.DiffAnalysis.FirstOrDefaultAsync(a => a.Id == id);

            return result;
        }

        /// <summary>
        /// Persist all changes to the DB
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
