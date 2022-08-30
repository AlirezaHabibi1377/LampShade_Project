using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _01_LampshadeQuery.Contracts.Product;
using _01_LampshadeQuery.Contracts.ProductCategory;
using _01_LampshadeQuery.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class SearchModel : PageModel
    {
        public string Value;
        public List<ProductQueryModel> Products;
        public ProductCategoryQueryModel ProductCategory;
        private readonly IProductQuery _productQuery;

        public SearchModel(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public void OnGet(string value)
        {
            Value = value;
            Products = _productQuery.Search(value);
        }
    }
}
