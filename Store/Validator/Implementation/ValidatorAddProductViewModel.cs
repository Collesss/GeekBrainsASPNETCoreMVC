using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repository.Interfaces;
using Store.Models;
using Store.Validator.Interfaces;

namespace Store.Validator.Implementation
{
    public class ValidatorAddProductViewModel : IValidator<AddProductViewModel>
    {
        private readonly IProductRepository _productRepository;

        public ValidatorAddProductViewModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        async Task IValidator<AddProductViewModel>.Validate(ModelStateDictionary modelState, AddProductViewModel entity)
        {
            if (await _productRepository.GetByIdAsync(entity.Id) != null)
                modelState.AddModelError("Id", "Продукт с таким Id уже есть.");

            if (await _productRepository.GetByNameAsync(entity.Name) != null)
                modelState.AddModelError("Name", "Продукт с таким Name уже есть.");
        }
    }
}
