using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MSClubInsights.API.Responses;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.Comment;
using System.Net;
using System.Security.Claims;

namespace MSClubInsights.API.Controllers.v1_1
{
    [Route("api/v{version:apiVersion}/comments")]
    [ApiController]
    [ApiVersion("1.1")]

    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public APIResponse _response;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;

            _response = new();

        }

        [HttpGet("article/{Article_Id:int}", Name = "GetArticleComments")]
        [EnableRateLimiting("Public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetArticleComments(int Article_Id)
        {
            try
            {
                if (Article_Id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid ID. ID must be greater than zero." };
                    _response.Data = new List<string> { "No Data Retrieved" };
                    return BadRequest(_response);
                }

                var comments = await _commentService.GetAllAsync(u => u.ArticleId == Article_Id);

                if (comments == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "No Comments Found For This Article" };
                    return NotFound(_response);
                }

                _response.Data = comments;
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

        [HttpGet("comment/{Comment_Id:int}")]
        [EnableRateLimiting("Public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetCommentDetails(int Comment_Id)
        {
            try
            {
                if (Comment_Id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid ID. ID must be greater than zero." };
                    _response.Data = new List<string> { "No Data Retrieved" };
                    return BadRequest(_response);
                }

                var comment = await _commentService.GetAsync(u => u.Id == Comment_Id);

                if (comment == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "No Comment Found " };
                    return NotFound(_response);
                }

                _response.Data = comment;
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
        public async Task<ActionResult<APIResponse>> CreateComment([FromBody] CommentCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Can't Accept Empty Comment Data" };
                    return BadRequest(_response);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _commentService.AddAsync(createDTO , userId);

                return CreatedAtAction(nameof(GetCommentDetails), new { id = result.Id }, result);
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

        [HttpPut("{id:int}")]
        [Authorize]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateComment(int id, [FromBody] CommentUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    _response.IsSuccess = false;

                    _response.ErrorMessages = new List<string>()
                    {
                        "Can't Accept Empty Comment Data"
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
                    return BadRequest(_response);

                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _commentService.UpdateAsync(id , userId , updateDTO);

                _response.StatusCode = HttpStatusCode.OK;

                _response.IsSuccess = true;

                _response.Data = result;

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
        [Authorize]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteComment(int id)
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

                Comment comment = await _commentService.GetAsync(u => u.Id == id);

                if (comment == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;

                    _response.IsSuccess = false;

                    _response.ErrorMessages = new List<string>()
                    {
                        "No Comment Found"
                    };

                    return NotFound(_response);
                }

                await _commentService.DeleteAsync(comment);

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