using Dapper;
using SeatReservationCore.Extensions;
using SeatReservationCore.Repositories;
using SeatReservationV1.Models.Entities;

namespace SeatReservationV1.Repositories
{
    public class RestaurantsRepository : GenericRepository<RestaurantEntity>
    {
        public RestaurantsRepository(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<RestaurantEntity>> GetByFilterAsync(int take, int skip, string filter = null, IEnumerable<int> ids = null)
        {
            var filterCondition = !string.IsNullOrEmpty(filter) 
                ? $@"AND (Restaurants.Name LIKE '%{filter}%' OR Restaurants.Description LIKE '%{filter}%' OR Restaurants.Address LIKE '%{filter}%')"
                : string.Empty;

            var sqlCommand = new CommandDefinition($@"
                SELECT *
                FROM Restaurants
                WHERE IsActive = 1
                    {filterCondition}
                ORDER BY Id
                OFFSET @skip ROWS
                FETCH NEXT @take ROWS ONLY;",
                new
                {
                    @take = take,
                    @skip = skip
                });

            return await Db.QueryAsync<RestaurantEntity>(sqlCommand);
        }

        public async Task<IEnumerable<RestaurantEntity>> GetByIdsAsync(IEnumerable<int> restaurantIds)
        {
            var sqlCommand = new CommandDefinition($@"
                SELECT 
                    restaurants.*
                FROM @restaurantIds ids
                INNER JOIN Restaurants restaurants ON restaurants.Id = ids.Id",
                new
                {
                    @restaurantIds = restaurantIds.AsIntList()
                });

            return await Db.QueryAsync<RestaurantEntity>(sqlCommand);
        }
    }
}
