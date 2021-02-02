using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using AutoParkData.Model.Base;

namespace AutoParkData.Model.Orders
{
    /// <summary>
    /// Class to represent spare part, which can be included in Order
    /// </summary>
    public class SparePart : Entity
    {
        [Required]
        public string Name { get; init; }
        [Required]
        public string Description { get; init; }
    }
}
