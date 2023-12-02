using AutoMapper;
using Unni.ToDo.Core.DTOs;
using Unni.ToDo.Core.Interfaces;
using Unni.ToDo.Core.Models;

namespace Unni.ToDo.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAdminUnitOfWork _unitOfWork;

        public AdminService(IAdminRepository repository, IMapper mapper, IAdminUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public CategoryDto AddCategory(AddCategoryRequest request)
        {
            var todoItem = _mapper.Map<CategoryEntity>(request);
            _repository.AddCategory(todoItem);
            _unitOfWork.SaveChanges();
            return _mapper.Map<CategoryDto>(todoItem);
        }

        public void DeleteCategory(int id)
        {
            _repository.DeleteCategory(id);
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            var items = _repository.GetAllCategories();
            var itemsDto = _mapper.Map<IEnumerable<CategoryDto>>(items);
            _unitOfWork.SaveChanges();
            return itemsDto;
        }

        public CategoryDto GetCategoryById(int id)
        {
            var item = _repository.GetCategoryById(id);
            _unitOfWork.SaveChanges();
            return _mapper.Map<CategoryDto>(item);
        }

        public CategoryDto UpdateCategory(CategoryDto category)
        {
            var item = _repository.GetCategoryById(category.Id);
            if (item != null)
            {
                item.Name = item.Name != category.Name ? category.Name : item.Name;
                item.Description = item.Description != category.Description ? category.Description : item.Description;
                _repository.UpdateCategory(item);
            }
            _unitOfWork.SaveChanges();
            return _mapper.Map<CategoryDto>(item);
        }
    }
}
