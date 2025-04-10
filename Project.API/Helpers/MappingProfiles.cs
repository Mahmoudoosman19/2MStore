using Project.BLL.Dtos;
using Project.BLL.Dtos.Basket;
using Project.DAL.Entities.Order;
using Project.MVC.Helpers;

namespace Project.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductsToReturnDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<PictureUrlResolver<Product, ProductsToReturnDto>>())
            .ForMember(dest => dest.PriceAfterDiscount, opt => opt.MapFrom(src => src.PriceBeforeDiscount - (src.PriceBeforeDiscount * (src.Discount / 100))))
            .ForMember(d => d.Images, opt => opt.MapFrom<ProductImagesUrlResolver<Product, ProductsToReturnDto>>())

            .ReverseMap();

            CreateMap<Brand, ProductBrandOrCategoryToReturnDto>()
            .ForMember(d => d.PictureUrl, o => o.MapFrom<PictureUrlResolver<Brand, ProductBrandOrCategoryToReturnDto>>());


            CreateMap<Category, ProductBrandOrCategoryToReturnDto>()
            .ForMember(d => d.PictureUrl, o => o.MapFrom<PictureUrlResolver<Category, ProductBrandOrCategoryToReturnDto>>());




            CreateMap<Product, ProductDto>()

                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))

                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))

                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<PictureUrlResolver<Product, ProductDto>>())

                .ForMember(dest => dest.PriceAfterDiscount, opt => opt.MapFrom(src => src.PriceBeforeDiscount - (src.PriceBeforeDiscount * (src.Discount / 100))))

                .ForMember(d => d.Images, opt => opt.MapFrom<ProductImagesUrlResolver<Product, ProductDto>>())

                .ReverseMap();

            CreateMap<Detail, DetailDto>()

                .ReverseMap();

            CreateMap<Image, ImageToReturnDto>()

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
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
                .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity));

            #endregion


        }
    }
}
