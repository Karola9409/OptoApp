using FluentAssertions;
using OptoApi.Models;

namespace OptoApi.Tests;

public class OperationResultTests
{
    [Fact]
    public void CreatedSuccessOperationResult_WithData_ShouldHaveExpectedPropertiesValues()
    {
        //Arrange
        var data = "some data";
        
        //Act
        var operationResult = OperationResult<string>.Success(data);

        //Assert
        operationResult.Data.Should().BeEquivalentTo(data);
        operationResult.Message.Should().BeEmpty();
        operationResult.Status.Should().BeNull();
        operationResult.Succeeded.Should().BeTrue();
    }
    [Fact]
    public void CreatedFilureOperationResult_WithData_ShouldHaveExpectedPropertiesValues()
    {
        //Arrange
        var message = "some message";
        var status = ErrorStatus.NotFound;
        
        //Act
        var operationResult = OperationResult<string>.Failure(message,status);

        //Assert
        operationResult.Data.Should().Be(default);
        operationResult.Message.Should().Be(message);
        operationResult.Status.Should().Be(status);
        operationResult.Succeeded.Should().BeFalse();
    }
    
        [Fact]
        public void CreatedSuccessOperationResult_ShouldHaveExpectedPropertiesValues()
        {
            //Act
            var operationResult = OperationResult.Success();

            //Assert
            operationResult.Message.Should().BeEmpty();
            operationResult.Status.Should().BeNull();
            operationResult.Succeeded.Should().BeTrue();
        }
        [Fact]
        public void CreatedFailureOperationResult_ShouldHaveExpectedPropertiesValues()
        {
            //Arrange
            var message = "some message";
            var status = ErrorStatus.NotFound;
        
            //Act
            var operationResult = OperationResult.Failure(message,status);

            //Assert
            operationResult.Message.Should().Be(message);
            operationResult.Status.Should().Be(status);
            operationResult.Succeeded.Should().BeFalse();
        }
   
}

