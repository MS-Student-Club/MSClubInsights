using MSClubInsights.Domain.Entities;
using MSClubInsights.Shared.DTOs.Comment;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface ICommentService : IGenericService<Comment>
    {
        Task<Comment> AddAsync(CommentCreateDTO createDTO, string userId);

        Task<Comment> UpdateAsync(int id, string userId, CommentUpdateDTO updateDTO);
    }
}
