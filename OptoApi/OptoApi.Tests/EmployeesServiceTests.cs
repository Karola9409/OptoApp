using FluentAssertions;
using Moq;
using OptoApi.Models;
using OptoApi.Repositories;
using OptoApi.Validators;
using OptoApi.Services;

namespace OptoApi.Tests;

public class EmployeesServiceTests
{
    private readonly EmployeesService _employeesService;
    private readonly EmployeeValidator _employeeValidator;
    private readonly Mock<IEmployeeRepository> _employeeRepository;

    public EmployeesServiceTests()
    {
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeValidator = new EmployeeValidator();
        _employeesService = new EmployeesService(_employeeRepository.Object, _employeeValidator);
    }

    [Fact]
    public async Task GetAllEmployees_ShouldReturn_EmployeesFromRepository()
    {
        //Arrange
        var employee = new Employee(1, "Karol", "Kowalski", "karol.kowalski@email.com", EmployeeRole.Boss);
        var employees = new List<Employee> { employee };
        _employeeRepository.Setup(x => x.GetAllEmployees()).ReturnsAsync(employees);
        //Act
        var result =await _employeesService.GetAllEmployees();
        //Assert
        result.Should().BeEquivalentTo(employees);
    }

    [Fact]
    public async Task GetEmployee_ShouldReturnOk_WhenEmployeeIsCorrect()
    {
        //Arrange 
        var employee = new Employee(1, "Karol", "Kowalski", "karol.kowalski@email.com", EmployeeRole.Boss);
        _employeeRepository.Setup(x => x.GetEmployee(employee.EmployeeId)).ReturnsAsync(employee);
        //Act
        var result =await _employeesService.GetEmployee(employee.EmployeeId);
        //Assert
        result.Succeeded.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();
        result.Status.Should().BeNull();
        result.Data.Should().Be(employee);
    }

    [Fact]
    public async Task GetEmployee_ShouldReturnFailureResult_WhenEmployeeIsNotFound()
    {
        //Arrange
        Employee employee = null;
        var employeeId = 1;
        _employeeRepository.Setup(x => x.GetEmployee(employeeId)).ReturnsAsync(employee);
        //Act
        var result =await _employeesService.GetEmployee(employeeId);
        //Assert
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.NotFound);
        result.Message.Should().NotBeNullOrEmpty();
        result.Data.Should().BeNull();
        _employeeRepository.Verify(x => x.GetEmployee(employeeId),Times.Once);
    }
    [Fact]
    public async Task AddEmployee_ShouldReturnFailedResult_WhenEmployeeIsInvalid()
    {
        //Arrange 
        var invalidEmployee = new Employee(0,"Karol", "Kowalski", "karol.kowalski@", EmployeeRole.Boss);
        //Act
        var result =await _employeesService.AddEmployee(invalidEmployee);
        //Assert
        result.Succeeded.Should().BeFalse();
        result.Message.Should().NotBeNullOrEmpty();
        result.Status.Should().Be(ErrorStatus.NotValid);
    }

    [Fact]
    public async Task AddEmployee_ShouldReturnOk_WhenEmployeeIsCorrect()
    {
        //Arrange 
        var employee = new Employee(1, "Karol", "Kowalski", "karol.kowalski@email.com", EmployeeRole.Boss);
        _employeeRepository.Setup(x => x.AddEmployee(employee)).ReturnsAsync(1);
        //Act
        var result =await _employeesService.AddEmployee(employee);
        //Assert
        result.Succeeded.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();
        result.Status.Should().BeNull();
        result.Data.Should().Be(1);
    }

    [Fact]
    public async Task UpdateEmployee_ShouldReturnFailureResult_WhenEmployeeIsNotFound()
    {
        //Arrange
        Employee employee = null;
        var employeeToUpdate = new UpdateEmployeeModel(1, "Kowalski", "karol.kowalski@email.com", EmployeeRole.Boss);
        _employeeRepository.Setup(x => x.GetEmployee(employeeToUpdate.ID)).ReturnsAsync(employee);
        //Act
        var result =await _employeesService.UpdateEmployee(employeeToUpdate);
        //Assert
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.NotFound);
        result.Message.Should().NotBeNullOrEmpty();
        _employeeRepository.Verify(x => x.GetEmployee(employeeToUpdate.ID),Times.Once);
    }

    [Fact]
    public async Task UpdateEmployee_ShouldReturnFailureResult_WhenEmployeeIsInvalid()
    {
        //Arrange
        var employeeToUpdate = new UpdateEmployeeModel(1, "Kowalski", "karol.kowalski@", EmployeeRole.Boss);
        var employee = new Employee(1, "Karol", "Kowalski", "karol.kowalski@", EmployeeRole.Boss);
        _employeeRepository.Setup(x => x.GetEmployee(employeeToUpdate.ID)).ReturnsAsync(employee);
        //Act
        var result = await _employeesService.UpdateEmployee(employeeToUpdate);
        //Assert
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.NotValid);
        result.Message.Should().NotBeNullOrEmpty();
        _employeeRepository.Verify(x => x.GetEmployee(employeeToUpdate.ID),Times.Once);
    }

    [Fact]
    public async Task UpdateEmployee_ShouldReturnOk_WhenEmployeeToUpdateIsCorrect()
    {
        //Arrange
        var employeeToUpdate = new UpdateEmployeeModel(1, "Kowalski", "karol.kowalski@email.com", EmployeeRole.Boss);
        var employee = new Employee(1, "Karol", "Kowalski", "karol.kowalski@email.com", EmployeeRole.Boss);
        _employeeRepository.Setup(x => x.GetEmployee(employeeToUpdate.ID)).ReturnsAsync(employee);
        //Act
        var result =await _employeesService.UpdateEmployee(employeeToUpdate);
        //Assert
        result.Succeeded.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();
        result.Status.Should().BeNull();
        _employeeRepository.Verify(x => x.GetEmployee(employeeToUpdate.ID),Times.Once);
        _employeeRepository.Verify(x=>x.UpdateEmployee(employeeToUpdate),Times.Once);
    }

    [Fact]
    public async Task RemoveProduct_ShouldReturnFailureResult_WhenEmployeeIsNull()
    {
        //Arrange
        Employee employee = null;
        var employeeIdToRemove = 1;
        _employeeRepository.Setup(x => x.GetEmployee(employeeIdToRemove)).ReturnsAsync(employee);
        //Act
        var result =await _employeesService.RemoveEmployee(employeeIdToRemove);
        //Assert
        result.Succeeded.Should().BeFalse();
        result.Message.Should().NotBeNullOrEmpty();
        result.Status.Should().Be(ErrorStatus.NotFound);
        _employeeRepository.Verify(x => x.GetEmployee(employeeIdToRemove),Times.Once);
    }

    [Fact]
    public async Task RemoveProduct_ShouldReturnOk_WhenEmployeeIsCorrect()
    {
        var employee = new Employee(1, "Karol", "Kowalski", "karol.kowalski@email.com", EmployeeRole.Boss);
        _employeeRepository.Setup(x => x.GetEmployee(employee.EmployeeId)).ReturnsAsync(employee);
        //Act
        var result =await _employeesService.RemoveEmployee(employee.EmployeeId);
        //Assert
        result.Succeeded.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();
        result.Status.Should().BeNull();
        _employeeRepository.Verify(x => x.GetEmployee(employee.EmployeeId),Times.Once);
        _employeeRepository.Verify(x=>x.RemoveEmployee(employee.EmployeeId),Times.Once);
    }

}