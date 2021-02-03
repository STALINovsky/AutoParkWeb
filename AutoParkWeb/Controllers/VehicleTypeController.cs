using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoParkData.Model.Vehicles;
using AutoParkData.Repositories.Interfaces;
using Microsoft.VisualBasic.CompilerServices;

namespace AutoParkWeb.Controllers
{
    public class VehicleTypeController : Controller
    {
        private readonly IVehicleTypeRepository vehicleTypeRepository;

        public VehicleTypeController(IVehicleTypeRepository vehicleTypeRepository)
        {
            this.vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<IActionResult> List()
        {
            var data = await vehicleTypeRepository.GetVehicleTypes();
            return View(data);
        }
        
        [HttpGet]
        public IActionResult Add()
        {
            return View(new VehicleType());
        }

        [HttpPost]
        public async Task<IActionResult> Add(VehicleType vehicleType)
        {
            await vehicleTypeRepository.Add(vehicleType);
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var vehicleType = await vehicleTypeRepository.Get(id);
            return View(vehicleType);
        }

        [HttpPost]
        public async Task<IActionResult> Update(VehicleType vehicleType)
        {
            await vehicleTypeRepository.Update(vehicleType);
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await vehicleTypeRepository.Delete(id);
            return RedirectToAction(nameof(List));
        }
    }
}
