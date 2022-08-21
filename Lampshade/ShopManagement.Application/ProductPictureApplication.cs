using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Application
{
    public class ProductPictureApplication : IProductPictureApplication
    {
        private readonly IProductPictureRepository _productPictureRepository;

        public ProductPictureApplication(IProductPictureRepository productPictureRepository)
        {
            _productPictureRepository = productPictureRepository;
        }

        public OperationResult Create(CreateProductPicture command)
        {
            var opertaion = new OperationResult();
            if (_productPictureRepository.Exists(x=>x.Picture == command.Picture
                && x.ProductId == command.ProductId))
            {
                opertaion.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var productPicture = new ProductPicture(command.ProductId, command.Picture, command.PictureAlt, command.PictureTitle);
            _productPictureRepository.Create(productPicture);
            _productPictureRepository.SaveChanges();
            return opertaion.Succedded();
        }

        public OperationResult Edit(EditProductPicture command)
        {
            var opertaion = new OperationResult();
            var productPicture = _productPictureRepository.Get(command.Id);
            if (productPicture == null)
            {
                opertaion.Failed(ApplicationMessages.RecordNotFound);
            }
            if (_productPictureRepository.Exists(x => x.Picture == command.Picture
                                                      && x.ProductId == command.ProductId
                                                      && x.Id != command.Id))
            {
                opertaion.Failed(ApplicationMessages.DuplicatedRecord);
            }

            productPicture.Edit(command.ProductId,command.Picture,command.PictureAlt,command.PictureTitle);
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
