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
                    new PermissionDto("ListProducts", ShopPermissions.ListProducts),
                    new PermissionDto("SearchProducts", ShopPermissions.SearchProducts),
                    new PermissionDto("CreateProduct", ShopPermissions.CreateProduct),
                    new PermissionDto("EditProduct", ShopPermissions.EditProduct),
                    }
                },
                
                {
                    "ProductCategory", new List<PermissionDto>
                    {
                    new PermissionDto("ListProductCategories", ShopPermissions.ListProductCategories),
                    new PermissionDto("SearchProductCategories", ShopPermissions.SearchProductCategories),
                    new PermissionDto("CreateProductCategory", ShopPermissions.CreateProductCategory),
                    new PermissionDto("EditProductCategory", ShopPermissions.EditProductCategory),
                    }
                }
            };
        }
    }
}
