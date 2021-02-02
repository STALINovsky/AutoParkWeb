using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoParkData.Model.Base;

namespace AutoParkData.Model.Vehicles
{
    /// <summary>
    /// Class to represent Vehicle
    /// </summary>
    public class Vehicle : Entity
    {
        public VehicleType VehicleType { get; set; }
        [Required]
        public string ModelName { get; init; }
        [Required]
        public string RegistrationNumber { get; init; }
        /// <summary>
        /// Weight in KG
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Weight { get; init; }
        [Required]
        public int ManufactureYear { get; init; }
        /// <summary>
        /// Mileage in kilometers 
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Mileage { get; init; }
        [Required]
        public string Color { get; init; }
        /// <summary>
        /// Volume in liters or kilowatts
        /// </summary>
        [Range(1, int.MaxValue)]
        public double VolumeOfTank { get; init; }

        /// <summary>
        /// Consumption per 100 kilometers 
        /// </summary>
        [Range(1, int.MaxValue)]
        public double Consumption { get; init; }

        /// <summary>
        /// max Driving range of vehicle
        /// </summary>
        public double DrivingRange => 100 * VolumeOfTank / Consumption;

        private const decimal MinimalTax = 5;
        private const decimal TaxWeightCoefficient = 0.0013m;
        /// <summary>
        /// Calculate Taxes by weight and vehicle type
        /// </summary>
        /// <returns>Tax per 1 month</returns>
        public decimal CalculateTaxPerMonth() => Weight * TaxWeightCoefficient + VehicleType.TaxCoefficient  * 30 + MinimalTax;
    }
}