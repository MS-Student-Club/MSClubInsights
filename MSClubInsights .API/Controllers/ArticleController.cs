using Microsoft.AspNetCore.Mvc;
using MSClubInsights.API.Responses;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using MSClubInsights.Shared.DTOs.Article;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using MSClubInsights.Shared.Utitlites;

namespace MSClubInsights_.API.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        public APIResponse _response;
        private readonly IMapper _mapper;
        public ArticleController(IArticleService articleService , IMapper mapper)
        {
            _articleService = articleService;

            _response = new();

            _mapper = mapper;

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
                    ex.Message
                };
                _response.StatusCode = HttpStatusCode.InternalServerError;

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
        public async Task<ActionResult<APIResponse>> GetArticleDetails(int id)
        {
            try
            {
                if(id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages =  new List<string> { "Invalid ID. ID must be greater than zero." };
                    _response.Data = new List<string> { "No Data Retrieved" };
                    return BadRequest(_response);
                }

                var article = await _articleService.GetAsync(u => u.Id == id);

                if (article == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "No Article Found " };
                    return NotFound(_response);
                }

                _response.Data = article;
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
        public async Task<ActionResult<APIResponse>> CreateArticle([FromBody] ArticleCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Can't Accept Empty Article Data" };
                    return BadRequest(_response);
                }

                var existingArticle = await _articleService.GetAsync(u => u.Title.ToLower() == createDTO.Title.ToLower());

                if (existingArticle != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "An Article with the same Title already exists." };
                    return BadRequest(_response);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Article article = _mapper.Map<Article>(createDTO);

                article.AuthorId = userId;

                await _articleService.AddAsync(article);

                return CreatedAtAction(nameof(GetArticleDetails), new { id = article.Id }, article);
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
                        "Can't Accept Empty Article Data"
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

                var existingArticle = await _articleService.GetAsync(u => u.Title.ToLower() == updateDTO.Title.ToLower());

                if (existingArticle != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "An Article with the same Title already exists." };
                    return BadRequest(_response);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


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

                _mapper.Map(updateDTO, article);

                article.AuthorId = userId;

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
                    ex.Message
                };
                _response.StatusCode = HttpStatusCode.InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }


        }

    }
}
