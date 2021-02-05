using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace AutoParkData
{
    /// <summary>
    /// Provides functionality to create database by SqlScript, script needs to be defined as template
    /// dataBase name as DataBaseName
    /// </summary>
    public static class DbCreator
    {
        /// <summary>
        /// Create new database by template sql script
        /// </summary>
        /// <param name="server">Sql server</param>
        /// <param name="sql">template sql code</param>
        /// <param name="dataBaseName">Name of DataBase</param>
        private static void CreateDatabase(Server server, string sql, string dataBaseName)
        {
            //Replacing template values to actual values
            var preparedSql = sql.Replace("DataBaseName", dataBaseName);
            //execute prepared script 
            server.ConnectionContext.ExecuteNonQuery(preparedSql);
        }

        /// <summary>
        /// Create database if it isn't exist, by template sql script file
        /// </summary>
        /// <param name="dbConnectionString">Connection string to desired database</param>
        /// <param name="sqlScriptPath">Path to sql script</param>
        public static void EnsureDbCreated(string dbConnectionString, string sqlScriptPath)
        {
            //get database name
            var dataBaseName = new SqlConnectionStringBuilder(dbConnectionString).InitialCatalog;
            //get connection string to sql Server
            var serverConnectionString = new SqlConnectionStringBuilder(dbConnectionString) { InitialCatalog = string.Empty }.ToString();

            //create connection and server by connection
            using var sqlConnection = new SqlConnection(serverConnectionString);
            var server = new Server(new ServerConnection(sqlConnection));

            //add database if it isn't exist
            if (!server.Databases.Contains(dataBaseName))
            {
                //get sql script from file
                string sql = File.ReadAllText(sqlScriptPath);
                CreateDatabase(server, sql, dataBaseName);
            }
        }
    }
}
