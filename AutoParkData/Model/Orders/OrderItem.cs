using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoParkData.Model.Base;

namespace AutoParkData.Model.Orders
{
    /// <summary>
    /// Class to represent item of Order 
    /// </summary>
    public class OrderItem : Entity
    {
        public int OrderId { get; set; }

        public int SparePartId { get; set; }
        public SparePart SparePart { get; set; }
        /// <summary>
        /// Count of Spare Part
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Count { get; init; }
    }
}
