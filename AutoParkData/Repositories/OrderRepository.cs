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
        public OrderRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        /// <summary>
        /// Load Orders From database and map data on Order
        /// </summary>
        /// <param name="sql">Sql code to load orders and dependencies, need to select data in correct Order(Order,OrderItem,SparePart, Vehicle)</param>
        /// <param name="sqlParam">sql arguments</param>
        /// <returns>Mapped orders</returns>
        private async Task<IEnumerable<Order>> LoadOrders(string sql, object sqlParam = null)
        {
            //Load data from database 
            var orders = await Connection.QueryAsync<Order, OrderItem, SparePart, Vehicle, (Order order, OrderItem item)>
            (sql, (order, orderItem, sparePart, vehicle) =>
                {
                    order.TargetVehicle = vehicle;

                    if (orderItem != null)
                    {
                        orderItem.SparePart = sparePart;
                    }

                    return (order, orderItem);
                }, sqlParam);

            //group data in orders 
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
                entity);
        }

        public async Task Delete(int id)
        {
            await Connection.ExecuteAsync("DELETE FROM Orders WHERE Id = @id", new { id });
        }

        public async Task Add(Order entity)
        {
            await Connection.ExecuteAsync("INSERT INTO Orders(VehicleId, Description) VALUES(@VehicleId, @Description)", entity);
        }
    }
}
