using Dapper;
using SeatReservationCore.Repositories;
using SeatReservationV1.Models.Entities;

namespace SeatReservationV1.Repositories
{
    public class ImagesRepository : GenericRepository<ImageEntity>
    {
        public ImagesRepository(string connectionString) : base(connectionString) { }

        public async Task<byte[]> GetAsync(int restaurantId, Guid guid)
        {
            var sqlCommand = new CommandDefinition($@"
                SELECT 
                    i.[Content]
                FROM Images i
                INNER JOIN ImagesToRestaurants itr ON itr.ImageId = i.Id
                WHERE i.Guid = @guid
                    AND itr.RestaurantId = @restaurantId",
                new
                {
                    @restaurantId = restaurantId,
                    @guid = guid
                });

            return await Db.QueryFirstOrDefaultAsync<byte[]>(sqlCommand);
        }
    }
}
