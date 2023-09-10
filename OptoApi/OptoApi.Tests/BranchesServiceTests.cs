using FluentAssertions;
using Moq;
using OptoApi.Models;
using OptoApi.Repositories;
using OptoApi.Services;
using OptoApi.Validators;

namespace OptoApi.Tests;

public class BranchesServiceTests
{
    private readonly BranchesService _branchesService;
    private readonly BranchValidator _branchValidator;
    private readonly Mock<IEmployeeRepository> _employeeRepository;
    private readonly Mock<IBranchRepository> _branchRepository;

    public BranchesServiceTests()
    {
        _branchRepository = new Mock<IBranchRepository>();
        _branchValidator = new BranchValidator();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _branchesService = new BranchesService(_branchRepository.Object, _branchValidator, _employeeRepository.Object);
    }

    [Fact]
    public async Task GetAllBranches_ShouldReturn_BranchesFromRepository()
    {
        //arrange
        var branch = new Branch(1, "Wrocław", "Główna", "22", new List<Employee>(), BranchStatus.Active);
        var branches = new List<Branch> { branch };
        _branchRepository.Setup(x => x.GetAllBranches()).ReturnsAsync(branches);
        //act
        var result = await _branchesService.GetAllBranches();
        //assert
        result.Should().BeEquivalentTo(branches);
    }

    [Fact]
    public async Task GetBranch_ShouldReturnFailureResult_WhenBranchIsNull()
    {
        //arrange
        Branch branch = null;
        var branchId = 1;
        _branchRepository.Setup(x => x.GetBranch(branchId)).ReturnsAsync(branch);
        //act
        var result = await _branchesService.GetBranch(branchId);
        //assert
        result.Succeeded.Should().BeFalse();
        result.Message.Should().NotBeNullOrEmpty();
        result.Status.Should().Be(ErrorStatus.NotFound);
        result.Data.Should().BeNull();
        _branchRepository.Verify(x =>x.GetBranch(branchId),Times.Once);
    }

    [Fact]
    public async Task GetBranch_ShouldReturnOk_WhenBranchIsCorrect()
    {
        //arrange
        var branch = new Branch(1, "Wrocław", "Główna", "22", new List<Employee>(), BranchStatus.Active);
        _branchRepository.Setup(x => x.GetBranch(branch.BranchId)).ReturnsAsync(branch);
        //act
        var result =await _branchesService.GetBranch(branch.BranchId);
        //assert
        result.Succeeded.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();
        result.Status.Should().BeNull();
        result.Data.Should().Be(branch);
    }

    [Fact]
    public async Task AddBranch_ShouldReturnFailureResult_WhenBranchIsInvalid()
    {
        //arrange 
        var invalidBranch = new Branch(0, "Wrocław", "Główna", "22", new List<Employee>(), BranchStatus.Active);
        //act
        var result =await _branchesService.AddBranch(invalidBranch);
        //assert
        result.Succeeded.Should().BeFalse();
        result.Message.Should().NotBeNullOrEmpty();
        result.Status.Should().Be(ErrorStatus.NotValid);
    }

    [Fact]
    public async Task AddBranch_ShouldReturnOk_WhenBranchIsCorrect()
    {
        //arrange
        var branch = new Branch(1, "Wrocław", "Główna", "22", new List<Employee>(), BranchStatus.Pending);
        _branchRepository.Setup(x => x.AddBranch(branch)).ReturnsAsync(1);
        //act
        var result =await _branchesService.AddBranch(branch);
        //assert
        result.Succeeded.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();
        result.Status.Should().BeNull();
        result.Data.Should().Be(1);
    }

    [Fact]
    public async Task AddEmployee_ShouldReturnFailureResult_WhenEmployeeIsNotFound()
    {
        //arrange
        Employee employee = null;
        var employeeId = 1;
        var branchId = 2;
        _employeeRepository.Setup(x => x.GetEmployee(employeeId)).ReturnsAsync(employee);
        //Act
        var result =await _branchesService.AddEmployee(employeeId, branchId);
        //Assert
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.NotFound);
        result.Message.Should().NotBeNullOrEmpty();
        _employeeRepository.Verify(x => x.GetEmployee(employeeId),Times.Once);
    }

    [Fact]
    public async Task AddEmployee_ShouldReturnFailureResult_WhenBranchIsNull()
    {
        //arrange
        Branch branch = null;
        var employee = new Employee(1, "Karol", "Kowalski", "karol.kowalski@email.com", EmployeeRole.Boss);
        var employeeId = 1;
        var branchId = 2;
        _branchRepository.Setup(x => x.GetBranch(branchId)).ReturnsAsync(branch);
        _employeeRepository.Setup(x => x.GetEmployee(employeeId)).ReturnsAsync(employee);
        //act
        var result =await _branchesService.AddEmployee(employeeId, branchId);
        //assert
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.NotFound);
        result.Message.Should().NotBeNullOrEmpty();
        _branchRepository.Verify(x => x.GetBranch(branchId),Times.Once);
    }

    [Fact]
    public async Task AddEmployee_ShouldReturnOk_WhenEmployeeAndBranchAreCorrect()
    {
        //arrange
        var employee = new Employee(1, "Karol", "Kowalski", "karol.kowalski@email.com", EmployeeRole.Boss);
        var branch = new Branch(2, "Wrocław", "Główna", "22", new List<Employee>(), BranchStatus.Active);
        var employeeId = 1;
        var branchId = 2;
        _branchRepository.Setup(x => x.GetBranch(branch.BranchId)).ReturnsAsync(branch);
        _employeeRepository.Setup(x => x.GetEmployee(employee.EmployeeId)).ReturnsAsync(employee);
        //act
        var result = await _branchesService.AddEmployee(employeeId, branchId);
        //assert
        result.Succeeded.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();
        result.Status.Should().BeNull();
    }
    [Fact]
    public async Task RemoveEmployee_ShouldReturnFailureResult_WhenEmployeeIsNotFound()
    {
        //arrange
        Employee employee = null;
        var employeeId = 1;
        var branchId = 2;
        _employeeRepository.Setup(x => x.GetEmployee(employeeId)).ReturnsAsync(employee);
        //Act
        var result =await _branchesService.RemoveEmployee(employeeId, branchId);
        //Assert
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.NotFound);
        result.Message.Should().NotBeNullOrEmpty();
        _employeeRepository.Verify(x => x.GetEmployee(employeeId),Times.Once);
    }
    [Fact]
    public async Task RemoveEmployee_ShouldReturnFailureResult_WhenBranchIsNull()
    {
        //arrange
        Branch branch = null;
        var employee = new Employee(1, "Karol", "Kowalski", "karol.kowalski@email.com", EmployeeRole.Boss);
        var employeeId = 1;
        var branchId = 2;
        _branchRepository.Setup(x => x.GetBranch(branchId)).ReturnsAsync(branch);
        _employeeRepository.Setup(x => x.GetEmployee(employeeId)).ReturnsAsync(employee);
        //act
        var result =await _branchesService.RemoveEmployee(employeeId, branchId);
        //assert
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.NotFound);
        result.Message.Should().NotBeNullOrEmpty();
        _branchRepository.Verify(x => x.GetBranch(branchId), Times.Once);
    }
    [Fact]
    public async Task RemoveEmployee_ShouldReturnOk_WhenEmployeeAndBranchAreCorrect()
    {
        //arrange
        var employee = new Employee(1, "Karol", "Kowalski", "karol.kowalski@email.com", EmployeeRole.Boss);
        var branch = new Branch(2, "Wrocław", "Główna", "22", new List<Employee>(), BranchStatus.Active);
        var employeeId = 1;
        var branchId = 2;
        _branchRepository.Setup(x => x.GetBranch(branch.BranchId)).ReturnsAsync(branch);
        _employeeRepository.Setup(x => x.GetEmployee(employee.EmployeeId)).ReturnsAsync(employee);
        //act
        var result =await _branchesService.RemoveEmployee(employeeId, branchId);
        //assert
        result.Succeeded.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();
        result.Status.Should().BeNull();
    }

    [Fact]
    public async Task ChangeStatus_ShouldReturnFailureResult_WhenBranchIsNotFound()
    {
        //arrange
        Branch branch = null;
        var branchId = 1;
        _branchRepository.Setup(x => x.GetBranch(branchId)).ReturnsAsync(branch);
        //act
        var result =await _branchesService.ChangeStatus(branchId, BranchStatus.Active);
        //assert
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(ErrorStatus.NotFound);
        result.Message.Should().NotBeNullOrEmpty();
        _branchRepository.Verify(x => x.GetBranch(branchId),Times.Once);
    }

    [Fact]
    public async Task ChangeStatus_ShouldReturnFailureResult_WhenBranchIsActiveBranchHaveEmptyEmployeeList()
    {
        //arrange
        var branch = new Branch(2, "Wrocław", "Główna", "22", new List<Employee>(), BranchStatus.Pending);
        var branchId = 2;
        _branchRepository.Setup(x => x.GetBranch(branch.BranchId)).ReturnsAsync(branch);
        //act
        var result = await _branchesService.ChangeStatus(branchId, BranchStatus.Active);
        //assert
        result.Succeeded.Should().BeFalse();
        result.Message.Should().NotBeNullOrEmpty();
        result.Status.Should().Be(ErrorStatus.Failure);
    }

    [Fact]
    public async Task ChangeStatus_ShouldReturnOk_WhenBranchIsValid()
    {
        //arrange
        var employee = new Employee(1, "Karol", "Kowalski", "karol.kowalski@email.com", EmployeeRole.Boss);
        var employee1 = new Employee(2, "Maciej", "Skonieczny", "maciej.skonieczny@email.com", EmployeeRole.Manager);
        var employees = new List<Employee> { employee, employee1 };
        var branch = new Branch(2, "Wrocław", "Główna", "22", employees, BranchStatus.Pending);
        var branchId = 2;
        _branchRepository.Setup(x => x.GetBranch(branch.BranchId)).ReturnsAsync(branch);
        //act
        var result = await _branchesService.ChangeStatus(branchId, BranchStatus.Active);
        //assert
        result.Succeeded.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();
        result.Status.Should().BeNull();
    }
}