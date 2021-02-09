using System.Data.Common;

namespace AutoParkData
{
    public interface IDbConnectionFactory
    {
        public DbConnection GetDbConnection();
    }
}