using Microsoft.AspNetCore.Mvc;
using MSClubInsights.API.Responses;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using System.Net;
using MSClubInsights.Shared.DTOs.ArticleTag;
using MSClubInsights.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace MSClubInsights_.API.Controllers
{
    [Route("api/articletags")]
    [ApiController]
    public class ArticleTagController : ControllerBase
    {
        private readonly IArticleTagService _articleTagService;
        public APIResponse _response;
        public ArticleTagController(IArticleTagService articleTagService, AppDbContext db)
        {
            _articleTagService = articleTagService;

            _response = new();

        }

        [HttpGet("{Article_Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetArticleTags(int Article_Id)
        {
            try
            {
                _response.Data = await _articleTagService.GetAllAsync(u => u.ArticleId == Article_Id);
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateArticleTag([FromBody] ArticleTagCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Article Tag is null" };
                    return BadRequest(_response);
                }


                ArticleTag articleTag = new()
                {
                   ArticleId = createDTO.ArticleId,
                    TagId = createDTO.TagId
                };

                await _articleTagService.AddAsync(articleTag);

                _response.Data = articleTag;
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

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateArticleTag(int id , [FromBody] ArticleTagUpdateDTO updateDTO)
        {
            try
            {
                if(updateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    _response.IsSuccess = false;

                    _response.ErrorMessages = new List<string>()
                    {
                        "Article Tag is null"
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
                }

                ArticleTag articleTag = await _articleTagService.GetAsync(u => u.Id == id);

                articleTag.ArticleId = updateDTO.ArticleId;
                articleTag.TagId = updateDTO.TagId;

                await _articleTagService.UpdateAsync(articleTag);
                
                _response.StatusCode = HttpStatusCode.NoContent;

                _response.IsSuccess = true;

                _response.Data = articleTag;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteArticleTag(int id)
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

                ArticleTag articleTag = await _articleTagService.GetAsync(u => u.Id == id);

                if(articleTag == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;

                    _response.IsSuccess = false;

                    return NotFound(_response);
                }

                await _articleTagService.DeleteAsync(articleTag);

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
