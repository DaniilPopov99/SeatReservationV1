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
    }
}
