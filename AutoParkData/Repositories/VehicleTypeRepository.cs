using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoParkData.Model.Vehicles;
using AutoParkData.Repositories.Base;
using AutoParkData.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AutoParkData.Repositories
{
    /// <summary>
    /// Repository for Work with Vehicle types
    /// </summary>
    public sealed class VehicleTypeRepository : Repository, IVehicleTypeRepository
    {
        public VehicleTypeRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<VehicleType> Get(int id)
        {
            return await Connection.QueryFirstAsync<VehicleType>("SELECT * FROM VehicleTypes WHERE Id = @id", new { id });
        }

        public async Task AddRange(IEnumerable<VehicleType> types)
        {
            await Connection.ExecuteAsync("INSERT INTO VehicleTypes(TypeName, TaxCoefficient) VALUES(@TypeName, @TaxCoefficient)", types);
        }

        public Task Add(VehicleType entity)
        {
            return AddRange(new[] {entity});
        }

        public async Task Update(VehicleType entity)
        {
            await Connection.ExecuteAsync(
                "UPDATE VehicleTypes SET TypeName = @TypeName, TaxCoefficient = @TaxCoefficient WHERE Id = @Id",
                entity);
        }

        public async Task Delete(int id)
        {
            await Connection.ExecuteAsync("DELETE FROM VehicleTypes WHERE Id = @id", new { id });
        }

        public async Task<IEnumerable<VehicleType>> GetVehicleTypes()
        {
            return await Connection.QueryAsync<VehicleType>("SELECT * FROM VehicleTypes");
        }
    }
}
