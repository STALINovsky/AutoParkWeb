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

        /// <summary>
        /// Returns select list of SpareParts
        /// </summary>
        /// <returns></returns>
        private async Task<List<SelectListItem>> GetSelectListOfSpareParts()
        {
            var spareParts = await sparePartRepository.GetSpareParts();
            return spareParts.Select(part => new SelectListItem(part.Name, part.Id.ToString())).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Add(int orderId)
        {
            ViewBag.SparePartList = await GetSelectListOfSpareParts();
            return View(new OrderItem() { OrderId = orderId });
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderItem orderItem)
        {
            await itemRepository.Add(orderItem);
            return RedirectToAction("Index", "Order", new { id = orderItem.OrderId });
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
            ViewBag.SparePartList = await GetSelectListOfSpareParts();

            var item = await itemRepository.Get(itemId);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Update(OrderItem orderItem)
        {
            await itemRepository.Update(orderItem);
            return RedirectToAction("Index", "Order", new { id = orderItem.OrderId });
        }

    }
}
