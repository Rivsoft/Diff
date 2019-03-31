using Diff.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Diff.Data.Repositories
{
    public interface IDiffAnalysisRepository
    {
        void Add(DiffAnalysis analysis);
        Task<DiffAnalysis> GetAnalysis(Guid id);
        Task<bool> SaveAll();
    }
}
