using AutoMapper;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Shared.DTOs.Tag;


namespace MSClubInsights.Application.Services
{
    public class TagService : GenericService<Tag>, ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        public TagService(ITagRepository tagRepository , IMapper mapper) : base(tagRepository)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<Tag> AddAsync(TagCreateDTO createDTO)
        {
            Tag tag = _mapper.Map<Tag>(createDTO);

            var ExistingTag = await _tagRepository.GetAsync(x => x.Name == tag.Name);

            if (ExistingTag != null)
                throw new Exception("A tag with the same name already exists.");

            await _tagRepository.CreateAsync(tag);

            return tag;
        }

        public async Task<Tag> UpdateAsync(int id , TagUpdateDTO updateDTO)
        {
            var ExistingTag = await _tagRepository.GetAsync(x => x.Id == id);

            if (ExistingTag == null)
                throw new ArgumentNullException(nameof(ExistingTag));

            if (ExistingTag.Name == updateDTO.Name)
                throw new Exception("A tag with the same name already exists.");

            _mapper.Map(updateDTO, ExistingTag);

            await _tagRepository.UpdateAsync(ExistingTag);

            return ExistingTag;
        }
    }
}
