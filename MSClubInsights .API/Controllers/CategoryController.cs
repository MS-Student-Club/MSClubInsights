using Microsoft.AspNetCore.Mvc;
using MSClubInsights.API.Responses;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.Category;
using System.Net;

namespace MSClubInsights.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public APIResponse _response;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;

            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCategories()
        {
            try
            {
                _response.Data = await _categoryService.GetAllAsync();
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add(ex.Message);
            }
            return Ok(_response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetCategory(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid ID. ID must be greater than zero." };
                    _response.Data = new List<string> { "No Data Retreived" };
                    return BadRequest(_response);
                }

                var category = await _categoryService.GetAsync(u => u.Id == id);

                if (category == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "No Category Found " };
                    return NotFound(_response);
                }

                _response.Data = category;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CategoryCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Category is null" };
                    return BadRequest(_response);
                }

                Category category = new()
                {
                    Name = createDTO.Name
                };

                await _categoryService.AddAsync(category);

                _response.Data = category;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return StatusCode(StatusCodes.Status201Created, _response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateCategory(int id, [FromBody] CategoryUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    _response.IsSuccess = false;

                    _response.ErrorMessages = new List<string>()
                    {
                        "Category is null"
                    };

                    return BadRequest(_response);
                }

                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    _response.IsSuccess = false;

                    _response.ErrorMessages = new List<string>()
                    {
                        "Invalid ID. ID must be greater than zero."
                    };
                }

                Category category = await _categoryService.GetAsync(u => u.Id == id);

                category.Name = updateDTO.Name;

                await _categoryService.UpdateAsync(category);

                _response.StatusCode = HttpStatusCode.NoContent;

                _response.IsSuccess = true;

                _response.Data = category;

                return StatusCode(StatusCodes.Status204NoContent, _response);


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteCategory(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    _response.IsSuccess = false;

                    _response.ErrorMessages = new List<string>()
                    {
                        "Invalid ID. ID must be greater than zero."
                    };

                    return BadRequest(_response);
                }

                Category category = await _categoryService.GetAsync(u => u.Id == id);

                if (category == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;

                    _response.IsSuccess = false;

                    return NotFound(_response);
                }

                await _categoryService.DeleteAsync(category);

                _response.StatusCode = HttpStatusCode.NoContent;

                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }

            return _response;

        }

    }
}