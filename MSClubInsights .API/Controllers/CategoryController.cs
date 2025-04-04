using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MSClubInsights.API.Responses;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.Category;
using MSClubInsights.Shared.Utitlites;
using System.Net;
using AutoMapper;

namespace MSClubInsights.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public APIResponse _response;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService , IMapper mapper)
        {
            _categoryService = categoryService;

            _response = new();

            _mapper = mapper;
        }

        [HttpGet]
        [EnableRateLimiting("Public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetCategories()
        {
            try
            {
                _response.Data = await _categoryService.GetAllAsync();
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>()
                {
                    ex.Message
                };
                _response.StatusCode = HttpStatusCode.InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, _response);

            }
        }


        [HttpPost]
        [Authorize(Roles = SD.TechMember + "," + SD.SysAdmin + "," + SD.CoreTeam)]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CategoryCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Can't Accept Empty Category Data" };
                    return BadRequest(_response);
                }

                var existingCategory = await _categoryService.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower());

                if (existingCategory != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "A Category with the same Name already exists." };
                    return BadRequest(_response);
                }

                Category category = _mapper.Map<Category>(createDTO);

                await _categoryService.AddAsync(category);

                _response.Data = category;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                return StatusCode(StatusCodes.Status201Created, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>()
                {
                    ex.Message
                };
                _response.StatusCode = HttpStatusCode.InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, _response);

            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = SD.TechMember + "," + SD.SysAdmin + "," + SD.CoreTeam)]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                        "Can't Accept Empty Category Data"
                    };

                    return BadRequest(_response);
                }

                var existingCategory = await _categoryService.GetAsync(u => u.Name.ToLower() == updateDTO.Name.ToLower());

                if (existingCategory != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "A Category with the same Name already exists." };
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

                    return BadRequest(_response);
                }

                Category category = await _categoryService.GetAsync(u => u.Id == id);

                if (category == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>()
                    {
                        "Category not found"
                    };
                    return NotFound(_response);
                }

                _mapper.Map(updateDTO, category);

                await _categoryService.UpdateAsync(category);

                _response.StatusCode = HttpStatusCode.NoContent;

                _response.IsSuccess = true;

                _response.Data = category;

                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>()
                {
                    ex.Message
                };
                _response.StatusCode = HttpStatusCode.InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = SD.TechMember + "," + SD.SysAdmin + "," + SD.CoreTeam)]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

                return NoContent();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>()
                {
                    ex.Message
                };
                _response.StatusCode = HttpStatusCode.InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, _response);

            }

        }

    }
}