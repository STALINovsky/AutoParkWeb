using System.Collections.Generic;
using System.Threading.Tasks;
using AutoParkData.Model.Orders;
using AutoParkData.Repositories.Base;

namespace AutoParkData.Repositories.Interfaces
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        public Task<IEnumerable<OrderItem>> GetOrderItems();
    }
}