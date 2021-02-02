using System;
using System.ComponentModel.DataAnnotations;
using AutoParkData.Model.Base;

namespace AutoParkData.Model.Vehicles
{
    /// <summary>
    /// Class to represent Vehicle type
    /// </summary>
    public class VehicleType : Entity
    {
        [Required]
        public string TypeName { get; init; }
        [Range(1, int.MaxValue)]
        public decimal TaxCoefficient { get; init; }
    }
}