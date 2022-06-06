using AutoMapper;
using Cars.Models.Entities;
using Nuyken.Vegasco.Backend.Microservices.Cars.Models.Dtos;
using Nuyken.Vegasco.Backend.Microservices.Cars.Models.Requests;

namespace Nuyken.Vegasco.Backend.Microservices.Cars.Models.Mappings;

public class CarsMappingProfile : Profile
{
    public CarsMappingProfile()
    {
        CreateMap<Car, CarDto>()
            .ForMember(x => x.Id, o => o.MapFrom(x => x.Id.Value));

        CreateMap<CreateCarCommand, Car>();
    }
}