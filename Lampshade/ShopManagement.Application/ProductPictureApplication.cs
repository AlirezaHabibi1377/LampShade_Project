using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Application
{
    public class ProductPictureApplication : IProductPictureApplication
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductPictureRepository _productPictureRepository;
        private readonly IFileUploader _fileUploader;

        public ProductPictureApplication(IProductPictureRepository productPictureRepository, IProductRepository productRepository, IFileUploader fileUploader)
        {
            _productPictureRepository = productPictureRepository;
            _productRepository = productRepository;
            _fileUploader = fileUploader;
        }

        public OperationResult Create(CreateProductPicture command)
        {
            var opertaion = new OperationResult();
            //if (_productPictureRepository.Exists(x=>x.Picture == command.Picture
            //    && x.ProductId == command.ProductId))
            //{
            //    opertaion.Failed(ApplicationMessages.DuplicatedRecord);
            //}
            var product = _productRepository.GetProductWithCategory(command.ProductId);
            
            var path = $"{product.Category.Slug}//{product.Slug}";
            var picturePath = _fileUploader.Upload(command.Picture, path);
            var productPicture = new ProductPicture(command.ProductId, picturePath, command.PictureAlt, command.PictureTitle);
            _productPictureRepository.Create(productPicture);
            _productPictureRepository.SaveChanges();
            return opertaion.Succedded();
        }

        public OperationResult Edit(EditProductPicture command)
        {
            var opertaion = new OperationResult();
            var productPicture = _productPictureRepository.GetWithProductAndCategory(command.Id);
            if (productPicture == null)
            {
                opertaion.Failed(ApplicationMessages.RecordNotFound);
            }
            //if (_productPictureRepository.Exists(x => x.Picture == command.Picture
            //                                          && x.ProductId == command.ProductId
            //                                          && x.Id != command.Id))
            //{
            //    opertaion.Failed(ApplicationMessages.DuplicatedRecord);
            //}
            var path = $"{productPicture.Product.Category.Slug}//{productPicture.Product.Slug}";
            var picturePath = _fileUploader.Upload(command.Picture, path);
            productPicture.Edit(command.ProductId, picturePath, command.PictureAlt,command.PictureTitle);
            _productPictureRepository.SaveChanges();
            return opertaion.Succedded();
        }

        public OperationResult Remove(long id)
        {
            var opertaion = new OperationResult();
            var productPicture = _productPictureRepository.Get(id);
            if (productPicture == null)
            {
                opertaion.Failed(ApplicationMessages.RecordNotFound);
            }
            productPicture.Removed();
            _productPictureRepository.SaveChanges();
            return opertaion.Succedded();
        }

        public OperationResult Restore(long id)
        {
            var opertaion = new OperationResult();
            var productPicture = _productPictureRepository.Get(id);
            if (productPicture == null)
            {
                opertaion.Failed(ApplicationMessages.RecordNotFound);
            }
            productPicture.Restore();
            _productPictureRepository.SaveChanges();
            return opertaion.Succedded();
        }

        public EditProductPicture GetDetails(long id)
        {
            return _productPictureRepository.GetDetails(id);
        }

        public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
        {
            return _productPictureRepository.Search(searchModel);
        }
    }
}
