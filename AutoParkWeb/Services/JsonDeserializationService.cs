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
    public class JsonDeserializationService : IJsonDeserializationService
    {

        public async Task<List<Vehicle>> ReadVehicles(IFormFile file)
        {
            var text = await new StreamReader(file.OpenReadStream()).ReadToEndAsync();
            return JsonSerializer.Deserialize<List<Vehicle>>(text);
        }

        public async Task<List<VehicleType>> ReadVehicleTypes(IFormFile file)
        {
            var text = await new StreamReader(file.OpenReadStream()).ReadToEndAsync();
            return JsonSerializer.Deserialize<List<VehicleType>>(text);
        }
    }
}
