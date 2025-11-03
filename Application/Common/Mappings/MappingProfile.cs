using Application.Addresses.Dtos;
using Application.Categories.Dtos;
using Application.Offers.Dtos;
using Application.Orders.Dtos;
using Application.Products.Dtos;
using Application.Users.Dtos;
using Application.Users.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 🧍 USERS
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserPermissions.ToString()))
                .ReverseMap();

            CreateMap<User, UpdateUserDto>().ReverseMap();

            // 🎁 OFFERS
            CreateMap<CreateOfferDto, Offer>();
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

            // 🗂️ CATEGORIES
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryID))
                .ForMember(dest => dest.IconName, opt => opt.MapFrom(src => src.IconName))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
                .ReverseMap();

            // 🛒 PRODUCTS
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.ProductDescription))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock))
                .ForMember(dest => dest.MainImageURL, opt => opt.MapFrom(src => src.MainImageURL))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : "N/A"))
                .ReverseMap();

            // Product → ProductDetailsDto
            CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : "N/A"))
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.OrderDetails));

            // OrderDetail → ProductOrderDto
            CreateMap<OrderDetail, ProductOrderDto>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.Order != null ? src.Order.OrderDate : default))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Order != null && src.Order.User != null ? src.Order.User.UserName : "N/A"))
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Subtotal));

            // 📦 ORDERS
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : "N/A"))
                .ForMember(dest => dest.UserPhone, opt => opt.MapFrom(src => src.User != null ? src.User.Phone : "N/A"))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
                .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments));

            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));

            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.PaymentId))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
                .ReverseMap()
                .ForMember(dest => dest.PaymentMethod, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentStatus, opt => opt.Ignore());

            // 📍 ADDRESSES
            CreateMap<Address, AddressDto>()
                .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => src.AddressType.ToString()));
        }
    }
}
