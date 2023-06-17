using OptoApi.Models;

namespace OptoApi.ApiModels;

public class ApiRequestUpdateEmployee
{
    public ApiRequestUpdateEmployee(string lastName, string email, EmployeeRole employeeRole)
    {
        LastName = lastName;
        Email = email;
        EmployeeRole = employeeRole;
    }
    public string LastName { get; set; }
    public string Email { get; set; }
    public EmployeeRole EmployeeRole { get; set; }
}
