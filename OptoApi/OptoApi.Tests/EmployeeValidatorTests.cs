using System.ComponentModel;
using FluentAssertions;
using OptoApi.Models;
using OptoApi.Validators;

namespace OptoApi.Tests;

public class EmployeeValidatorTests
{
    [Theory]
    [InlineData(EmployeeRole.Boss)]
    [InlineData(EmployeeRole.Manager)]
    [InlineData(EmployeeRole.Optician)]
    [InlineData(EmployeeRole.Optometrist)]
    [InlineData(EmployeeRole.Seller)]
    public void WhenValidEmployee_IsValidated_ThenExpectedResultIsRerurned(EmployeeRole employeeRole)
    {
        //Arrange
        var employee = new Employee(
            1, "Adam", "Kowalski", "adamkowalski@email.com", employeeRole, true);
        var validator = new EmployeeValidator();
        
        //Act
        var validationResult = validator.IsValid(employee);
        
        //Assert
        validationResult.IsValid.Should().BeTrue();
        validationResult.ErrorMessage.Should().BeEmpty();
    }
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WhenEmployeeFirstName_IsNullOrEmpty_ThenEmployeeShouldNotBeValid(string firstName)
    {
        //Arrange
        var employee = new Employee(
            1, firstName, "Kowalski", "adamkowalski@email.com", EmployeeRole.Boss, true);
        var validator = new EmployeeValidator();
        
        //Act
        var validationResult = validator.IsValid(employee);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WhenEmployeeLastName_IsNullOrEmpty_ThenEmployeeShouldNotBeValid(string lastName)
    {
        //Arrange
        var employee = new Employee(
            1, "Adam", lastName, "adamkowalski@email.com", EmployeeRole.Boss, true);
        var validator = new EmployeeValidator();
        
        //Act
        var validationResult = validator.IsValid(employee);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WhenUpdateEmployeeLastName_IsNullOrEmpty_ThenEmployeeShouldNotBeValid(string lastName)
    {
        //Arrange
        var employee = new UpdateEmployeeModel(
            1, lastName, "adamkowalski@email.com", EmployeeRole.Boss);
        var validator = new EmployeeValidator();
        
        //Act
        var validationResult = validator.IsValid(employee);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }
    [Theory]
    [InlineData("adamkowalski")]
    [InlineData("adamkowalski.com")]
    [InlineData("adamkowalski@")]
    [InlineData("adamkowalski@emailcom")]
    [InlineData("adamkowalskiemail.com")]
    public void WhenEmployeeEmail_IsNotValid_ThenExpectedResultIsRerurned(string employeeEmail)
    {
        //Arrange
        var employee = new Employee(
            1, "Adam", "Kowalski", employeeEmail, EmployeeRole.Boss, true);
        var validator = new EmployeeValidator();
        
        //Act
        var validationResult = validator.IsValid(employee);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeEmpty();
    }
    [Theory]
    [InlineData("adamkowalski")]
    [InlineData("adamkowalski.com")]
    [InlineData("adamkowalski@")]
    [InlineData("adamkowalski@emailcom")]
    [InlineData("adamkowalskiemail.com")]
    public void WhenUpdateEmployeeEmail_IsNotValid_ThenExpectedResultIsRerurned(string updateEmployeeEmail)
    {
        //Arrange
        var employee = new UpdateEmployeeModel(
            1, "Kowalski", updateEmployeeEmail, EmployeeRole.Boss);
        var validator = new EmployeeValidator();
        
        //Act
        var validationResult = validator.IsValid(employee);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeEmpty();
    }
    [Fact]
    public void WhenEmployeeRole_IsNotValid_ThenExpectedResultIsRerurned()
    {
        //Arrange
        var invalidEmployeeRole = (EmployeeRole)100;
        var employee = new Employee(
            1, "Adam", "Kowalski", "adamkowalski@email.com", invalidEmployeeRole, true);
        var validator = new EmployeeValidator();
        
        //Act
        var validationResult = validator.IsValid(employee);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }
    [Fact]
    public void WhenUpdateEmployeeRole_IsNotValid_ThenExpectedResultIsRerurned()
    {
        //Arrange
        var invalidUpdateEmployeeRole = (EmployeeRole)100;
        var employee = new UpdateEmployeeModel(
            1, "Kowalski", "adamkowalski@email.com", invalidUpdateEmployeeRole);
        var validator = new EmployeeValidator();
        
        //Act
        var validationResult = validator.IsValid(employee);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}