using Microsoft.AspNetCore.Mvc;
using MSClubInsights.API.Responses;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using System.Net;
using MSClubInsights.Shared.DTOs.ArticleTag;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using MSClubInsights.Shared.Utitlites;
using Asp.Versioning;

namespace MSClubInsights_.API.Controllers
{
    [Route("api/v{version:apiVersion}/articletags")]
    [ApiController]
    [ApiVersion("1.0")]

    public class ArticleTagController : ControllerBase
    {
        private readonly IArticleTagService _articleTagService;
        public APIResponse _response;
        public ArticleTagController(IArticleTagService articleTagService)
        {
            _articleTagService = articleTagService;

            _response = new();

        }

        [HttpGet("{Article_Id:int}")]
        [EnableRateLimiting("Public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetArticleTags(int Article_Id)
        {
            try
            {
                if (Article_Id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>()
                    {
                        "Invalid ID. ID must be greater than zero."
                    };
                    return BadRequest(_response);
                }

                var articleTags = await _articleTagService.GetAllAsync(u => u.ArticleId == Article_Id);

                if (articleTags == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>()
                    {
                        "No Tags found for the given Article ID."
                    };
                    return NotFound(_response);
                }

                _response.Data = articleTags;
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
        public async Task<ActionResult<APIResponse>> CreateArticleTag([FromBody] ArticleTagCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Can't Accept Empty Article - Tag Data" };
                    return BadRequest(_response);
                }

                var result = await _articleTagService.AddAsync(createDTO);

                _response.Data = result;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                _response.ErrorMessages = null;

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

        [HttpDelete("{id:int}")]
        [Authorize(Roles = SD.TechMember + "," + SD.SysAdmin + "," + SD.CoreTeam)]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteArticleTag(int id)
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

                ArticleTag articleTag = await _articleTagService.GetAsync(u => u.Id == id);

                if (articleTag == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;

                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>()
                    {
                        "Tag Not Found"
                    };

                    return NotFound(_response);
                }

                await _articleTagService.DeleteAsync(articleTag);

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
