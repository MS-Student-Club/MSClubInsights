using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MSClubInsights.API.Responses;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.Rating;
using System.Net;
using System.Security.Claims;

namespace MSClubInsights.API.Controllers.v1_1
{
    [Route("api/v{version:apiVersion}/ratings")]
    [ApiController]
    [ApiVersion("1.1")]

    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        public APIResponse _response;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;

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
        public async Task<ActionResult<APIResponse>> GetArticleRatings(int Article_Id)
        {
            try
            {
                var Ratings = await _ratingService.GetAllAsync(u => u.ArticleId == Article_Id);
                if (Ratings == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>()
                    {
                        "No Ratings Found For This Article"
                    };
                    return NotFound(_response);
                }

                _response.Data = Ratings ;
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

        [HttpGet("rating/{Rating_Id:int}")]
        [EnableRateLimiting("Public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRatingDetails(int Rating_Id)
        {
            try
            {
                if (Rating_Id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid ID. ID must be greater than zero." };
                    _response.Data = new List<string> { "No Data Retreived" };
                    return BadRequest(_response);
                }

                var rating = await _ratingService.GetAsync(u => u.Id == Rating_Id);

                if (rating == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "No Rating Found " };
                    return NotFound(_response);
                }

                _response.Data = rating;
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
        [Authorize]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateRating([FromBody] RatingCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Can't Accept Empty Rating Data" };
                    return BadRequest(_response);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


                var result = await _ratingService.AddAsync(createDTO, userId);

                return CreatedAtAction(nameof(GetRatingDetails), new { id = result.Id }, result);
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
        [Authorize]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateRating(int id, [FromBody] RatingUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>();

                    if (updateDTO == null)
                        _response.ErrorMessages.Add("Can't Accept Empty Rating Data");

                    if (id <= 0)
                        _response.ErrorMessages.Add("Invalid ID. ID must be greater than zero.");

                    return BadRequest(_response);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _ratingService.UpdateAsync(id , userId , updateDTO);

                _response.StatusCode = HttpStatusCode.OK;

                _response.IsSuccess = true;

                _response.Data = result;

                return  Ok(_response);


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
        [Authorize]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteRating(int id)
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

                Rating rating = await _ratingService.GetAsync(u => u.Id == id);

                if (rating == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;

                    _response.IsSuccess = false;

                    _response.ErrorMessages = new List<string>()
                    {
                        "No Rating Found"
                    };

                    return NotFound(_response);
                }

                await _ratingService.DeleteAsync(rating);

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