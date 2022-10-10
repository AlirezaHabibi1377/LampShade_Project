using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;

namespace ShopManagement.Configuration.Permissions
{
    public class ShopPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string, List<PermissionDto>>
            {
                {
                    "Product", new List<PermissionDto>
                    {
                    new PermissionDto("ListProducts", 10),
                    new PermissionDto("SearchProducts", 11),
                    new PermissionDto("CreateProduct", 12),
                    new PermissionDto("EditProduct", 13),
                    }
                },
                
                {
                    "ProductCategory", new List<PermissionDto>
                    {
                    new PermissionDto("ListProductCategories", 20),
                    new PermissionDto("SearchProductCategories", 21),
                    new PermissionDto("CreateProductCategory", 22),
                    new PermissionDto("EditProductCategory", 23),
                    }
                }
            };
        }
    }
}
