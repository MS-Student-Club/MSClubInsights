using Microsoft.AspNetCore.Mvc;
using MSClubInsights.API.Responses;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using System.Net;
using MSClubInsights.Shared.DTOs.Like;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Asp.Versioning;


namespace MSClubInsights_.API.Controllers
{
    [Route("api/v{version:apiVersion}/likes")]
    [ApiController]
    [ApiVersion("1.0")]

    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;
        public APIResponse _response;
        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;

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
        public async Task<ActionResult<APIResponse>> GetArticleLikeCount(int Article_Id)
        {
            try
            {
                var likes = await _likeService.GetAllAsync(u => u.ArticleId == Article_Id);

                if (likes == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>()
                    {
                        "No Likes Found For This Article"
                    };
                    return NotFound(_response);
                }

                _response.Data = new List<string>()
                {
                    likes.Count().ToString()
                };
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
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
            return Ok(_response);
        }

        [HttpGet("like/{like_id:int}")]
        [EnableRateLimiting("Public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetLikeDetails(int like_id)
        {
            try
            {
                if(like_id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages =  new List<string> { "Invalid ID. ID must be greater than zero." };
                    _response.Data = new List<string> { "No Data Retreived" };
                    return BadRequest(_response);
                }

                var like = await _likeService.GetAsync(u => u.Id == like_id);

                if (like == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "No Like Data Found " };
                    return NotFound(_response);
                }

                _response.Data = like;
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
                _response.Data = null;

                return StatusCode(StatusCodes.Status500InternalServerError, _response);

            }
        }

        [HttpPost]
        [Authorize]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateLike([FromBody] LikeCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Can't Accept Empty Like Data" };
                    return BadRequest(_response);
                }


                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _likeService.AddAsync(createDTO , userId);

                return CreatedAtAction(nameof(GetLikeDetails), new { id = result.Id }, result);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>()
                {
                    ex.Message
                };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Data = null;

                return StatusCode(StatusCodes.Status500InternalServerError, _response);

            }
        }

       
        [HttpDelete("{id:int}")]
        [Authorize]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteArticle(int id)
        {
            try
            {
                if(id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    _response.IsSuccess = false;

                    _response.ErrorMessages = new List<string>()
                    {
                        "Invalid ID. ID must be greater than zero."
                    };

                    return BadRequest(_response);
                }

                Like like = await _likeService.GetAsync(u => u.Id == id);

                if(like == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;

                    _response.IsSuccess = false;

                    _response.ErrorMessages = new List<string>()
                    {
                        "No Like Data Found"
                    };

                    return NotFound(_response);
                }

                await _likeService.DeleteAsync(like);

                return NoContent();
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>()
                {
                    ex.Message
                };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Data = null;

                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

    }
}
