using System.Collections.Generic;
using System.Threading.Tasks;
using AutoParkData.Model.Orders;

namespace AutoParkData.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        public Task<IEnumerable<OrderItem>> GetOrderItems();
        public Task<OrderItem> Get(int id);
        public Task Add(OrderItem orderItem, int orderId);
        public Task Update(OrderItem item);
        public Task Delete(int id);
    }
}