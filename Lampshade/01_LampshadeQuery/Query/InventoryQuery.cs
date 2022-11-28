using System.Linq;
using _01_LampshadeQuery.Contracts.Inventory;
using InventoryManagement.Infrastructure.EFCore;
using ShopManagement.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query
{
    public class InventoryQuery : IInventoryQuery
    {
        private readonly InventoryContext _inventoryContext;
        private readonly ShopContext _shopContext;

        public InventoryQuery(InventoryContext inventoryContext, ShopContext shopContext)
        {
            _inventoryContext = inventoryContext;
            _shopContext = shopContext;
        }

        public StockStatus CheckStock(IsInStock command)
        {
            var product = _shopContext.Products.Select(x => new { x.Id, x.Name });
            var inventory = _inventoryContext.Inventory.FirstOrDefault(x => x.ProductId == command.ProductId);
            if (inventory == null || inventory.CalculateCurrentCount() < command.Count)
            {
                
                return new StockStatus
                {
                    IsStock = false,
                    ProductName = product.FirstOrDefault(x=>x.Id == command.ProductId)?.Name
                };
            }

            return new StockStatus
            {
                IsStock = true,
                ProductName = product.FirstOrDefault(x => x.Id == command.ProductId)?.Name
            };
        }
    }
}
