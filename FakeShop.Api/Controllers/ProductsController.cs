using AutoMapper;
using FakeShop.Api.ApiModels;
using FakeShop.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FakeShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository ?? 
                throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetProducts(string category = "all")
        {
            //Log.Information("Starting controller action GetProducts for {category}", category);
            Log.ForContext("Category", category)
               .Information("Starting controller action GetProducts");

            var products = await _productRepository.GetProductsForCategory(category);

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
