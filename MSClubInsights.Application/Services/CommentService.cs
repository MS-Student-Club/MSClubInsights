using AutoMapper;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Shared.DTOs.Comment;


namespace MSClubInsights.Application.Services
{
    public class CommentService : GenericService<Comment>, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        public CommentService(ICommentRepository commentRepository , IMapper mapper) : base(commentRepository)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }
        public async Task<Comment> AddAsync(CommentCreateDTO createDTO, string userId)
        {
            Comment comment = _mapper.Map<Comment>(createDTO);

            comment.UserId = userId;

            await _commentRepository.CreateAsync(comment);

            return comment;
        }

        public async Task<Comment> UpdateAsync(int id, string userId, CommentUpdateDTO updateDTO)
        {
            var ExistingComment = await _commentRepository.GetAsync(x => x.Id == id);

            if (ExistingComment == null)
                throw new ArgumentNullException(nameof(ExistingComment));


            Comment comment = _mapper.Map(updateDTO, ExistingComment);

            comment.UserId = userId;

            await _commentRepository.UpdateAsync(comment);

            return comment;
        }
    }
}
