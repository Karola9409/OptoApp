using FluentAssertions;
using OptoApi.Models;
using OptoApi.Validators;

namespace OptoApi.Tests;

public class ProductValidatorTests
{
    [Theory]
    [InlineData(23)]
    [InlineData(8)]
    [InlineData(5)]
    [InlineData(0)]
    public void WhenValidProduct_IsValidated_ThenExpectedResultIsReturned(decimal validVat)
    {
        //Arrange
        var product = new Product(
            1, "Frame", "Black, round, size 52x14x135", 10, 299.99m, validVat, "https://example.com");
        var validator = new ProductValidator();
        
        //Act
        var validationResult = validator.IsValid(product);
        
        //Assert
        validationResult.IsValid.Should().BeTrue();
        validationResult.ErrorMessage.Should().BeEmpty();
    }

    [Fact]
    public void WhenProductName_IsEmpty_ThenProductShouldNotBeValid()
    {
        //Arrange
        var product = new Product(
            1, "", "Black, round, size 52x14x135", 10, 299.99m, 23, "https://example.com");
        var validator = new ProductValidator();
        
        //Act
        var validationResult = validator.IsValid(product);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }
    [Theory]
    [InlineData("")]
    [InlineData("Too short")]
    public void WhenProductDescription_IsInvalid_ThenProductShouldNotBeValid(string invalidDescription)
    {
        //Arrange
        var product = new Product(
            1, "Frame", invalidDescription, 10, 299.99m, 23, "https://example.com");
        var validator = new ProductValidator();
        
        //Act
        var validationResult = validator.IsValid(product);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }
    [Fact]
    public void WhenProductStockCount_IsLessThanZero_ThenExpectedResultIsReturned()
    {
        //Arrange
        var product = new Product(
            1, "Frame", "Black, round, size 52x14x135", -10, 299.99m, 23, "https://example.com");
        var validator = new ProductValidator();
        
        //Act
        var validationResult = validator.IsValid(product);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeEmpty();
    }
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void WhenGrossPrice_IsLessThanOrEqualZero_ThenProductShouldNotBeValid(decimal invalidGrossPrice)
    {
        //Arrange
        var product = new Product(
            1, "Frame", "Black, round, size 52x14x135", 10, invalidGrossPrice, 23, "https://example.com");
        var validator = new ProductValidator();
        
        //Act
        var validationResult = validator.IsValid(product);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }
    [Fact]
    public void WhenVatPercentage_IsInvalid_ThenExpectedResultIsReturned()
    {
        //Arrange
        var invalidVat = 9m;
        var product = new Product(
            1, "Frame", "Black, round, size 52x14x135", 10, 299.99m, invalidVat, "https://example.com");
        var validator = new ProductValidator();
        
        //Act
        var validationResult = validator.IsValid(product);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeEmpty();
    }
    [Fact]
    public void WhenPhotoUrl_IsInvalid_ThenExpectedResultIsReturned()
    {
        //Arrange
        var invalidPhotoUrl = "examplecom";
        var product = new Product(
            1, "Frame", "Black, round, size 52x14x135", 10, 299.99m, 23, invalidPhotoUrl);
        var validator = new ProductValidator();
        
        //Act
        var validationResult = validator.IsValid(product);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeEmpty();
    }
}