using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _01_LampshadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class LatestArrivalViewComponent : ViewComponent
    {
        private readonly IProductQuery _productQuery;

        public LatestArrivalViewComponent(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public IViewComponentResult Invoke()
        {
            var product = _productQuery.GetLatestArrivals();
            return View(product);
        }
    }
}
