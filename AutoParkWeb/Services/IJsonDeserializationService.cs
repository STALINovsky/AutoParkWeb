using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoParkData.Model.Vehicles;
using Microsoft.AspNetCore.Http;

namespace AutoParkWeb.Services
{
    public interface IJsonDeserializationService
    {
        public Task<List<Vehicle>> ReadVehicles(IFormFile file);
        public Task<List<VehicleType>> ReadVehicleTypes(IFormFile file);
    }
}
