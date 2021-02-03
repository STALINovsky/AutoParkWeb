using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoParkData.Model.Orders;
using AutoParkData.Repositories.Base;
using AutoParkData.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AutoParkData.Repositories
{
    /// <summary>
    /// Repository for work with Spare Parts
    /// </summary>
    public class SparePartRepository : Repository, ISparePartRepository
    {
        public SparePartRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<IEnumerable<SparePart>> GetSpareParts()
        {
            return await Connection.QueryAsync<SparePart>("SELECT * FROM SpareParts");
        }

        public async Task<SparePart> Get(int id)
        {
            return await Connection.QuerySingleAsync<SparePart>(
                "SELECT * FROM SpareParts WHERE Id = @id",
                new { id });
        }

        public async Task Update(SparePart entity)
        {
            await Connection.ExecuteAsync(
                "UPDATE SpareParts SET Name = @Name, Description = @Description WHERE Id = @Id",
                entity);
        }

        public async Task Delete(int id)
        {
            await Connection.ExecuteAsync("DELETE FROM SpareParts WHERE Id = @id", new { id });
        }

        public async Task Add(SparePart entity)
        {
            await Connection.ExecuteAsync(
                "INSERT INTO SpareParts(Name, Description) VALUES(@Name, @Description)",
                entity);
        }
    }
}
