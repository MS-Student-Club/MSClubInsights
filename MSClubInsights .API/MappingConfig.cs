using AutoMapper;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.Article;
using MSClubInsights.Shared.DTOs.ArticleTag;
using MSClubInsights.Shared.DTOs.Category;
using MSClubInsights.Shared.DTOs.City;
using MSClubInsights.Shared.DTOs.Comment;
using MSClubInsights.Shared.DTOs.Like;
using MSClubInsights.Shared.DTOs.Rating;
using MSClubInsights.Shared.DTOs.Tag;

namespace MSClubInsights.API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Article, ArticleCreateDTO>().ReverseMap();
            CreateMap<Article, ArticleUpdateDTO>().ReverseMap();

            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<Category, CategoryUpdateDTO>().ReverseMap();

            CreateMap<Comment, CommentCreateDTO>().ReverseMap();
            CreateMap<Comment, CommentUpdateDTO>().ReverseMap();

            CreateMap<Like, LikeCreateDTO>().ReverseMap();

            CreateMap<ArticleTag, ArticleTagCreateDTO>().ReverseMap();

            CreateMap<Tag, TagCreateDTO>().ReverseMap();
            CreateMap<Tag, TagUpdateDTO>().ReverseMap();

            CreateMap<City, CityCreateDTO>().ReverseMap();
            CreateMap<City, CityUpdateDTO>().ReverseMap();

            CreateMap<Rating, RatingCreateDTO>().ReverseMap();
            CreateMap<Rating, RatingUpdateDTO>().ReverseMap();

        }
    }
}
