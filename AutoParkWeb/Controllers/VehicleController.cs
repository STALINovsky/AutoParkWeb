using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoParkData.Model.Vehicles;
using AutoParkData.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using AutoParkWeb.Models;

namespace AutoParkWeb.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository vehicleRepository;
        private readonly IVehicleTypeRepository typeRepository;
        public VehicleController(IVehicleRepository vehicleRepository, IVehicleTypeRepository typeRepository)
        {
            this.vehicleRepository = vehicleRepository;
            this.typeRepository = typeRepository;
        }

        public async Task<IActionResult> Index(int id)
        {
            var vehicle = await vehicleRepository.Get(id);
            return View(vehicle);
        }

        public async Task<IActionResult> List(VehicleSortingOptions sortingOptions = VehicleSortingOptions.Default)
        {
            var data = await vehicleRepository.GetVehicles();
            data = sortingOptions switch
            {
                VehicleSortingOptions.ByName => data.OrderBy(item => item.ModelName),
                VehicleSortingOptions.ByTypeName => data.OrderBy(item => item.VehicleType.TypeName),
                VehicleSortingOptions.ByMileage => data.OrderBy(item => item.Mileage),
                _ => data
            };

            return View(data);
        }

        private async Task<List<SelectListItem>> GetSelectVehicleTypes()
        {
            var vehicleTypes = await typeRepository.GetVehicleTypes();
            return vehicleTypes.Select(type => new SelectListItem(type.TypeName, type.Id.ToString())).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.TypeList = await GetSelectVehicleTypes();
            return View(new Vehicle() { VehicleType = new VehicleType() });
        }

        [HttpPost]
        public async Task<IActionResult> Add(Vehicle vehicle)
        {
            await vehicleRepository.Add(vehicle);
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.TypeList = await GetSelectVehicleTypes();

            var vehicleType = await vehicleRepository.Get(id);
            return View(vehicleType);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Vehicle vehicle)
        {
            await vehicleRepository.Update(vehicle);
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await vehicleRepository.Delete(id);
            return RedirectToAction(nameof(List));
        }
    }
}
