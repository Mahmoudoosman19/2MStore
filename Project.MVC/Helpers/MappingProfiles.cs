using AutoMapper;
using Project.BLL.Dtos;
using Project.BLL.Dtos.Basket;
using Project.BLL.Dtos.Product;
using Project.DAL.Entities.Order;
using Project.MVC.Helpers;

namespace Project.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<Brand, ProductBrandOrCategoryToReturnDto>()
            .ForMember(d => d.PictureUrl, o => o.MapFrom<PictureUrlResolver<Brand, ProductBrandOrCategoryToReturnDto>>())
            .ReverseMap();
            CreateMap<Brand, CategoryOrBrandCreateDto>()
            .ReverseMap();
            CreateMap<Category, CategoryOrBrandCreateDto>()
            .ReverseMap();


            CreateMap<Category, ProductBrandOrCategoryToReturnDto>()
            .ForMember(d => d.PictureUrl, o => o.MapFrom<PictureUrlResolver<Category, ProductBrandOrCategoryToReturnDto>>())
            .ReverseMap();

            CreateMap<Brand, ProductBrandOrCategoryEditDto>()
            .ForMember(d => d.PictureUrl, o => o.MapFrom<PictureUrlResolver<Brand, ProductBrandOrCategoryEditDto>>())
            .ReverseMap();


            CreateMap<Category, ProductBrandOrCategoryEditDto>()
            .ForMember(d => d.PictureUrl, o => o.MapFrom<PictureUrlResolver<Category, ProductBrandOrCategoryEditDto>>())
            .ReverseMap();



            CreateMap<Product, EditProductDto>()

                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<PictureUrlResolver<Product, EditProductDto>>())
                .ForMember(dest => dest.PriceAfterDiscount, opt => opt.MapFrom(src => src.PriceBeforeDiscount - (src.PriceBeforeDiscount * (src.Discount / 100))))
                .ForMember(d => d.Images, opt => opt.MapFrom<ProductImagesUrlResolver<Product, EditProductDto>>())
                .ReverseMap();


            #region Order Module
            CreateMap<CustomerBasket, CustomerBasketDto>()
               .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.BasketItems));

            CreateMap<BasketItem, BasketItemDto>();
            CreateMap<CustomerBasketDto, CustomerBasket>()
                .ForMember(dest => dest.BasketItems, opt => opt.MapFrom(src => src.Items));
            CreateMap<BasketItemDto, BasketItem>();


            CreateMap<AddressDto, Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());

            #endregion
        }
    }
}
