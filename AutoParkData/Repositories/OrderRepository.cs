using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoParkData.Model.Orders;
using AutoParkData.Model.Vehicles;
using AutoParkData.Repositories.Base;
using AutoParkData.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace AutoParkData.Repositories
{
    /// <summary>
    /// Repository for work with Orders
    /// </summary>
    public class OrderRepository : Repository, IOrderRepository
    {
        public OrderRepository(string connectionString) : base(connectionString)
        {
        }

        private async Task<IEnumerable<Order>> LoadOrders(string sql, object sqlParam = null)
        {
            var orders = await Connection.QueryAsync<Order, OrderItem, SparePart, Vehicle, (Order order, OrderItem item)>
                (sql, (order, orderItem, sparePart, vehicle) =>
                {
                    order.TargetVehicle = vehicle;

                    if (orderItem != null)
                    {
                        orderItem.SparePart = sparePart;
                    }

                    return (order, orderItem);
                },sqlParam);

            return orders.GroupBy(tuple => tuple.order.Id).Select(tuples =>
           {
               var groupedOrder = tuples.First().order;
               groupedOrder.OrderItems = (tuples.Select(tuple => tuple.item).Where(item => item != null)).ToList();
               return groupedOrder;
           });
        }

        public Task<IEnumerable<Order>> GetOrders()
        {
            const string sql = "SELECT O.*, oi.*, sp.*, oi.Count, v.* FROM Orders O " +
                               "LEFT JOIN OrderItems oi on O.Id = oi.OrderId " +
                               "LEFT JOIN SpareParts sp on oi.SparePartId = sp.Id " +
                               "JOIN Vehicles v on v.Id = o.VehicleId";

            return LoadOrders(sql);
        }

        public async Task<Order> Get(int id)
        {
            const string sql = "SELECT O.*, oi.*, sp.*, oi.Count, v.* FROM Orders O " +
                               "LEFT JOIN OrderItems oi on O.Id = oi.OrderId " +
                               "LEFT JOIN SpareParts sp on oi.SparePartId = sp.Id " +
                               "JOIN Vehicles v on v.Id = o.VehicleId " +
                               "Where O.Id = @id";

            return (await LoadOrders(sql, new { id })).Single();
        }

        public async Task Update(Order entity)
        {
            await Connection.ExecuteAsync(
                "UPDATE Orders SET " +
                "VehicleId = @VehicleId, " +
                "Description = @Description " +
                "WHERE Id = @Id",
                new
                {
                    entity.Id,
                    VehicleId = entity.TargetVehicle.Id,
                    entity.Description
                });
        }

        public async Task Delete(int id)
        {
            await Connection.ExecuteAsync("DELETE FROM Orders WHERE Id = @id", new { id });
        }

        public async Task Add(Order entity)
        {
            await Connection.ExecuteAsync(
                "INSERT INTO Orders(VehicleId, Description) VALUES(@VehicleId, @Description)",
                new
                {
                    VehicleId = entity.TargetVehicle.Id,
                    entity.Description
                }
            );
        }
    }
}
