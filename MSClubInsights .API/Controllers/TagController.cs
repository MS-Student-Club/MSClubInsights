using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MSClubInsights.API.Responses;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.Tag;
using System.Net;

namespace MSClubInsights.API.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        public APIResponse _response;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;

            _response = new();
        }

        [HttpGet]
        [EnableRateLimiting("Public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetTags()
        {
            try
            {
                _response.Data = await _tagService.GetAllAsync();
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

        [HttpPost]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateTag([FromBody] TagCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Tag is null" };
                    return BadRequest(_response);
                }

                Tag tag = new()
                {
                    Name = createDTO.Name
                };

                await _tagService.AddAsync(tag);

                _response.Data = tag;
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
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateTag(int id, [FromBody] TagUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    _response.IsSuccess = false;

                    _response.ErrorMessages = new List<string>()
                    {
                        "Tag is null"
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

                Tag tag = await _tagService.GetAsync(u => u.Id == id);

                tag.Name = updateDTO.Name;

                await _tagService.UpdateAsync(tag);

                _response.StatusCode = HttpStatusCode.NoContent;

                _response.IsSuccess = true;

                _response.Data = tag;

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
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteTag(int id)
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

                Tag tag = await _tagService.GetAsync(u => u.Id == id);

                if (tag == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;

                    _response.IsSuccess = false;

                    return NotFound(_response);
                }

                await _tagService.DeleteAsync(tag);

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