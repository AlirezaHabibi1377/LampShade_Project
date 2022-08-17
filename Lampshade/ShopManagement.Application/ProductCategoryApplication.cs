﻿using ShopManagement.Application.Contracts.ProductCategory;
using System;
using System.Collections.Generic;
using _0_Framework.Application;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application
{
    public class ProductCategoryApplication : IProductCategoryApplication
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryApplication(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public OperationResult Create(CreateProductCategory command)
        {
            var operation = new OperationResult();
            if (_productCategoryRepository.Exists(x=>x.Name == command.Name))
            {
                return operation.Failed("امکان اضافه کردن رکورد تکراری وجود ندارد. مجدد تلاش بفرمایید.");
            }

            var slug = command.Slug.Slugify();
            var productCategory = new ProductCategory(command.Name, command.Description,
                command.Picture, command.PictureAlt, command.PictureTitle, command.Keywords,
                command.MetaDescription, slug);

            _productCategoryRepository.Create(productCategory);
            _productCategoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditProductCategory command)
        {
            var operation = new OperationResult();
            var productCategory = _productCategoryRepository.Get(command.Id);
            if (productCategory == null)
            {
                return operation.Failed("رکورد با اطلاعات درخواست شده یافت نشد. لطفا مجدد تلاش بفرمایید.");
            }

            if (_productCategoryRepository.Exists(x=>x.Name == command.Name && x.Id == command.Id))
            {
                return operation.Failed("امکان اضافه کردن رکورد تکراری وجود ندارد. مجدد تلاش بفرمایید.");
            }

            var slug = command.Slug.Slugify();
            productCategory.Edit(command.Name, command.Description, command.Picture,
                command.PictureAlt, command.PictureTitle, command.Keywords, command.MetaDescription , slug);

            _productCategoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditProductCategory GetDetails(long id)
        {
            return _productCategoryRepository.GetDetails(id);
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            return _productCategoryRepository.Search(searchModel);
        }
    }
}