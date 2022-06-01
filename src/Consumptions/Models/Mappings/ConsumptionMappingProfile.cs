using AutoMapper;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Dtos;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Requests;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Mappings;

public class ConsumptionMappingProfile : Profile
{
    public ConsumptionMappingProfile()
    {
        CreateMap<Consumption, ConsumptionDto>()
            .ForMember(x => x.Id, config => config.MapFrom(x => x.Id.Value))
            .ForMember(x => x.CarId, config => config.MapFrom(x => x.CarId.Value));

        CreateMap<CreateConsumptionCommand, Consumption>()
            .ForMember(x => x.CarId, config => config.MapFrom(x => x.CarId.Value));
    }
}