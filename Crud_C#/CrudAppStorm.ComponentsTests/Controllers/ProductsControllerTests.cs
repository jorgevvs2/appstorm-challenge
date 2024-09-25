using CrudAppStorm.app.Domain.Entities;
using CrudAppStorm.app.Repositories.Interface;
using CrudAppStorm.src.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CrudAppStorm.ComponentsTests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _controller = new ProductsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetProductById_ReturnsProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product" };

            _mockRepo.Setup(repo => repo.Exists(productId)).ReturnsAsync(true);
            _mockRepo.Setup(repo => repo.GetById(productId)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            Assert.NotNull(result.Value);
            var returnValue = Assert.IsType<Product>(result.Value);
            Assert.Equal(productId, returnValue.Id);
        }

        [Fact]
        public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;

            _mockRepo.Setup(repo => repo.Exists(productId)).ReturnsAsync(false);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };

            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            var returnValue = Assert.IsType<List<Product>>(result);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task PostProduct_ReturnsCreatedProduct()
        {
            // Arrange
            var newProduct = new Product { Id = 1, Name = "New Product" };

            _mockRepo.Setup(repo => repo.Insert(newProduct)).Returns(Task.CompletedTask);
            _mockRepo.Setup(repo => repo.Save(0)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PostProduct(newProduct);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Product>(actionResult.Value);
            Assert.Equal(newProduct.Id, returnValue.Id);
            Assert.Equal("New Product", returnValue.Name);
        }

        [Fact]
        public async Task PutProduct_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var productId = 1;
            var updatedProduct = new Product { Id = productId, Name = "Updated Product" };

            _mockRepo.Setup(repo => repo.Exists(productId)).ReturnsAsync(true);
            _mockRepo.Setup(repo => repo.Update(updatedProduct)).Returns(Task.CompletedTask);
            _mockRepo.Setup(repo => repo.Save(0)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutProduct(productId, updatedProduct);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutProduct_ReturnsBadRequest_WhenProductIdsDoNotMatch()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = 2, Name = "Mismatched Product" };

            // Act
            var result = await _controller.PutProduct(productId, product);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsDeletedProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Product to Delete" };

            _mockRepo.Setup(repo => repo.GetById(productId)).ReturnsAsync(product);
            _mockRepo.Setup(repo => repo.Delete(productId)).Returns(Task.CompletedTask);
            _mockRepo.Setup(repo => repo.Save(0)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            var returnValue = Assert.IsType<Product>(result.Value);
            Assert.Equal(productId, returnValue.Id);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;

            _mockRepo.Setup(repo => repo.GetById(productId)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }

}
