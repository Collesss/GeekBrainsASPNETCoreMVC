using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repository.Interfaces;
using Repository.Models;
using Repository.Validator.Interfaces.ProductValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validator.Interfaces;

namespace Repository.Validator.Default.Implementations.ProductValidators
{
    public class ValidatorProductUpdate : IValidatorProductUpdate
    {
        private readonly IProductRepository _productRepository;

        public ValidatorProductUpdate(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        async Task IValidator<Product>.Validate(ModelStateDictionary modelState, Product entity)
        {
            if (await _productRepository.GetByIdAsync(entity.Id) == null)
                modelState.AddModelError("Id", "Продукт с таким Id не существутет.");

            if (await _productRepository.GetByNameAsync(entity.Name) != null)
                modelState.AddModelError("Name", "Продукт с таким Name уже есть.");
        }
    }
}
