using Microsoft.SqlServer.Server;
using System.Data;
using static Dapper.SqlMapper;

namespace SeatReservationCore.Extensions
{
    public static class SqlExtensions
    {
        public static ICustomQueryParameter AsIntList(this IEnumerable<int> collection, string name = "Id")
        {
            if (collection == null)
                return null;

            var dataTable = collection.Select(c =>
            {
                var rec = new SqlDataRecord(new SqlMetaData(name, SqlDbType.Int));

                rec.SetInt32(0, c);

                return rec;
            });

            return dataTable.AsTableValuedParameter("IntList");
        }
    }
}
