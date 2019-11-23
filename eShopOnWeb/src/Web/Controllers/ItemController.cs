using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Web.Controllers
{
    public class ItemController : Controller
    {
        private readonly ICatalogViewModelService _catalogViewModelService;

        public ItemController(ICatalogViewModelService catalogViewModelService) => _catalogViewModelService = catalogViewModelService;

        public IActionResult Index()
        {
            return View();
        }

       [HttpPost]
       public IActionResult AddItem()
       {
            CatalogItemViewModel item = new CatalogItemViewModel();
            _catalogViewModelService.AddItem(item);
            return View();
       }
    }
}