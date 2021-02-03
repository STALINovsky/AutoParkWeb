using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoParkData.Model.Orders;
using AutoParkData.Repositories.Interfaces;
using AutoParkWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoParkWeb.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly IOrderItemRepository itemRepository;
        private readonly ISparePartRepository sparePartRepository;

        public OrderItemController(IOrderItemRepository itemRepository, ISparePartRepository sparePartRepository)
        {
            this.itemRepository = itemRepository;
            this.sparePartRepository = sparePartRepository;
        }

        private async Task<List<SelectListItem>> GetSelectSparePartsList()
        {
            var spareParts = await sparePartRepository.GetSpareParts();
            return spareParts.Select(part => new SelectListItem(part.Name, part.Id.ToString())).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Add(int orderId)
        {
            ViewBag.SparePartList = await GetSelectSparePartsList();
            return View(new OrderItemViewModel() { OrderId = orderId });
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderItemViewModel viewModel)
        {
            await itemRepository.Add(viewModel.OrderItem, viewModel.OrderId);
            return RedirectToAction("Index", "Order", new { id = viewModel.OrderId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int orderId, int orderItemId)
        {
            await itemRepository.Delete(orderItemId);
            return RedirectToAction("Index", "Order", new { id = orderId });
        }

        [HttpGet]
        public async Task<IActionResult> Update(int orderId, int itemId)
        {
            ViewBag.SparePartList = await GetSelectSparePartsList();

            var item = await itemRepository.Get(itemId);
            return View(new OrderItemViewModel() { OrderId = orderId, OrderItem = item });
        }

        [HttpPost]
        public async Task<IActionResult> Update(OrderItemViewModel viewModel)
        {
            await itemRepository.Update(viewModel.OrderItem);
            return RedirectToAction("Index", "Order", new { id = viewModel.OrderId });
        }

    }
}
