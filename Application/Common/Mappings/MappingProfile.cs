using Application.Addresses.Dtos;
using Application.Categories.Dtos;
using Application.Offers.Dtos;
using Application.Products.Dtos;
using Application.Users.Dtos;
using Application.Users.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 🧍 Users
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();

            // 🎁 Offers
            CreateMap<CreateOfferDto, Offer>();
            CreateMap<Offer, OfferDto>().ReverseMap();
            CreateMap<UpdateOfferDto, Offer>().ReverseMap();

            // 🗂️ Categories
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryID))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentID))
                .ForMember(dest => dest.IconName, opt => opt.MapFrom(src => src.IconName))
                .ReverseMap();

            CreateMap<Category, CategoryTreeDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryID))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.IconName, opt => opt.MapFrom(src => src.IconName))
                .ReverseMap();

            // 🛒 Products
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.MainImageURL, opt => opt.MapFrom(src => src.MainImageURL))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ReverseMap();

            CreateMap<Address, UpdateAddressDto>().ReverseMap();
        }
    }
}

