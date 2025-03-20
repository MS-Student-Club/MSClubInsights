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
