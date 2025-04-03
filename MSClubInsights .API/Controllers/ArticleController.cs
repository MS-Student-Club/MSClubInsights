using Microsoft.AspNetCore.Mvc;
using MSClubInsights.API.Responses;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using System.Net;
using System.Security.Claims;
using MSClubInsights.Shared.DTOs.Article;
using MSClubInsights.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.JsonWebTokens;
using MSClubInsights.Shared.Utitlites;
using Azure;

namespace MSClubInsights_.API.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        public APIResponse _response;
        private readonly AppDbContext _db;
        public ArticleController(IArticleService articleService , AppDbContext db)
        {
            _articleService = articleService;

            _response = new();

            _db = db;

        }

        [HttpGet]
        [EnableRateLimiting("Public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetArticles()
        {
            try
            {
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Data = await _articleService.GetAllAsync();
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Data = null;

                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
            
        }

        [HttpGet("{id:int}")]
        [EnableRateLimiting("Public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetArticle(int id)
        {
            try
            {
                if(id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages =  new List<string> { "Invalid ID. ID must be greater than zero." };
                    _response.Data = new List<string> { "No Data Retreived" };
                    return BadRequest(_response);
                }

                var articles = await _articleService.GetAsync(u => u.Id == id);

                if (articles == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "No Article Found " };
                    return NotFound(_response);
                }

                _response.Data = articles;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Data = null;

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
        public async Task<ActionResult<APIResponse>> CreateArticle([FromBody] ArticleCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Article is null" };
                    return BadRequest(_response);
                }

                var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

                Article article = new()
                {
                    ImageUrl = createDTO.ImageUrl,
                    Title = createDTO.Title,
                    Content = createDTO.Content,
                    CategoryId = createDTO.CategoryId,
                    AuthorId = userId,
                    Date = DateTime.Now,
                };

                await _articleService.AddAsync(article);

                return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Data = null;

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
        public async Task<ActionResult<APIResponse>> UpdateArticle(int id , [FromBody] ArticleUpdateDTO updateDTO)
        {
            try
            {
                if(updateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    _response.IsSuccess = false;

                    _response.ErrorMessages = new List<string>()
                    {
                        "Article is null"
                    };

                    return BadRequest(_response);
                }

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

                var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);


                Article article = await _articleService.GetAsync(u => u.Id == id);

                if (article == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>()
                    {
                        "No Article Found"
                    };
                    return NotFound(_response);
                }

                article.Title = updateDTO.Title;
                article.Content = updateDTO.Content;
                article.ImageUrl = updateDTO.ImageUrl;
                article.CategoryId = updateDTO.CategoryId;
                article.AuthorId = userId;
                article.Date = DateTime.Now;

                await _articleService.UpdateAsync(article);

                _response.StatusCode = HttpStatusCode.NoContent;

                _response.IsSuccess = true;

                _response.Data = article;

                return  Ok(_response);


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Data = null;

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

                Article article = await _articleService.GetAsync(u => u.Id == id);

                if(article == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;

                    _response.IsSuccess = false;

                    return NotFound(_response);
                }

                await _articleService.DeleteAsync(article);

                return NoContent();
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Data = null;

                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }


        }

    }
}
