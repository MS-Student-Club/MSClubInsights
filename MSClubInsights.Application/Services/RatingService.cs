using AutoMapper;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Shared.DTOs.Rating;


namespace MSClubInsights.Application.Services
{
    public class RatingService : GenericService<Rating>, IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;
        public RatingService(IRatingRepository ratingRepository , IMapper mapper) : base(ratingRepository)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public  async Task<Rating> AddAsync(RatingCreateDTO createDTO , string userId)
        {
            Rating rating = _mapper.Map<Rating>(createDTO);

            rating.UserId = userId;

            var ExistingRating = await _ratingRepository.GetAsync(x => x.ArticleId == rating.ArticleId && x.UserId == rating.UserId);

            if (ExistingRating != null)
                throw new Exception("A tag with the same name already exists.");

            await _ratingRepository.CreateAsync(rating);

            return rating;
        }

        public async Task<Rating> UpdateAsync(int id , string userId , RatingUpdateDTO updateDTO)
        {
            var ExistingRating = await _ratingRepository.GetAsync(x => x.Id == id);

            if (ExistingRating == null)
                throw new ArgumentNullException(nameof(ExistingRating));

            _mapper.Map(updateDTO, ExistingRating);

            await _ratingRepository.UpdateAsync(ExistingRating);

            return ExistingRating;
        }
    }
}
