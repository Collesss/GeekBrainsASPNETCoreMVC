using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class AddProductViewModel
    {
        [Required(ErrorMessage = "Id не может быть пустым.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id должен быть положительным")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name не может быть пустым.")]
        [Display(Name = "Имя продукта.")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Картинка продукта в формате Base64 (не обязательно).")]
        public string Base64Img { get; set; }
    }
}
