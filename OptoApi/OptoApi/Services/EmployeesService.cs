using Dapper;
using OptoApi.Models;
using OptoApi.Repositories;
using OptoApi.Validators;

namespace OptoApi.Services;

public class EmployeesService : IEmployeesService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly EmployeeValidator _employeeValidator;

    public EmployeesService(IEmployeeRepository employeeRepository, EmployeeValidator employeeValidator)
    {
        _employeeRepository = employeeRepository;
        _employeeValidator = employeeValidator;
    }

    public async Task<List<Employee>> GetAllEmployees()
    {
        var result = await _employeeRepository.GetAllEmployees();
        return result.AsList();
    }

    public async Task<OperationResult<Employee>> GetEmployee(int id)
    {
        var employee = await _employeeRepository.GetEmployee(id);
        if (employee == null)
        {
            return OperationResult<Employee>.Failure($"Employee with id {id} not found", ErrorStatus.NotFound);
        }

        return OperationResult<Employee>.Success(employee);
    }

    public async Task<OperationResult<int>> AddEmployee(Employee employee)
    {
        var validationResult = _employeeValidator.IsValid(employee);
        if (validationResult.IsValid is false)
        {
            return OperationResult<int>.Failure(
                $"Employee is invalid: {validationResult.ErrorMessage}", ErrorStatus.NotValid);
        }

        var result = await _employeeRepository.AddEmployee(employee);
        return OperationResult<int>.Success(result);
    }

    public async Task<OperationResult> UpdateEmployee(UpdateEmployeeModel employee)
    {
        var existingEmployee = await _employeeRepository.GetEmployee(employee.ID);
        if (existingEmployee == null)
        {
            return OperationResult.Failure(
                $"Employee with id {employee.ID} not found", ErrorStatus.NotFound);
        }

        var validationResult = _employeeValidator.IsValid(employee);
        if (validationResult.IsValid is false)
        {
            return OperationResult.Failure($"Employee is invalid:", ErrorStatus.NotValid);
        }

        await _employeeRepository.UpdateEmployee(employee);
        return OperationResult.Success();
    }

    public async Task<OperationResult<bool>> RemoveEmployee(int employeeId)
    {
        var employee =await _employeeRepository.GetEmployee(employeeId);
        if (employee == null)
        {
            return OperationResult<bool>.Failure(
                $"Employee with id {employeeId} not found", ErrorStatus.NotFound);
        }

        var result =await _employeeRepository.RemoveEmployee(employeeId);
        return OperationResult<bool>.Success(result);
    }
}