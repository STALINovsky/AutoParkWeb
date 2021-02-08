using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoParkData.Model.Orders;
using AutoParkData.Repositories.Base;
using AutoParkData.Repositories.Interfaces;
using Dapper;

namespace AutoParkData.Repositories
{
    /// <summary>
    /// Repository for work with order items  
    /// </summary>
    public class OrderItemRepository : Repository, IOrderItemRepository
    {
        public OrderItemRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        private async Task<IEnumerable<OrderItem>> LoadOrderItems(string sql, object param = null)
        {
            return await Connection.QueryAsync<OrderItem, SparePart, OrderItem>(sql, (item, part) =>
            {
                item.SparePart = part;
                return item;
            }, param);
        }

        public Task<IEnumerable<OrderItem>> GetOrderItems()
        {
            const string sql = "SELECT oi.*, sp.* FROM OrderItems oi " +
                               "Left JOIN SpareParts sp on oi.SparePartId = sp.Id ";
            return LoadOrderItems(sql);
        }

        public async Task<OrderItem> Get(int id)
        {
            const string sql = "SELECT oi.*, sp.* FROM OrderItems oi " +
                               "Left JOIN SpareParts sp on oi.SparePartId = sp.Id " +
                               "Where oi.Id = @id";

            return (await LoadOrderItems(sql, new { id })).Single();
        }

        public Task Update(OrderItem entity)
        {
            const string sql = "Update OrderItems Set " +
                               "SparePartId = @SparePartId," +
                               "Count = @Count " +
                               "Where Id = @Id";

            return Connection.ExecuteAsync(sql, new { SparePartId = entity.SparePart.Id, entity.Count, entity.Id });
        }

        public Task Delete(int id)
        {
            const string sql = "DELETE FROM OrderItems WHERE Id = @id";
            return Connection.ExecuteAsync(sql, new { id });
        }

        public Task Add(OrderItem entity, int orderId)
        {
            const string sql = "INSERT INTO OrderItems (OrderId, SparePartId, Count) VALUES(@orderId, @sparePartId, @Count)";
            return Connection.ExecuteAsync(sql, new { orderId, sparePartId = entity.SparePart.Id, entity.Count });
        }
    }
}
