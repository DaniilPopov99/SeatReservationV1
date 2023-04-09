using AutoMapper;
using SeatReservationV1.Models.Entities;
using SeatReservationV1.Models.Presentation;

namespace SeatReservationV1.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateOrderVM, OrderEntity>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<OrderEntity, UserOrderVM>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id));

            CreateMap<CreateRestaurantVM, RestaurantEntity>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<RestaurantEntity, RestaurantVM>()
                .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => src.Address + ' ' + src.House)); //TODO сделать хелпер под это
            
            CreateMap<RegisterUserVM, UserEntity>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<OrderEntity, RestaurantOrderVM>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id));

            CreateMap<UserVM, UserVMAndOrdersCount>();
        }
    }
}
