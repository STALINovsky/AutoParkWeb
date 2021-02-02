using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoParkData.Model.Base;
using AutoParkData.Model.Vehicles;

namespace AutoParkData.Model.Orders
{
    /// <summary>
    /// Class to represent Order of spare parts
    /// </summary>
    public class Order : Entity
    {
        public Vehicle TargetVehicle { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        [Required]
        public string Description { get; init; }
    }
}
