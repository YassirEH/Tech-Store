using AutoMapper;
using Core.Dto;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using webApi.Application.Services;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : APIController
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRep _categoryRep;

        public CategoryController(ICategoryRep categoryRep, IMapper mapper, INotificationService notificationService)
            : base(notificationService)
        {
            _mapper = mapper;
            _categoryRep = categoryRep;
        }

        [HttpGet("Get All Categories")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _categoryRep.GetCategories();
            var categoryDto = _mapper.Map<List<CategoryDto>>(categories);

            return !ModelState.IsValid ? BadRequest(ModelState) : Response(categoryDto);
        }

        [HttpGet("Get Category By Id/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!ModelState.IsValid)  return BadRequest(ModelState);

            //if (!_categoryRep.CategoryExists(categoryId))  return NotFound();

            var category = _mapper.Map<CategoryDto>(_categoryRep.GetCategory(categoryId));

            return Response(category);
        }

        [HttpGet("Filter By Name")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult FilterByName()
        {
            var categories = _categoryRep.FilterByName();
            var categoryDto = _mapper.Map<List<CategoryDto>>(categories);

            return !ModelState.IsValid ? BadRequest(ModelState) : Response(categoryDto);
        }

        [HttpPost("Create Category")]
        [ProducesResponseType(204, Type = typeof(CategoryDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _mapper.Map<Category>(categoryDto);
            _categoryRep.CreateCategory(category);
            var createdCategoryDto = _mapper.Map<CategoryDto>(category);

            _notificationService.Notify("A new category has been created", "Success", ErrorType.Success);

            return CreatedAtAction(nameof(GetCategory), new { categoryId = category.Id }, createdCategoryDto);
        }

        [HttpPut("Update Category/{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest(ModelState);

            var existingCategory = _categoryRep.GetCategory(categoryId);
            if (existingCategory == null)
                return NotFound();

            existingCategory.Name = updatedCategory.Name;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return NoContent();
        }

        [HttpDelete("Delete Category/{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRep.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var categoryToDelete = _categoryRep.GetCategory(categoryId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool deleteResult = _categoryRep.DeleteCategory(categoryToDelete);

            if (deleteResult)
            {
                _notificationService.Notify("The category has been deleted", "Success", ErrorType.Success);
                return NoContent();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong deleting this category");
                _notificationService.Notify("Error deleting the category", "Error", ErrorType.Error);
                return BadRequest(ModelState);
            }
        }
    }
}
