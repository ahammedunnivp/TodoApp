using Microsoft.AspNetCore.Mvc;
using Unni.ToDo.Common.DTOs;
using Unni.ToDo.Common.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Unni.ToDo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IAdminService _service;
        private readonly ILogger _logger;

        public CategoryController(IAdminService service, ILogger<ToDoController> logger)
        {
            _service = service;
            _logger = logger;

        }
        // GET: api/<CategoryController>
        [HttpGet]
        [ResponseCache(Duration = 60)]
        public IActionResult Get()
        {
            _logger.LogInformation("Entering {Controller}/{Action}", nameof(CategoryController), nameof(Get));
            var items = _service.GetCategories();
            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation("Entering {Controller}/{Action}", nameof(CategoryController), nameof(GetById));
            var item = _service.GetCategoryById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public IActionResult Post([FromBody] AddCategoryRequest request)
        {
            _logger.LogInformation("Entering {Controller}/{Action}", nameof(CategoryController), nameof(GetById));
            if (request == null)
            {
                return BadRequest("Invalid request body");
            }
            var createdCategory = _service.AddCategory(request);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CategoryDto value)
        {
            _logger.LogInformation("Entering {Controller}/{Action}", nameof(CategoryController), nameof(Put));
            if (value == null || id != value.Id)
            {
                return BadRequest("Invalid request body");
            }

            var updatedItem = _service.UpdateCategory(value);
            return CreatedAtAction(nameof(GetById), new { id = updatedItem.Id }, updatedItem);
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogInformation("Entering {Controller}/{Action}", nameof(CategoryController), nameof(Delete));
            _service.DeleteCategory(id);
        }
    }
}
