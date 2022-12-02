﻿using ShopManagement.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using ShopManagement.Domain.OrderAgg;
using InventoryManagement.Application.Contracts.Inventory;

namespace ShopManagement.Infrastructure.InventoryAcl
{
    public class ShopInventoryAcl : IShopInventoryAcl
    {
        private readonly IInventoryApplication _inventoryApplication;

        public ShopInventoryAcl(IInventoryApplication inventoryApplication)
        {
            _inventoryApplication = inventoryApplication;
        }

        public bool ReduceFromInventory(List<OrderItem> items)
        {
            var command = items.Select(orderItem =>
                    new ReduceInventory(orderItem.ProductId, orderItem.Count, "خرید مشتری", orderItem.OrderId))
                .ToList();

            return _inventoryApplication.Reduce(command).IsSuccedded;
        }
    }
}