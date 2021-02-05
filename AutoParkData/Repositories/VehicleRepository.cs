using System;
using System.Collections.Generic;
using System.Data;
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
    /// Repository for vehicles 
    /// </summary>
    public class VehicleRepository : Repository, IVehicleRepository
    {
        public VehicleRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<IEnumerable<Vehicle>> GetVehicles()
        {
            return await Connection.QueryAsync<Vehicle, VehicleType, Vehicle>(
                "SELECT * " +
                "FROM Vehicles v " +
                "LEFT JOIN VehicleTypes vt " +
                "ON v.VehicleTypeId = vt.Id ", ((vehicle, vehicleType) =>
                {
                    vehicle.VehicleType = vehicleType;
                    return vehicle;
                }));
        }

        public async Task<Vehicle> Get(int id)
        {
            var query = await Connection.QueryAsync<Vehicle, VehicleType, Vehicle>(
                @"SELECT * 
                    FROM Vehicles v 
                    JOIN VehicleTypes vt 
                    ON v.VehicleTypeId = vt.Id 
                    WHERE v.Id = @id",
                (vehicle, type) =>
                {
                    vehicle.VehicleType = type;
                    return vehicle;
                }, new { id }
            );

            return query.Single();
        }

        public async Task Update(Vehicle entity)
        {
            await Connection.ExecuteAsync(
                "UPDATE Vehicles SET " +
                "ModelName = @ModelName, " +
                "VehicleTypeId = @VehicleTypeId, " +
                "Color = @Color, " +
                "ManufactureYear = @ManufactureYear, " +
                "Mileage = @Mileage, " +
                "RegistrationNumber = @RegistrationNumber, " +
                "Weight = @Weight, " +
                "VolumeOfTank = @VolumeOfTank " +
                "WHERE Id = @Id",
                new
                {
                    entity.Id,
                    entity.ModelName,
                    VehicleTypeId = entity.VehicleType.Id,
                    entity.Color,
                    entity.ManufactureYear,
                    entity.Mileage,
                    entity.RegistrationNumber,
                    entity.Weight,
                    entity.VolumeOfTank
                }
            );
        }

        public async Task Delete(int id)
        {
            await Connection.ExecuteAsync("DELETE FROM Vehicles WHERE Id = @id", new { id });
        }

        public async Task Add(Vehicle entity)
        {
            const string sql = "INSERT INTO Vehicles " +
                               "(ModelName, VehicleTypeId, Color, ManufactureYear, Mileage, RegistrationNumber, Weight, VolumeOfTank, Consumption) " +
                               "VALUES (@ModelName, @VehicleTypeId, @Color, @ManufactureYear, @Mileage, @RegistrationNumber, @Weight, @VolumeOfTank, @Consumption) ";
            await Connection.ExecuteAsync(
                sql,
                new
                {
                    entity.ModelName,
                    VehicleTypeId = entity.VehicleType.Id,
                    entity.Color,
                    entity.ManufactureYear,
                    entity.Mileage,
                    entity.RegistrationNumber,
                    entity.Weight,
                    entity.VolumeOfTank,
                    entity.Consumption
                }
            );
        }
    }
}
