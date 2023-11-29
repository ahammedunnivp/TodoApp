using Microsoft.AspNetCore.Mvc;
using Unni.ToDo.API.DTOs;
using Unni.ToDo.API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Unni.ToDo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ITodoService _service;
        private readonly ILogger _logger;

        public ToDoController(ITodoService service, ILogger<ToDoController> logger)
        {
            _service = service;
            _logger = logger;

        }


        // GET api/<ToDoController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation("Entering {Controller}/{Action}", nameof(ToDoController), nameof(GetById));
            var item = _service.GetById(id);
            if(item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }



        // GET api/<ToDoController>/search
        [HttpPost("search")]
        public IActionResult Search([FromBody] GetTodoRequest request)
        {
            _logger.LogInformation("Entering {Controller}/{Action}", nameof(ToDoController), nameof(Search));
            var items = _service.Search(request);
            if(items?.Items?.Count() != 0)
                return Ok(items);
            else
                return NotFound( new { Message = "Search is empty"});
        }

        // POST api/<ToDoController>
        [HttpPost]
        public IActionResult AddToDo([FromBody] CreateTodoRequest request)
        {
            _logger.LogInformation("Entering {Controller}/{Action}", nameof(ToDoController), nameof(AddToDo));
            if (request == null)
            {
                return BadRequest("Invalid request body");
            }
            var createdTodo = _service.AddToDoItem(request);
            return CreatedAtAction(nameof(GetById), new { id = createdTodo.Id }, createdTodo);
        }

        // PUT api/<ToDoController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateToDo(int id, [FromBody] TodoItemDto item)
        {
            _logger.LogInformation("Entering {Controller}/{Action}", nameof(ToDoController), nameof(UpdateToDo));
            if (item == null || id != item.Id)
            {
                return BadRequest("Invalid request body");
            }

            var existingTodoItem = _service.GetById(id);
            if(existingTodoItem == null)
            {
                return NotFound();
            }

            var updatedItem = _service.UpdateToDoItem(id, item);
            return CreatedAtAction(nameof(GetById), new { id = updatedItem.Id }, updatedItem);

        }

        // DELETE api/<ToDoController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Entering {Controller}/{Action}", nameof(ToDoController), nameof(Delete));
            var existingTodoItem = _service.GetById(id);
            if (existingTodoItem == null)
            {
                return NotFound();
            }
            _service.DeleteToDoItemById(id);
            return NoContent();
        }
    }
}
