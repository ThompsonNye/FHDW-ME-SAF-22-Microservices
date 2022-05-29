using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Dtos;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Mappings;

public class ConsumptionMappingProfile : Profile
{
    public ConsumptionMappingProfile()
    {
        CreateMap<Consumption, ConsumptionDto>()
            .ForMember(x => x.Id, config => config.MapFrom(x => x.Id.Value))
            .ForMember(x => x.CarId, config => config.MapFrom(x => x.CarId.Value));
        
        CreateMap<ConsumptionDto, Consumption>()
            .ForMember(x => x.Id, config => config.MapFrom(dto => new ConsumptionId(dto.Id)))
            .ForMember(x => x.CarId, config => config.MapFrom(dto => new CarId(dto.CarId)));
    }
}
