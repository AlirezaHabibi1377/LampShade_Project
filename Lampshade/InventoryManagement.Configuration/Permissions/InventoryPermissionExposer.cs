using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;

namespace InventoryManagement.Configuration.Permissions
{
    public class InventoryPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string, List<PermissionDto>>
            {
                {
                    "Inventory", new List<PermissionDto>
                    {
                        new PermissionDto("ListInventory", 50),
                        new PermissionDto("SearchInventory", 51),
                        new PermissionDto("CreateInventory", 52),
                        new PermissionDto("EditInventory", 53),
                    }
                }
            };
        }
    }
}
