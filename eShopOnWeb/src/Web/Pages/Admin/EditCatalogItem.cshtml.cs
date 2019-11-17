using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Constants;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopOnWeb.Web.Pages.Admin
{
    [Authorize(Roles = AuthorizationConstants.Roles.ADMINISTRATORS)]
    public class EditCatalogItemModel : PageModel
    {
        private readonly ICatalogItemViewModelService _catalogItemViewModelService;

        public EditCatalogItemModel(ICatalogItemViewModelService catalogItemViewModelService)
        {
            _catalogItemViewModelService = catalogItemViewModelService;
        }

        [BindProperty]
        public CatalogItemViewModel CatalogModel { get; set; } = new CatalogItemViewModel();

        public async Task OnGet(CatalogItemViewModel catalogModel)
        {
            CatalogModel = catalogModel;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _catalogItemViewModelService.UpdateCatalogItem(CatalogModel);
            }

            return RedirectToPage("/Admin/Index");
        }
    }
}