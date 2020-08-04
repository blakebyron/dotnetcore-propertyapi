using System;
using AutoMapper;

namespace Property.Api.Features.Property
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Core.Property, List.Result.Property>()
                .ForMember(dest => dest.PropertyReference, opt => opt.MapFrom(src => src.Reference.Reference))
                .ForMember(dest => dest.PropertyDescription, opt => opt.MapFrom(src => src.Description));

            CreateMap<Core.Property, Detail.Result>()
                .ForMember(dest => dest.PropertyReference, opt => opt.MapFrom(src => src.Reference.Reference))
                .ForMember(dest => dest.PropertyDescription, opt => opt.MapFrom(src => src.Description));

        }
    }
}
