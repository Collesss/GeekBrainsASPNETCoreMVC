using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using Repository.Models;
using Store.Models;
using Store.Validator.Interfaces;

namespace Store.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<AddProductViewModel> _validatorAddProductViewModel;
        private readonly IMapper _mapper;


        public CatalogController(IProductRepository productRepository, 
            IValidator<AddProductViewModel> validatorAddProductViewModel, 
            IMapper mapper)
        {
            _productRepository = productRepository;
            _validatorAddProductViewModel = validatorAddProductViewModel;
            _mapper = mapper;
        }



        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel createProduct)
        {
            /*
            if (await _productRepository.GetByIdAsync(createProduct.Id) != null)
                ModelState.AddModelError("Id", "Продукт с таким Id уже есть.");

            if (await _productRepository.GetByNameAsync(createProduct.Name) != null)
                ModelState.AddModelError("Name", "Продукт с таким Name уже есть.");
            */

            await _validatorAddProductViewModel.Validate(ModelState, createProduct);

            if (ModelState.IsValid)
            {
                await _productRepository.AddAsync(_mapper.Map<Product>(createProduct));

                return RedirectToAction("Products");
            }

            return View(createProduct);
        }

        [HttpGet]
        public IActionResult AddProduct() =>
            View();

        [HttpGet]
        public async Task<IActionResult> Products() =>
            View((await _productRepository.GetAllAsync()).OrderBy(product => product.Id));
    }
}
