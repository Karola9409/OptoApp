using FluentAssertions;
using OptoApi.Models;
using OptoApi.Models.ReadModels;

namespace OptoApi.Tests;

public class BranchReadModelMapperTests
{
    [Fact]
    public void WhenReadModels_AreEmpty_ThenEmptyListShouldBeReturned()
    {
        //Arrange
        var readModels = new List<BranchReadModel>();
        //Act
        var result = BranchReadModelMapper.Map(readModels);
        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void WhenReadModels_AreNotEmpty_ThenExpectedResultShouldBeReturned()
    {
        //Arrange
        var readModels = new List<BranchReadModel>
        {
            new BranchReadModel
            {
                BranchId = 1,
                BranchStatus = BranchStatus.Pending,
                City = "Gdańsk",
                Email = "email@email.com",
                EmployeeId = 1,
                EmployeeRole = EmployeeRole.Boss,
                FirstName = "Kasia",
                LastName = "Kowalska",
                IsDeleted = false,
                StreetName = "Główna",
                StreetNumber = "2",
            },
            new BranchReadModel
            {
                BranchId = 1,
                BranchStatus = BranchStatus.Pending,
                City = "Gdańsk",
                Email = "basia@email.com",
                EmployeeId = 2,
                EmployeeRole = EmployeeRole.Manager,
                FirstName = "Basia",
                LastName = "Nowak",
                IsDeleted = true,
                StreetName = "Główna",
                StreetNumber = "2",
            },
            new BranchReadModel
            {
                BranchId = 2,
                BranchStatus = BranchStatus.Active,
                City = "Toruń",
                Email = "basia@email.com",
                EmployeeId = 2,
                EmployeeRole = EmployeeRole.Manager,
                FirstName = "Basia",
                LastName = "Nowak",
                IsDeleted = true,
                StreetName = "Ciemna",
                StreetNumber = "9",
            }
        };
        var expectedResult = new List<Branch>
        {
            new Branch(1,"Gdańsk", "Główna", "2", new List<Employee>
            {
                new Employee(1,"Kasia", "Kowalska", "email@email.com", EmployeeRole.Boss, false),
                new Employee(2,"Basia", "Nowak", "basia@email.com", EmployeeRole.Manager, true),
            },BranchStatus.Pending),
            new Branch(2,"Toruń", "Ciemna", "9", new List<Employee>
            {
                new Employee(2,"Basia", "Nowak", "basia@email.com", EmployeeRole.Manager, true)
            },BranchStatus.Active),
        };
        
        //Act
        var result = BranchReadModelMapper.Map(readModels);
        //Assert
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expectedResult);
    }
     [Fact]
    public void WhenReadModels_EmployeeIdIsNull_ThenExpectedResultShouldBeReturned()
    {
        //Arrange
        var readModels = new List<BranchReadModel>
        {
            new BranchReadModel
            {
                BranchId = 1,
                BranchStatus = BranchStatus.Pending,
                City = "Gdańsk",
                Email = null,
                EmployeeId = null,
                EmployeeRole = null,
                FirstName = null,
                LastName = null,
                IsDeleted = false,
                StreetName = "Główna",
                StreetNumber = "2",
            }
        };
        var expectedResult = new List<Branch>
        {
            new Branch(1,"Gdańsk", "Główna", "2", new List<Employee>(),BranchStatus.Pending),
        };
        
        //Act
        var result = BranchReadModelMapper.Map(readModels);
        //Assert
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expectedResult);
    }
}