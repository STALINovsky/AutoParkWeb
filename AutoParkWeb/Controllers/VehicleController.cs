using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoParkData.Model.Vehicles;
using AutoParkData.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AutoParkWeb.Models;
using AutoParkWeb.Models.ViewModels;
using AutoParkWeb.Services;

namespace AutoParkWeb.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository vehicleRepository;
        private readonly IVehicleTypeRepository typeRepository;
        private readonly IJsonDeserializationService deserializationService;
        public VehicleController(IVehicleRepository vehicleRepository, IVehicleTypeRepository typeRepository, IJsonDeserializationService deserializationService)
        {
            this.vehicleRepository = vehicleRepository;
            this.typeRepository = typeRepository;
            this.deserializationService = deserializationService;
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

        /// <summary>
        /// Returns Select list of Vehicle Types
        /// </summary>
        /// <returns></returns>
        private async Task<List<SelectListItem>> GetSelectListOfVehicleTypes()
        {
            var vehicleTypes = await typeRepository.GetVehicleTypes();
            return vehicleTypes.Select(type => new SelectListItem(type.TypeName, type.Id.ToString())).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.TypeList = await GetSelectListOfVehicleTypes();
            return View();
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
            ViewBag.TypeList = await GetSelectListOfVehicleTypes();

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

        [HttpGet]
        public IActionResult FromJson()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FromJson(JsonFileViewModel fileViewModel)
        {
            var vehicles = await deserializationService.ReadVehicles(fileViewModel.File);
            await vehicleRepository.AddRange(vehicles);

            return RedirectToAction(nameof(List));
        }
    }
}
