using OptoApi.Models;

namespace OptoApi.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllEmployees();
    Task<Employee?> GetEmployee(int id);
    Task<int> AddEmployee(Employee employee);
    Task UpdateEmployee(UpdateEmployeeModel employee);
    Task<bool> RemoveEmployee(int employeeId);
}