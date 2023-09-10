using FluentAssertions;
using Moq;
using OptoApi.Exceptions;
using OptoApi.Models;
using OptoApi.Repositories;
using OptoApi.Services;
using OptoApi.Validators;

namespace OptoApi.Tests;

public class ProductsServiceTests
{
    private readonly ProductsService _productsService;
    private readonly ProductValidator _productValidator;
    private readonly Mock<IProductRepository> _productsRepository;

    public ProductsServiceTests()
    {
        _productsRepository = new Mock<IProductRepository>();
        _productValidator = new ProductValidator();
        _productsService = new ProductsService(_productsRepository.Object, _productValidator);
    }
    [Fact]
    public async Task GetAllProducts_ShouldReturn_ProductsFromRepository()
    {
        //Arrange
        var product = new Product(
            1, "Frame", "Black, round, size 52x14x135", 10, 299.99m, 23m, "https://example.com");
        var products = new List<Product> { product };
        _productsRepository
            .Setup(x => x.GetAllProducts())
            .ReturnsAsync(products);
        //Act
        var result =  await _productsService.GetAllProducts();
        //Assert
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task GetProduct_ShouldReturnFailureResult_WhenProductIsNotFound()
    {
        //Arrange
        Product product = null;
        var productId = 1;
        _productsRepository.Setup(x => x.GetProduct(productId)).ReturnsAsync(product);
        //Act
        var result = await _productsService.GetProduct(productId);
        //Assert 
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.NotFound);
        result.Message.Should().NotBeNullOrEmpty();
        result.Data.Should().BeNull();
        _productsRepository.Verify(x => x.GetProduct(productId),Times.Once);
    }

    [Fact]
    public async Task GetProduct_ShouldReturnOk_WhenProductIsCorrect()
    {
        //arrange
        var product = new Product(1, "Frame", "Black, round, size 52x14x135", 10, 299.99m, 23m, "https://example.com");
        _productsRepository.Setup(x => x.GetProduct(product.Id)).ReturnsAsync(product);
        //act
        var result = await _productsService.GetProduct(product.Id);
        //assert
        result.Succeeded.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();
        result.Status.Should().BeNull();
        result.Data.Should().Be(product);
    }
    [Fact]
    public async Task AddProduct_ShouldReturnFailedResult_WhenProductIsAlreadyExists()
    {
        //Arrange
        var product = new Product(1, "Frame", "Black, round, size 52x14x135", 10, 299.99m, 23m, "https://example.com");
        _productsRepository.Setup(x => x.Exists(product.Name)).ReturnsAsync(true);
        
        //Act
        var result = await _productsService.AddProduct(product);
        
        //Assert 
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.AlreadyExists);
    }

    [Fact]
    public async Task AddProduct_ShouldReturnFailedResult_WhenProductIsInvalid()
    {
        //Arrange
        var invalidProduct = new Product(1, "", "Black, round, size 52x14x135", 10, 299.99m, 23m, "https://example.com");
        _productsRepository.Setup(x => x.Exists(invalidProduct.Name)).ReturnsAsync(false);
        //Act
        var result = await _productsService.AddProduct(invalidProduct);
        //Assert 
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.NotValid);
        result.Message.Should().NotBeNullOrEmpty();
        result.Data.Should().Be(default);
        _productsRepository.Verify(x => x.AddProduct(invalidProduct),Times.Never);
    }

    [Fact]
    public async Task AddProduct_ShouldReturnOk_WhenProductIsCorrect()
    {
        //Arrange
        var expectedProductId = 1;
        var product = new Product(0, "Frame", "Black, round, size 52x14x135", 10, 299.99m, 23m, "https://example.com");
        _productsRepository.Setup(x => x.AddProduct(product)).ReturnsAsync(expectedProductId);
        //Act
        var result = await _productsService.AddProduct(product);
        //Assert 
        result.Succeeded.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();
        result.Status.Should().BeNull();
        result.Data.Should().Be(expectedProductId);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnFailureResult_WhenProductIsNotFound()
    {
        //Arrange
        Product product = null;
        var productToUpdate = new Product(1, "", "Black, round, size 52x14x135", 10, 299.99m, 23m, "https://example.com");
        _productsRepository.Setup(x => x.GetProduct(productToUpdate.Id)).ReturnsAsync(product);
        //Act
        var result = await _productsService.UpdateProduct(productToUpdate);
        //Assert 
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.NotFound);
        result.Message.Should().NotBeNullOrEmpty();
        _productsRepository.Verify(x => x.GetProduct(productToUpdate.Id),Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnedFailureResult_WhenProductIsInvalid()
    {
        //Arrange
        var productToUpdate = new Product(1, "", "Black, round, size 52x14x135", 10, 299.99m, 23m, "https://example.com");
        _productsRepository.Setup(x => x.GetProduct(productToUpdate.Id)).ReturnsAsync(productToUpdate);
        //Act
        var result = await _productsService.UpdateProduct(productToUpdate);
        //Assert 
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.NotValid);
        result.Message.Should().NotBeNullOrEmpty();
        _productsRepository.Verify(x => x.GetProduct(productToUpdate.Id),Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnFail_WhenProductNameAlreadyExists()
    {
        //Arrange
        var product = new Product(1, "Frame", "Black, round, size 52x14x135", 10, 299.99m, 23m, "https://example.com");
        _productsRepository.Setup(x => x.GetProduct(product.Id)).ReturnsAsync(product);
        _productsRepository.Setup(x => x.UpdateProduct(product)).Throws(new ProductNameDuplicateException());
        //Act
        var result = await _productsService.UpdateProduct(product);
        //Assert 
        result.Succeeded.Should().BeFalse();
        result.Message.Should().NotBeEmpty();
        result.Status.Should().NotBeNull();
        _productsRepository.Verify(x => x.UpdateProduct(product),Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnOk_WhenProductToUpdateIsCorrect()
    {
        //Arrange
        var product = new Product(1, "Frame", "Black, round, size 52x14x135", 10, 299.99m, 23m, "https://example.com");
        _productsRepository.Setup(x => x.GetProduct(product.Id)).ReturnsAsync(product);
        //Act
        var result = await _productsService.UpdateProduct(product);
        //Assert 
        result.Succeeded.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();
        result.Status.Should().BeNull();
        _productsRepository.Verify(x => x.UpdateProduct(product),Times.Once);
    }

    [Fact]
    public async Task RemoveProduct_ShouldReturnFailedResult_WhenProductToRemoveIsNull()
    {
        //Arrange
        Product product = null;
        var productIdToRemove = 1;
        _productsRepository.Setup(x => x.GetProduct(productIdToRemove)).ReturnsAsync(product);
        //Act
        var result = await _productsService.RemoveProduct(productIdToRemove);
        //Assert 
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.NotFound);
        result.Message.Should().NotBeNullOrEmpty();
        _productsRepository.Verify(x => x.GetProduct(productIdToRemove),Times.Once);
    }

    [Fact]
    public async Task RemoveProduct_ShouldReturnOk_WhenProductToRemoveIsCorrect()
    {
        //Arrange
        var product = new Product(1, "Frame", "Black, round, size 52x14x135", 10, 299.99m, 23m, "https://example.com");
        _productsRepository.Setup(x => x.GetProduct(product.Id)).ReturnsAsync(product);
        //Act
        var result = await _productsService.RemoveProduct(product.Id);
        //Assert
        result.Succeeded.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();
        result.Status.Should().BeNull();
        _productsRepository.Verify(x => x.RemoveProduct(product.Id),Times.Once);
    }
}
