using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoParkData.Model.Orders;
using AutoParkData.Repositories.Interfaces;

namespace AutoParkWeb.Controllers
{
    public class SparePartController : Controller
    {
        private readonly ISparePartRepository repository;

        public SparePartController(ISparePartRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> List()
        {
            var spareParts = await repository.GetSpareParts();
            return View(spareParts);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new SparePart());
        }

        [HttpPost]
        public async Task<IActionResult> Add(SparePart sparePart)
        {
            if (ModelState.IsValid)
            {
                await repository.Add(sparePart);
                return RedirectToAction(nameof(List));
            }

            return View(sparePart);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var sparePart = await repository.Get(id);
            return View(sparePart);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SparePart sparePart)
        {
            await repository.Update(sparePart);
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await repository.Delete(id);
            return RedirectToAction(nameof(List));
        }
    }
}
