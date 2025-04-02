using Microsoft.AspNetCore.Mvc;
using MSClubInsights.API.Responses;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using System.Net;
using MSClubInsights.Shared.DTOs.Article;
using MSClubInsights.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using MSClubInsights.Shared.DTOs.Like;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;

namespace MSClubInsights_.API.Controllers
{
    [Route("api/likes")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;
        public APIResponse _response;
        private readonly AppDbContext _db;
        public LikeController(ILikeService likeService , AppDbContext db)
        {
            _likeService = likeService;

            _response = new();
            _db = db;
        }

        [HttpGet("{Article_Id:int}")]
        [EnableRateLimiting("Public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetLikes(int Article_Id)
        {
            try
            {
                var likes = await _likeService.GetAllAsync(u => u.ArticleId == Article_Id);
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
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return Ok(_response);
        }

        [HttpGet("like/{like_id:int}")]
        [EnableRateLimiting("Public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetLike(int like_id)
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
                    _response.ErrorMessages = new List<string> { "No Like Found " };
                    return NotFound(_response);
                }

                _response.Data = like;
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
        [Authorize]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateLike([FromBody] LikeCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Like is null" };
                    return BadRequest(_response);
                }

                var user = await _db.Users.OrderBy(u => u.Id).FirstOrDefaultAsync();

                Like like = new()
                {
                    ArticleId = createDTO.ArticleId,
                    UserId = user.Id
                };

                await _likeService.AddAsync(like);

                _response.Data = like;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return StatusCode(StatusCodes.Status201Created , _response);
        }

       
        [HttpDelete("{id:int}")]
        [Authorize]
        [EnableRateLimiting("Modify")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

                    return NotFound(_response);
                }

                await _likeService.DeleteAsync(like);

                _response.StatusCode = HttpStatusCode.NoContent;

                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch(Exception ex)
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
