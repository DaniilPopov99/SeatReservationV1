using System.Data;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;

namespace SeatReservationCore.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly string connectionString;

        protected IDbConnection Db => new SqlConnection(connectionString);

        public GenericRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public virtual async Task<int> CreateAsync(TEntity entity, IDbConnection connection = null, IDbTransaction transaction = null) =>
           await (connection ?? Db).InsertAsync(entity, transaction);

        public virtual async Task<int> CreateAsync(IEnumerable<TEntity> entities, IDbConnection connection = null, IDbTransaction transaction = null) =>
            await (connection ?? Db).InsertAsync(entities, transaction);
    }
}
