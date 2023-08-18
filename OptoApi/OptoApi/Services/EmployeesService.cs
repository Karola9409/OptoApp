using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using OptoApi.Models;
using OptoApi.Options;
using OptoApi.Sql;

namespace OptoApi.Services;

public class EmployeesService : IEmployeesService
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;

    public EmployeesService(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public List<Employee> GetAllEmployees()
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result = connection.Query<Employee>(EmployeesSql.GetAllEmployees);

        return result.AsList();
    }

    public Employee? GetEmployee(int id)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result = connection.QueryFirstOrDefault<Employee>(EmployeesSql.GetEmployee, new
        {
            EmployeeId = id
        });
        return result;
    }

    public int AddEmployee(Employee employee)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result = connection.Execute(EmployeesSql.AddEmployee, new
        {
            employee.FirstName,
            employee.LastName,
            employee.Email,
            employee.EmployeeRole,
            employee.IsDeleted
        });
        return result;
    }

    public void UpdateEmployee(Employee employee)
    {
        {
            using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

            connection.Execute(EmployeesSql.UpdateEmployee, new
            {
                employee.EmployeeId,
                employee.FirstName,
                employee.LastName,
                employee.Email,
                employee.EmployeeRole,
                employee.IsDeleted
            });
        }
    }
    public bool RemoveEmployee(int employeeId)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute(EmployeesSql.RemoveEmployee, new
        {
            EmployeeId = employeeId
        });
        return true;
    }
}