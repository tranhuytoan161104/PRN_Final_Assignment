using Final.ProductAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Final.ProductAPI.Services
{
    public interface IBrandService
    {
        Task<List<BrandDTO>> GetAllBrandsAsync();
    }
}
