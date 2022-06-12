using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using Repository.Models;
using Store.Models;

namespace Store.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;


        public CatalogController(IProductRepository productRepository, 
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> RemoveProduct(RemoveProductViewModel removeProduct)
        {
            await _productRepository.DeleteAsync(_mapper.Map<Product>(removeProduct));

            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult RemoveProduct() =>
            View();


        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel createProduct)
        {
            await _productRepository.AddAsync(_mapper.Map<Product>(createProduct));

            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult AddProduct() =>
            View();

        [HttpGet]
        public async Task<IActionResult> Products() =>
            View((await _productRepository.GetAllAsync()).OrderBy(product => product.Id));
    }
}
