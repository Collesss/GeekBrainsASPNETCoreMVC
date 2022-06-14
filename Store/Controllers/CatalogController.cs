using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using Repository.Models;
using Store.MailSender;
using Store.MailSender.MailKit;
using Store.Models;

namespace Store.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IMailSender<MessageData> _mailSender;

        public CatalogController(IProductRepository productRepository, 
            IMapper mapper,
            IMailSender<MessageData> mailSender)
        {
            _mailSender = mailSender;
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
            var product = await _productRepository.AddAsync(_mapper.Map<Product>(createProduct));

            _mailSender.Send(new MessageData { 
                Subject = "Товар добавлен.", 
                Message = $"{{\n\tId: {product.Id};\n\tName: {product.Name};\n\tBase64ImgOrUrl: {product.Base64ImgOrUrl}}}"
            });

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
