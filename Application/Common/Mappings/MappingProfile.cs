using Application.Addresses.Dtos;
using Application.Categories.Dtos;
using Application.Offers.Dtos;
using Application.Orders.Dtos;
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
            CreateMap< CreateOfferDto, Offer>();
            CreateMap<Offer, OfferDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src =>
                    src.Products.Select(p => new OfferProductDto
                    {
                        ProductId = p.ProductID,
                        ProductName = p.ProductName,
                        CategoryName = p.Category != null ? p.Category.CategoryName : string.Empty,
                        OriginalPrice = p.Price,
                        DiscountedPrice = p.Price * (1 -
                            (src.DiscountPercentage > 1
                                ? (src.DiscountPercentage / 100)
                                : src.DiscountPercentage))
                    })
                ));

            CreateMap<UpdateOfferDto, Offer>().ReverseMap();

            // 🗂️ Categories
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryID))
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

            // 📦 Orders
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserPhone, opt => opt.MapFrom(src => src.User.Phone))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
                .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments));

            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));

            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()));

            // 📍 Addresses
            CreateMap<Address, AddressDto>()
                .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => src.AddressType.ToString()));

        }
    }
}

