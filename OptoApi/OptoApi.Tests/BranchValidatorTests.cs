using FluentAssertions;
using OptoApi.Models;
using OptoApi.Validators;

namespace OptoApi.Tests;

public class BranchValidatorTests
{
    [Fact]
    public void WhenNotActiveBranchWithoutEmployees_IsValidated_ThenExpectedResultIsReturned()
    {
        //Arrange
        var branch = new Branch(1, "Wrocław", "Główna", "23", new List<Employee>(), BranchStatus.Pending);
        var validator = new BranchValidator();
        
        //Act
        var validationResult = validator.IsValid(branch);
        
        //Assert
        validationResult.IsValid.Should().BeTrue();
        validationResult.ErrorMessage.Should().BeEmpty();
    }
    
    [Fact]
    public void WhenActiveBranchWithEmployees_IsValidated_ThenExpectedResultIsReturned()
    {
        //Arrange
        var employees = new List<Employee>
        {
            new Employee(1, "Maciej", "Kowlaski", "maciej.kowalski@mail.com", EmployeeRole.Manager),
            new Employee(2, "Karolina", "Nowak", "karolina.nowak@email.com",EmployeeRole.Optician)
        };
        var branch = new Branch(1, "Wrocław", "Główna", "23", employees, BranchStatus.Active);
        var validator = new BranchValidator();
        
        //Act
        var validationResult = validator.IsValid(branch);
        
        //Assert
        validationResult.IsValid.Should().BeTrue();
        validationResult.ErrorMessage.Should().BeEmpty();
    }
    [Fact]
    public void WhenActiveBranchWithoutEmployees_IsValidated_ThenExpectedResultIsReturned()
    {
        //Arrange
        var branch = new Branch(1, "Wrocław", "Główna", "23", new List<Employee>(), BranchStatus.Active);
        var validator = new BranchValidator();
        
        //Act
        var validationResult = validator.IsValid(branch);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeEmpty();
    }
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WhenBranchCityName_IsNullOrEmpty_ThenBranchShouldNotBeValid(string cityName)
    {
        //Arrange
        var branch = new Branch(1, cityName, "Główna", "23", new List<Employee>(), BranchStatus.Pending);
        var validator = new BranchValidator();
        
        //Act
        var validationResult = validator.IsValid(branch);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WhenBranchStreetName_IsNullOrEmpty_ThenBranchShouldNotBeValid(string streetName)
    {
        //Arrange
        var branch = new Branch(1,"Wrocław", streetName, "23", new List<Employee>(), BranchStatus.Pending);
        var validator = new BranchValidator();
        
        //Act
        var validationResult = validator.IsValid(branch);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("number without digit")]
    public void WhenBranchStreetNumber_IsInvalid_ThenBranchShouldNotBeValid(string streetNumber)
    {
        //Arrange
        var branch = new Branch(
            1, "Wrocław", "Główna", streetNumber, new List<Employee>(), BranchStatus.Pending);
        var validator = new BranchValidator();
        
        //Act
        var validationResult = validator.IsValid(branch);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void WhenTypeOfBranchStatus_IsInvalid_ThenBranchShouldNotBeValid()
    {
        //Arrange
        var invalidBranchStatus = (BranchStatus)790;
        var branch = new Branch(
            1, "Wrocław", "Główna","23", new List<Employee>(), invalidBranchStatus);
        var validator = new BranchValidator();
        
        //Act
        var validationResult = validator.IsValid(branch);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}