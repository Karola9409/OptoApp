using OptoApi.Models;

namespace OptoApi.Services;

public interface IEmployeesService
{ 
    Task<List<Employee>> GetAllEmployees();

    Task<OperationResult<Employee>> GetEmployee(int id);

    Task<OperationResult<int>> AddEmployee(Employee employee);

    Task<OperationResult> UpdateEmployee(UpdateEmployeeModel employee);

    Task<OperationResult<bool>> RemoveEmployee(int employeeId);
}