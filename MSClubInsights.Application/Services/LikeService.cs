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
    public class LikeService : GenericService<Like>, ILikeService
    {
        private readonly ILikeRepository _likeRepository;
        public LikeService(ILikeRepository likeRepository) : base(likeRepository)
        {
            _likeRepository = likeRepository;
        }
    }
}
