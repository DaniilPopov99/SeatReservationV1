using Dapper;
using SeatReservationCore.Repositories;
using SeatReservationV1.Models.Entities;

namespace SeatReservationV1.Repositories
{
    public class OrdersRepository : GenericRepository<OrderEntity>
    {
        public OrdersRepository(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<OrderEntity>> GetActiveAsync(int userId)
        {
            var sqlCommand = new CommandDefinition($@"
                SELECT *
                FROM Orders
                WHERE UserId = @userId
                    AND IsActive = 1
                ORDER BY Date",
                new
                {
                    @userId = userId
                });

            return await Db.QueryAsync<OrderEntity>(sqlCommand);
        }

        public async Task<IEnumerable<OrderEntity>> GetInactiveAsync(int take, int skip, int userId)
        {
            var sqlCommand = new CommandDefinition($@"
                SELECT *
                FROM Orders
                WHERE UserId = @userId
                    AND IsActive = 0
                ORDER BY Date DESC
                OFFSET @skip ROWS
                FETCH NEXT @take ROWS ONLY;",
                new
                {
                    @take = take,
                    @skip = skip,
                    @userId = userId
                });

            return await Db.QueryAsync<OrderEntity>(sqlCommand);
        }

        public async Task<IEnumerable<OrderEntity>> GetActiveByRestaurantAsync(int take, int skip, int restaurantId)
        {
            var sqlCommand = new CommandDefinition($@"
                SELECT *
                FROM Orders
                WHERE RestaurantId = @restaurantId
                    AND IsActive = 1
                ORDER BY Date
                OFFSET @skip ROWS
                FETCH NEXT @take ROWS ONLY;",
                new
                {
                    @take = take,
                    @skip = skip,
                    @restaurantId = restaurantId
                });

            return await Db.QueryAsync<OrderEntity>(sqlCommand);
        }

        public async Task<IEnumerable<OrderEntity>> GetInactiveByRestaurantAsync(int take, int skip, int restaurantId)
        {
            var sqlCommand = new CommandDefinition($@"
                SELECT *
                FROM Orders
                WHERE RestaurantId = @restaurantId
                    AND IsActive = 0
                ORDER BY Date DESC
                OFFSET @skip ROWS
                FETCH NEXT @take ROWS ONLY;",
                new
                {
                    @take = take,
                    @skip = skip,
                    @restaurantId = restaurantId
                });

            return await Db.QueryAsync<OrderEntity>(sqlCommand);
        }

        public async Task<int> GetCountByUserAndRestaurantAsync(int userId, int restaurantId)
        {
            var sqlCommand = new CommandDefinition($@"
                SELECT 
                    COUNT(Id)
                FROM Orders
                WHERE UserId = @userId
                    AND RestaurantId = @restaurantId",
                new
                {
                    @userId = userId,
                    @restaurantId = restaurantId
                });

            return await Db.QueryFirstOrDefaultAsync<int>(sqlCommand);
        }
    }
}
