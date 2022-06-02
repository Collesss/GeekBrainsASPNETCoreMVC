using Microsoft.Extensions.DependencyInjection;
using Repository.Validator.Default.Implementations.ProductValidators;
using Repository.Validator.Interfaces.ProductValidators;

namespace Repository.Validator.Default.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryDefaultValidators(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IValidatorProductAdd, ValidatorProductAdd>();
            serviceCollection.AddScoped<IValidatorProductDelete, ValidatorProductDelete>();
            serviceCollection.AddScoped<IValidatorProductUpdate, ValidatorProductUpdate>();

            return serviceCollection;
        }
    }
}
