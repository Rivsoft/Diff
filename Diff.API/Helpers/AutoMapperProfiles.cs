using System.Linq;
using AutoMapper;
using Diff.API.DTOs;
using Diff.Data.Models;

namespace Diff.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<DiffAnalysis, GetDiffAnalysisForResultDTO>()
                .ForMember(dest => dest.Segments, opt =>
                {
                    opt.MapFrom(src => src.Segments.Select(x =>
                        new GetDiffSegmentForResultDTO { Offset = x.Offset, Length = x.Length }));
                })
                .ForMember(dest => dest.AreEqual, opt =>
                {
                    opt.MapFrom(src => src.Left.Length == src.Right.Length && src.Segments.Count == 0);
                })
                .ForMember(dest => dest.AreEqualSize, opt =>
                {
                    opt.MapFrom(src => src.Left.Length == src.Right.Length);
                });
        }
    }
}