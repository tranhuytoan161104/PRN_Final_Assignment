using Final.Domain.Interfaces;
using Final.ProductAPI.DTOs;

namespace Final.ProductAPI.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<List<BrandDTO>> GetAllBrandsAsync()
        {
            var categories = await _brandRepository.GetAllBrandsAsync();

            return categories.Select(c => new BrandDTO
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();
        }
    }
}
