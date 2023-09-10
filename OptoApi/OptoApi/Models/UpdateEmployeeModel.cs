namespace OptoApi.Models;

public class UpdateEmployeeModel
{
    public UpdateEmployeeModel(int id, string lastName, string email, EmployeeRole employeeRole)
    {
        ID = id;
        LastName = lastName;
        Email = email;
        EmployeeRole = employeeRole;
    }
    public int ID { get; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public EmployeeRole EmployeeRole { get; set; }
}