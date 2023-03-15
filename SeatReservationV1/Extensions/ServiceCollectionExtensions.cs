using SeatReservationV1.Managers.Implementation;
using SeatReservationV1.Managers.Interfaces;
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

            return services;
        }

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddScoped<IUserManager, UserManager>();

            return services;
        }
    }
}
