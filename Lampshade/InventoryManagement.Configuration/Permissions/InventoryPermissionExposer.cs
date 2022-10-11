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
                        new PermissionDto("ListInventory", InventoryPermissions.ListInventory),
                        new PermissionDto("SearchInventory", InventoryPermissions.SearchInventory),
                        new PermissionDto("CreateInventory", InventoryPermissions.CreateInventory),
                        new PermissionDto("EditInventory", InventoryPermissions.EditInventory),
                        new PermissionDto("Increase", InventoryPermissions.Increase),
                        new PermissionDto("Reduce", InventoryPermissions.Reduce),
                        new PermissionDto("OperationLog", InventoryPermissions.OperationLog),
                    }
                }
            };
        }
    }
}
