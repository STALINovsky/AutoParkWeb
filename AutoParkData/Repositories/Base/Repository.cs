using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoParkData.Model.Base;
using Microsoft.Data.SqlClient;

namespace AutoParkData.Repositories.Base
{
    /// <summary>
    /// Base implementation of Repository, which contains db connection and realize IDisposable
    /// </summary>
    public abstract class Repository : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Ctor that init Sql connection
        /// </summary>
        /// <param name="connectionString">Connection string to DB</param>
        protected Repository(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }
        protected readonly DbConnection Connection;

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return Connection.DisposeAsync();
        }
    }
}