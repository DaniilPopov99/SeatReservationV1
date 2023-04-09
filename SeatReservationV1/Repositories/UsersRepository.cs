using Dapper;
using SeatReservationCore.Extensions;
using SeatReservationCore.Repositories;
using SeatReservationV1.Models.Entities;
using SeatReservationV1.Models.Presentation;

namespace SeatReservationV1.Repositories
{
    public class UsersRepository : GenericRepository<UserEntity>
    {
        public UsersRepository(string connectionString) : base(connectionString) { }

        public async Task<int?> GetIdByPhoneNumberAsync(string phoneNumber, string password)
        {
            var sqlCommand = new CommandDefinition(@"
                SELECT 
                    Id 
                FROM Users
                WHERE PhoneNumber = @phoneNumber
                    AND Password = @password",
                new
                {
                    @phoneNumber = phoneNumber,
                    @password = password
                });

            return await Db.QueryFirstOrDefaultAsync<int?>(sqlCommand);
        }

        public async Task<UserEntity> GetAsync(int id)
        {
            var sqlCommand = new CommandDefinition(@"
                SELECT * 
                FROM Users
                WHERE Id = @id",
                new
                {
                    @id = id
                });

            return await Db.QueryFirstOrDefaultAsync<UserEntity>(sqlCommand);
        }

        public async Task<IEnumerable<UserEntity>> GetAsync(IEnumerable<int> userIds)
        {
            var sqlCommand = new CommandDefinition(@"
                SELECT * 
                FROM @userIds ids
                INNER JOIN Users u ON u.Id = ids.Id",
                new
                {
                    @userIds = userIds.AsIntList()
                });

            return await Db.QueryAsync<UserEntity>(sqlCommand);
        }
    }
}
