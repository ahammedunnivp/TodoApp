using AutoMapper;
using Unni.Todo.Application.DTOs;
using Unni.Todo.Application.Interfaces;
using Unni.Todo.Domain.Entities;

namespace Unni.Todo.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;
        private readonly IMapper _mapper;
        private readonly ITodoUnitOfWork _unitOfWork;

        public TodoService(ITodoRepository repository, IMapper mapper, ITodoUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public TodoItemDto AddToDoItem(CreateTodoRequest request)
        {
            var todoItem = _mapper.Map<TodoItemEntity>(request);
            _repository.Add(todoItem);
            _unitOfWork.SaveChanges();
            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public void DeleteToDoItemById(int id)
        {
            _repository.Delete(id);
            _unitOfWork.SaveChanges();
        }

        public PaginatedResponseDto<TodoItemDto> Search(GetTodoRequest request)
        {
            var filter = request.IsFilter ? request.Filter : null;
            var pagination = new Pagination();

            if (request?.Pagination != null)
            {
                pagination.Page = request.Pagination.Page < 1 ? 1 : request.Pagination.Page;
                if (request.Pagination.PageSize > 60)
                    request.Pagination.PageSize = 60;
                if (request.Pagination.PageSize < 1)
                    request.Pagination.PageSize = 3;
            }
            else
            {
                request.Pagination = new Pagination
                {
                    PageSize = 20,
                    Page = 1
                };
            }
            (var todoItems, int total_count) = _repository.Search(request.Pagination, filter);
            var items = _mapper.Map<IEnumerable<TodoItemDto>>(todoItems);

            return new PaginatedResponseDto<TodoItemDto>(request.Pagination, items, total_count);
        }

        public TodoItemDto GetById(int id)
        {
            var todoItem = _repository.GetById(id);
            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public TodoItemDto UpdateToDoItem(int id, TodoItemDto item)
        {
            var todoItem = _repository.GetById(id);
            if (todoItem != null)
            {
                todoItem.Title = item.Title != todoItem.Title ? item.Title : todoItem.Title;
                todoItem.Description = item.Description != todoItem.Description ? item.Description : todoItem.Description;
                todoItem.Difficulty = item.Difficulty != todoItem.Difficulty ? item.Difficulty : todoItem.Difficulty;
                todoItem.IsDone = item.IsDone != todoItem.IsDone ? item.IsDone : todoItem.IsDone;
                todoItem.Category = item.Category != todoItem.Category ? item.Category : todoItem.Category;
                todoItem = _repository.Update(todoItem);
                _unitOfWork.SaveChanges();
            }
            return _mapper.Map<TodoItemDto>(todoItem);
        }
    }
}
