using Microsoft.eShopWeb.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Interfaces
{
    public interface ICatalogItemViewModelService
    {
        Task UpdateCatalogItem(CatalogItemViewModel viewModel);
    }
}
