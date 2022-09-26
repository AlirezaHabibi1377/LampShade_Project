using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Product;
using DiscountManagement.Infrastructure.EFCore;
using CommentManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query
{
    public class ProductQuery : IProductQuery
    {
        private readonly DiscountContext _discountContext;
        private readonly ShopContext _context;
        private readonly InventoryContext _inventoryContext;
        private readonly CommentContext _commentContext;

        public ProductQuery(ShopContext context, InventoryContext inventoryContext, DiscountContext discountContext, CommentContext commentContext)
        {
            _context = context;
            _inventoryContext = inventoryContext;
            _discountContext = discountContext;
            _commentContext = commentContext;
        }

        public ProductQueryModel GetProductDetails(string slug)
        {
            var inventory = _inventoryContext.Inventory
                .Select(x => new
                {
                    x.ProductId,
                    x.UnitPrice,
                    x.InStock
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

            var product = _context.Products
                .Include(x => x.Category)
                .Include(x => x.ProductPictures)
                .Select(product => new ProductQueryModel
                {
                    Id = product.Id,
                    Category = product.Category.Name,
                    Name = product.Name,
                    Picture = product.Picture,
                    PictureAlt = product.PictureAlt,
                    PictureTitle = product.PictureTitle,
                    Slug = product.Slug,
                    CategorySlug = product.Category.Slug,
                    Code = product.Code,
                    Description = product.Description,
                    ShortDescription = product.ShortDescription,
                    MetaDescription = product.MetaDescription,
                    keywords = product.Keywords,
                    Pictures = MapProductPictures(product.ProductPictures)
                }).FirstOrDefault(x => x.Slug == slug);

            if (product == null)
            {
                return new ProductQueryModel();
            }

            var ProductInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
            if (ProductInventory != null)
            {
                product.IsInStock = ProductInventory.InStock;
                var price = ProductInventory.UnitPrice;

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

            product.Comments = _commentContext.Comments
                .Where(x=>x.Type == CommentType.Product)
                .Where(x => !x.IsCanceled)
                .Where(x => x.IsConfirmed)
                .Where(x=>x.OwnerRecordId == product.Id)
                .Select(x=>new CommentQueryModel()
                {
                    Id = x.Id,
                    Message = x.Message,
                    Name = x.Name,
                    CreationDate = DateTime.Now.ToFarsi()
                })
                .ToList();

            return product;
        }

        //private static List<CommentQueryModel> MapComment(List<Comment> comments)
        //{
        //    return comments.Where(x => !x.IsCanceled).Where(x => x.IsConfirmed).Select(x => new CommentQueryModel
        //    {
        //        Id = x.Id,
        //        Message = x.Message,
        //        Name = x.Name
        //    }).OrderByDescending(x=>x.Id).ToList();
        //}

        private static List<ProductPictureQueryModel> MapProductPictures(List<ProductPicture> pictures)
        {
            return pictures.Select(x => new ProductPictureQueryModel
            {
                IsRemoved = x.IsRemoved,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ProductId = x.ProductId
            }).Where(x => !x.IsRemoved).ToList();
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
                }).OrderByDescending(x => x.Id).Take(6).ToList();

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