using AutoMapper;
using RestoreAPI.DTOs;
using RestoreAPI.Entities;

namespace RestoreAPI.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
