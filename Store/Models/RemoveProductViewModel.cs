using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class RemoveProductViewModel
    {
        [Required(ErrorMessage = "Id не может быть пустым.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id должен быть положительным.")]
        public int Id { get; set; }
    }
}
