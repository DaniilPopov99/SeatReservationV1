﻿using Dapper;
using SeatReservationCore.Extensions;
using SeatReservationCore.Repositories;
using SeatReservationV1.Models.Entities;

namespace SeatReservationV1.Repositories
{
    public class ImagesToRestaurantsRepository : GenericRepository<ImageToRestaurantEntity>
    {
        public ImagesToRestaurantsRepository(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<KeyValuePair<int, Guid>>> GetRestaurantIdsToImageGuidsAsync(IEnumerable<int> restaurantIds)
        {
            var sqlCommand = new CommandDefinition($@"
                SELECT DISTINCT
                    itr.RestaurantId AS [Key],
                    i.Guid AS [Value]
                FROM @restaurantIds ids
                INNER JOIN ImagesToRestaurants itr ON itr.RestaurantId = ids.Id
                INNER JOIN Images i ON i.Id = itr.ImageId
                WHERE itr.IsActive = 1",
                new
                {
                    @restaurantIds = restaurantIds.AsIntList()
                });

            return await Db.QueryAsync<KeyValuePair<int, Guid>>(sqlCommand);
        }

        public async Task<IEnumerable<KeyValuePair<int, byte[]>>> GetImageIdsToContentsAsync(IEnumerable<int> imageIds)
        {
            var sqlCommand = new CommandDefinition($@"
                SELECT DISTINCT
                    i.Id AS [Key],
                    i.[Content] AS [Value]
                FROM @imageIds ids
                INNER JOIN Images i ON i.Id = ids.Id",
                new
                {
                    @imageIds = imageIds.AsIntList()
                });

            return await Db.QueryAsync<KeyValuePair<int, byte[]>>(sqlCommand);
        }

        public async Task<IEnumerable<int>> GetRestaurantsByImagesAsync(IEnumerable<int> imageIds)
        {
            var sqlCommand = new CommandDefinition($@"
                SELECT
                    itr.RestaurantId
                FROM @imageIds ids
                INNER JOIN ImagesToRestaurants itr ON itr.ImageId = ids.Id",
                new
                {
                    @imageIds = imageIds.AsIntList()
                });

            return await Db.QueryAsync<int>(sqlCommand);
        }
    }
}
