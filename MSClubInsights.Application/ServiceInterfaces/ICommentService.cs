using MSClubInsights.Domain.Entities;


namespace MSClubInsights.Application.ServiceInterfaces
{
    public interface ICommentService : IGenericService<Comment>
    {
        Task UpdateAsync(Comment comment);
    }
}
