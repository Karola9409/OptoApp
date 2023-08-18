using OptoApi.Models;

namespace OptoApi.Services;

public interface IEmployeesService
{ 
    List<Employee> GetAllEmployees();

    Employee? GetEmployee(int id);

    int AddEmployee(Employee employee);

    void UpdateEmployee(Employee employee);

    bool RemoveEmployee(int employeeId);
}