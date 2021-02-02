using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoParkData.Model.Vehicles;
using AutoParkData.Repositories.Base;

namespace AutoParkData.Repositories.Interfaces
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        public Task<IEnumerable<Vehicle>> GetVehicles();
    }
}
