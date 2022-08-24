﻿using ShopManagement.Application.Contracts.Product;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;

namespace DiscountManagement.Application.Contracts.CustomerDiscount
{
    public class DefineCustomerDiscount
    {
        [Range(1, 100000, ErrorMessage = ValidationMessage.IsRequired)]
        public long ProductId { get; set; }

        [Range(1, 99, ErrorMessage = ValidationMessage.IsRequired)]
        public int DiscountRate { get; set; }

        [Required(ErrorMessage = ValidationMessage.IsRequired)]
        public string StartDate { get; set; }

        [Required(ErrorMessage = ValidationMessage.IsRequired)]
        public string EndDate { get; set; }
        public string Reason { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
