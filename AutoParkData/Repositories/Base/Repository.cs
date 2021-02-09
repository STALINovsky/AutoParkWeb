using System;
using System.Collections.Generic;
using System.Data;
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
    public abstract class Repository : IDisposable
    {
        /// <summary>
        /// Init connection by connection factory
        /// </summary>
        /// <param name="connectionFactory"></param>
        protected Repository(IDbConnectionFactory connectionFactory)
        {
            Connection = connectionFactory.GetDbConnection();
        }
        protected readonly IDbConnection Connection;

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}