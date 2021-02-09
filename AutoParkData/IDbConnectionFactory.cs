using System.Data;
using System.Data.Common;

namespace AutoParkData
{
    public interface IDbConnectionFactory
    {
        public IDbConnection GetDbConnection();
    }
}