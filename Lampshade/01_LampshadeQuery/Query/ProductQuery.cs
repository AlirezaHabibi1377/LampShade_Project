using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Product;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query
{
    public class ProductQuery : IProductQuery
    {
        private readonly DiscountContext _discountContext;
        private readonly ShopContext _context;
        private readonly InventoryContext _inventoryContext;

        public ProductQuery(ShopContext context, InventoryContext inventoryContext, DiscountContext discountContext)
        {
            _context = context;
            _inventoryContext = inventoryContext;
            _discountContext = discountContext;
        }

        public List<ProductQueryModel> GetLatestArrivals()
        {
            var inventory = _inventoryContext.Inventory
                .Select(x => new
                {
                    x.ProductId,
                    x.UnitPrice
                })
                .ToList();

            var discounts = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new
                {
                    x.DiscountRate,
                    x.ProductId
                })
                .ToList();

            var products = _context.Products
                .Include(x => x.Category)
                .Select(product => new ProductQueryModel
                {
                    Id = product.Id,
                    Category = product.Category.Name,
                    Name = product.Name,
                    Picture = product.Picture,
                    PictureAlt = product.PictureAlt,
                    PictureTitle = product.PictureTitle,
                    Slug = product.Slug
                }).OrderByDescending(x=>x.Id).Take(6).ToList();

            foreach (var product in products)
            {
                var ProductInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                if (ProductInventory != null)
                {
                    var price = ProductInventory.UnitPrice;
                    product.Price = price.ToMoney();
                    var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                    if (discount != null)
                    {
                        int discountRate = discount.DiscountRate;
                        product.DiscountRate = discountRate;

                        if (product.DiscountRate > 0)
                        {
                            product.HasDiscount = true;
                        }
                        else
                        {
                            product.HasDiscount = false;
                        }

                        var discountAmount = Math.Round((price * product.DiscountRate) / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }

            return products;

        }

        public List<ProductQueryModel> Search(string value)
        {
            var inventory = _inventoryContext.Inventory
                .Select(x => new
                {
                    x.ProductId,
                    x.UnitPrice,
                })
                .ToList();

            var discounts = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new
                {
                    x.DiscountRate,
                    x.ProductId,
                    x.EndDate
                })
                .ToList();

            var query = _context.Products
                .Include(a => a.Category)
                .Select(x => new ProductQueryModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Category = x.Category.Name,
                    ShortDescription = x.ShortDescription,
                    CategorySlug = x.Category.Slug,
                    Slug = x.Slug,
                }).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(value))
            {
                query = query.Where(x => x.Name.Contains(value) || x.ShortDescription.Contains(value));
            }

            var products = query.OrderByDescending(x => x.Id).ToList();

            foreach (var product in products)
            {
                var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;
                    product.Price = price.ToMoney();
                    var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                    if (discount != null)
                    {
                        int discountRate = discount.DiscountRate;
                        product.DiscountRate = discountRate;
                        product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                        if (product.DiscountRate > 0)
                        {
                            product.HasDiscount = true;
                        }
                        else
                        {
                            product.HasDiscount = false;
                        }

                        var discountAmount = Math.Round((price * product.DiscountRate) / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }
            return products;
        }
    }
}
