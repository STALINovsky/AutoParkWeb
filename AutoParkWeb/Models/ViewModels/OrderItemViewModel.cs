using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoParkData.Model.Orders;

namespace AutoParkWeb.Models.ViewModels
{
    public class OrderItemViewModel
    {
        public int OrderId { get; init; }
        public OrderItem OrderItem { get; set; }
    }
}
