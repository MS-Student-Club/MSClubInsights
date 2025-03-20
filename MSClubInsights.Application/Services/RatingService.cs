using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSClubInsights.Application.Services
{
    public class RatingService : GenericService<Rating>, IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository) : base(ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }
        public async Task UpdateAsync(Rating rating)
        {
            await _ratingRepository.UpdateAsync(rating);
        }
    }
}
