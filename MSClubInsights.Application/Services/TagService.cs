using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;


namespace MSClubInsights.Application.Services
{
    public class TagService : GenericService<Tag>, ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository) : base(tagRepository)
        {
            _tagRepository = tagRepository;
        }
        public async Task UpdateAsync(Tag tag)
        {
            await _tagRepository.UpdateAsync(tag);
        }
    }
}
