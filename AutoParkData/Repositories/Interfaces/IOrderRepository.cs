using System.Collections.Generic;
using System.Threading.Tasks;
using AutoParkData.Model.Orders;
using AutoParkData.Repositories.Base;

namespace AutoParkData.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrders();
    }
}