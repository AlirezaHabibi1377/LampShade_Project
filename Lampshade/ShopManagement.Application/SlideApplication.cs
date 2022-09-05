using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Application
{
    public class SlideApplication : ISlideApplication
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ISlideRepository _slideRepository;
        private readonly IFileUploader _fileUploader;

        public SlideApplication(ISlideRepository slideRepository, IProductCategoryRepository productCategoryRepository, IFileUploader fileUploader)
        {
            _slideRepository = slideRepository;
            _productCategoryRepository = productCategoryRepository;
            _fileUploader = fileUploader;
        }

        public OperationResult Create(CreateSlide command)
        {
            var operation = new OperationResult();

            var picturePath = _fileUploader.Upload(command.Picture, "slides");

            var slide = new Slide(picturePath, command.PictureAlt, command.Title,
                command.Heading, command.Title, command.Text, command.BtnText, command.Link);
            _slideRepository.Create(slide);
            _slideRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Edit(EditSlide command)
        {
            var operation = new OperationResult();
            var Slide = _slideRepository.Get(command.Id);

            if (Slide == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }
            var picturePath = _fileUploader.Upload(command.Picture, "slides");

            Slide.Edit(picturePath, command.PictureAlt, command.Title,
                command.Heading, command.Title, command.Text, command.BtnText, command.Link);
            _slideRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Remove(long id)
        {
            var operation = new OperationResult();
            var Slide = _slideRepository.Get(id);
            if (Slide == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }
            Slide.Remove();
            _slideRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Restore(long id)
        {
            var operation = new OperationResult();
            var Slide = _slideRepository.Get(id);
            if (Slide == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }
            Slide.Restore();
            _slideRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditSlide GetDetails(long id)
        {
            return _slideRepository.GetDetails(id);
        }

        public List<SlideViewModel> GetList()
        {
            return _slideRepository.GetList();
        }
    }
}
