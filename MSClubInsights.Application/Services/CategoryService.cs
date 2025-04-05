using AutoMapper;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Domain.Entities;
using MSClubInsights.Domain.RepoInterfaces;
using MSClubInsights.Shared.DTOs.Category;


namespace MSClubInsights.Application.Services
{
    public class CategoryService : GenericService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository , IMapper mapper) : base(categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Category> AddAsync(CategoryCreateDTO createDTO)
        {
            var existingCategory =
                    await _categoryRepository.GetAsync(u => u.Name == createDTO.Name);

            if (existingCategory != null)
                throw new Exception("A Category with the same name already exists.");

            Category category = _mapper.Map<Category>(createDTO);

            await _categoryRepository.CreateAsync(category);

            return category;
        }

        public async Task<Category> UpdateAsync(int id, CategoryUpdateDTO updateDTO)
        {
            var ExistingCategory = await _categoryRepository.GetAsync(x => x.Id == id);

            if (ExistingCategory == null)
                throw new ArgumentNullException(nameof(ExistingCategory));

            if (ExistingCategory.Name == updateDTO.Name)
                throw new Exception("A Category with the same name already exists.");

            _mapper.Map(updateDTO, ExistingCategory);

            await _categoryRepository.UpdateAsync(ExistingCategory);

            return ExistingCategory;
        }
    }
}
