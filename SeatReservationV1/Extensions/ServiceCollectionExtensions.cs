using AutoMapper;
using SeatReservationCore.Services.Implementation;
using SeatReservationCore.Services.Interfaces;
using SeatReservationV1.AutoMapper;
using SeatReservationV1.Managers.Implementation;
using SeatReservationV1.Managers.Interfaces;
using SeatReservationV1.Microservices.Implementation;
using SeatReservationV1.Microservices.Interfaces;
using SeatReservationV1.Models.Options;
using SeatReservationV1.Repositories;

namespace SeatReservationV1.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton(configuration.GetSection("AppSettings").Get<AppSettings>());

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddScoped(provider => new UsersRepository(connectionString));
            services.AddScoped(provider => new RestaurantsRepository(connectionString));
            services.AddScoped(provider => new OrdersRepository(connectionString));
            services.AddScoped(provider => new ImagesToRestaurantsRepository(connectionString));
            services.AddScoped(provider => new ImagesRepository(connectionString));
            services.AddScoped(provider => new FavoritesRestaurantsRepository(connectionString));

            return services;
        }

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IRestaurantManager, RestaurantManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<IRestaurantImageManager, RestaurantImageManager>();
            
            return services;
        }

        public static IServiceCollection AddMicroservices(this IServiceCollection services)
        {
            services.AddHttpClient<IHttpService, HttpService>();

            services.AddScoped<ISimilarImagesMicroservice, SimilarImagesMicroservice>();

            return services;
        }

        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            var mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }
    }
}
