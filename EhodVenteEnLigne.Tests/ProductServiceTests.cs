using EhodBoutiqueEnLigne.Models;
using EhodBoutiqueEnLigne.Models.Repositories;
using EhodBoutiqueEnLigne.Models.Services;
using EhodBoutiqueEnLigne.Models.ViewModels;
using Microsoft.Extensions.Localization;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace EhodBoutiqueEnLigne.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<ICart> _mockCart;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IStringLocalizer<ProductService>> _mockLocalizer;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockCart = new Mock<ICart>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
            _mockLocalizer.Setup(l => l["MissingName"]).Returns(new LocalizedString("MissingName", "MissingName"));

            _productService = new ProductService(_mockCart.Object, _mockProductRepository.Object, _mockOrderRepository.Object, _mockLocalizer.Object);
        }

        [Fact]
        public void SaveProduct_MissingName_ShouldReturnErrorMessage()
        {
            // Arrange
            var productViewModel = new ProductViewModel
            {
                Name = " ",
                Description = "Test Description",
                Price = "10,99",
                Stock = "5"
            };

            // Act
            _productService.SaveProduct(productViewModel);
            var modelErrors = _productService.CheckProductModelErrors(productViewModel);

            // Assert
            Assert.Contains("MissingName", modelErrors);
        }

    }
}
