using Dapper;
using SeatReservationCore.Repositories;
using SeatReservationV1.Models.Entities;

namespace SeatReservationV1.Repositories
{
    public class FavoritesRestaurantsRepository : GenericRepository<FavoritesRestaurantEntity>
    {
        public FavoritesRestaurantsRepository(string connectionString) : base(connectionString) { }

        public async Task RemoveAsync(int userId, int restaurantId)
        {
            var sqlCommand = new CommandDefinition($@"
                UPDATE FavoritesRestaurants
                SET IsActive = 0
                WHERE UserId = @userId
                    AND RestaurantId = @restaurantId",
                new
                {
                    @userId = userId,
                    @restaurantId = restaurantId
                });

            await Db.ExecuteAsync(sqlCommand);
        }

        public async Task<IEnumerable<int>> GetActiveAsync(int take, int skip, int userId)
        {
            var sqlCommand = new CommandDefinition($@"
                SELECT DISTINCT
                    RestaurantId
                FROM FavoritesRestaurants
                WHERE UserId = @userId
                    AND IsActive = 1
                ORDER BY Id DESC
                OFFSET @skip ROWS
                FETCH NEXT @take ROWS ONLY;",
                new
                {
                    @take = take,
                    @skip = skip,
                    @userId = userId
                });

            return await Db.QueryAsync<int>(sqlCommand);
        }
    }
}
