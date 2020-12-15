using System;
using AutoMapper;

namespace Property.Api.Features.PropertyCollection
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Core.Property, ListByPropertyReference.Result.Property>()
                .ForMember(dest => dest.PropertyReference, opt => opt.MapFrom(src => src.Reference.Reference))
                .ForMember(dest => dest.PropertyDescription, opt => opt.MapFrom(src => src.Description));
        }
    }
}
