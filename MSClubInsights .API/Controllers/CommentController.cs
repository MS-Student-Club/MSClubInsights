using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using MSClubInsights.API.Responses;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Infrastructure.DB;
using MSClubInsights.Shared.DTOs.Comment;
using MSClubInsights.Shared.DTOs.Tag;
using System.Net;

namespace MSClubInsights.API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public APIResponse _response;
        private readonly AppDbContext _db;
        public CommentController(ICommentService commentService , AppDbContext db)
        {
            _commentService = commentService;

            _response = new();

            _db = db;
        }

        [HttpGet("{Article_Id:int}", Name = "GetComments")]
        [EnableRateLimiting("Public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetComments(int Article_Id)
        {
            try
            {
                _response.Data = await _commentService.GetAllAsync(u => u.ArticleId == Article_Id);
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

        [HttpGet("comment/{Comment_Id:int}")]
        [EnableRateLimiting("Public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetComment(int Comment_Id)
        {
            try
            {
                if (Comment_Id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid ID. ID must be greater than zero." };
                    _response.Data = new List<string> { "No Data Retreived" };
                    return BadRequest(_response);
                }

                var tag = await _commentService.GetAsync(u => u.Id == Comment_Id);

                if (tag == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "No Comment Found " };
                    return NotFound(_response);
                }

                _response.Data = tag;
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
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateComment([FromBody] CommentCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Comment is null" };
                    return BadRequest(_response);
                }

                var user = await _db.Users.OrderBy(u => u.Id).FirstOrDefaultAsync();

                Comment comment = new()
                {
                    Content = createDTO.Content,
                    ArticleId = createDTO.ArticleId,
                    Date = DateTime.Now,
                    UserId = user.Id
                };

                await _commentService.AddAsync(comment);

                _response.Data = comment;
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
                        "Comment is null"
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
                var user = await _db.Users.OrderBy(u => u.Id).FirstOrDefaultAsync();

                Comment comment = await _commentService.GetAsync(u => u.Id == id);

                comment.Content = updateDTO.Content;
                comment.ArticleId = updateDTO.ArticleId;
                comment.UserId = user.Id;

                await _commentService.UpdateAsync(comment);

                _response.StatusCode = HttpStatusCode.NoContent;

                _response.IsSuccess = true;

                _response.Data = comment;

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

                    return NotFound(_response);
                }

                await _commentService.DeleteAsync(comment);

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