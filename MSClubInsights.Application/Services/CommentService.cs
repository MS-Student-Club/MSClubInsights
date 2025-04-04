using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;


namespace MSClubInsights.Application.Services
{
    public class CommentService : GenericService<Comment>, ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository) : base(commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task UpdateAsync(Comment comment)
        {
            await _commentRepository.UpdateAsync(comment);
        }
    }
}
