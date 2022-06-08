using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Store.Validator.Interfaces
{
    public interface IValidator<TEntity> where TEntity : class
    {
        public Task Validate(ModelStateDictionary modelState, TEntity entity);
    }
}
