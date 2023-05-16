using API.DTOs;
using AutoMapper;
using Core;
using Core.Helpers;
using Core.Models;

namespace API.Profiles;

public class APIProfile : Profile
{
    public APIProfile()
    {
        CreateMap<Product, ProductDetailsDto>();
        CreateMap<PagedList<Product>, PagedDto>();
        CreateMap<NotEnoughStockException, NotEnoughStock>();
        CreateMap<ProductNotFoundException, ProductNotFound>();
    }
}
