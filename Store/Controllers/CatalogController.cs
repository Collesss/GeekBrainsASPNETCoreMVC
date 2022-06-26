﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Polly;
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
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, 
            IMapper mapper,
            IMailSender<MessageData> mailSender,
            ILogger<CatalogController> logger)
        {
            _mailSender = mailSender;
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> RemoveProduct(RemoveProductViewModel removeProduct)
        {
            var product = await _productRepository.DeleteAsync(_mapper.Map<Product>(removeProduct));

            _logger.LogInformation("Product removed: {@Product}", product);

            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult RemoveProduct() =>
            View();


        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel createProduct)
        {
            var product = await _productRepository.AddAsync(_mapper.Map<Product>(createProduct));

            _logger.LogInformation("Product added: {@Product}", product);

            var policy = Policy.Handle<Exception>()
                .RetryAsync(3, (e, retryCount) => _logger.LogWarning("Error while sending email. Retrying: {Attempt}", retryCount));

            var policyRes = await policy.ExecuteAndCaptureAsync(() => _mailSender.Send(new MessageData
            {
                Subject = "Товар добавлен.",
                Message = $"{{\n\tId: {product.Id};\n\tName: {product.Name};\n\tBase64ImgOrUrl: {product.Base64ImgOrUrl}}}"
            }));

            if (policyRes.Outcome == OutcomeType.Failure)
                _logger.LogError(policyRes.FinalException, "There was an error while sending email.");
            else
                _logger.LogInformation("Email sent: {@Product}", product);

            /*
            _mailSender.Send(new MessageData { 
                Subject = "Товар добавлен.", 
                Message = $"{{\n\tId: {product.Id};\n\tName: {product.Name};\n\tBase64ImgOrUrl: {product.Base64ImgOrUrl}}}"
            });
            */

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
