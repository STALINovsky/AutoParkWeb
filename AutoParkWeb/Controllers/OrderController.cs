using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoParkData.Model.Orders;
using AutoParkData.Repositories.Interfaces;
using AutoParkWeb.Models;
using AutoParkWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoParkWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly IVehicleRepository vehicleRepository;
        private readonly IOrderRepository orderRepository;
        public OrderController(IOrderRepository orderRepository, IVehicleRepository vehicleRepository)
        {
            this.orderRepository = orderRepository;
            this.vehicleRepository = vehicleRepository;
        }

        private async Task<List<SelectListItem>> GetSelectVehicles()
        {
            var vehicles = await vehicleRepository.GetVehicles();
            return vehicles.Select(vehicle =>
                new SelectListItem($"{vehicle.ModelName}: {vehicle.RegistrationNumber}", vehicle.Id.ToString())).ToList();
        }

        public async Task<IActionResult> Index(int id)
        {
            var order = await orderRepository.Get(id);
            return View(order);
        }

        public async Task<IActionResult> List()
        {
            var data = await orderRepository.GetOrders();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.VehicleList = await GetSelectVehicles();
            return View(new Order());
        }

        [HttpPost]
        public async Task<IActionResult> Add(Order order)
        {
            await orderRepository.Add(order);
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.VehicleList = await GetSelectVehicles();

            var order = await orderRepository.Get(id);
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Order order)
        {
            await orderRepository.Update(order);
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await orderRepository.Delete(id);

            return RedirectToAction(nameof(List));
        }

    }
}
