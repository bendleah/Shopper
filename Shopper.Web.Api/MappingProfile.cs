using AutoMapper;
using Shopper.Web.Api.Dto;
using Shopper.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopper.Web.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderLine, OrderLineDto>()
                .ForMember(dest => dest.Name, src => src.MapFrom(o => o.Product.Name))
                .ForMember(dest => dest.Price, src => src.MapFrom(o => o.Product.Price))
                .ForMember(dest => dest.ProductId, src => src.MapFrom(o => o.Product.Id))
                .ForMember(dest => dest.Quantity, src => src.MapFrom(o => o.Quantity));

            CreateMap<OrderLine, OrderLineUpdateDto>()
                .ForMember(dest => dest.Quantity, src => src.MapFrom(o => o.Quantity)).ReverseMap();

            CreateMap<Basket, BasketDto>()
                .ForMember(dest => dest.NumberOfOrderLines, src => src.MapFrom(o => o.OrderLines.Count()));

            CreateMap<Product, ProductDto>();
        }
    }
}
