using System.Linq;
using AutoMapper;
using Diff.API.DTOs;
using Diff.Data.Models;

namespace AlmostSorted.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<DiffAnalysis, GetDiffAnalysisForResultDTO>()
                .ForMember(dest => dest.Segments, opt => {
                    opt.MapFrom(src => src.Segments.Select(x =>
                        new GetDiffSegmentForResultDTO { Offset = x.Offset, Length = x.Length }));
                });
        }
    }
}