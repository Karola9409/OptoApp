using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using OptoApi.Models;
using OptoApi.Options;
using OptoApi.Sql;

namespace OptoApi.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;

    public EmployeeRepository(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }
    public async Task<List<Employee>> GetAllEmployees()
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result = await connection.QueryAsync<Employee>(EmployeesSql.GetAllEmployees);

        return result.AsList();
    }
    public async Task<Employee?> GetEmployee(int id)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result = await connection.QueryFirstOrDefaultAsync<Employee>(EmployeesSql.GetEmployee, new
        {
            EmployeeId = id
        });
        return result;
    }
    public async Task<int> AddEmployee(Employee employee)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result = await connection.ExecuteAsync(EmployeesSql.AddEmployee, new
        {
            employee.FirstName,
            employee.LastName,
            employee.Email,
            employee.EmployeeRole,
            employee.IsDeleted
        });
        return result;
    }
    public async Task UpdateEmployee(UpdateEmployeeModel employee)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

       await connection.ExecuteAsync(EmployeesSql.UpdateEmployee, new
        {
            employee.ID,
            employee.LastName,
            employee.Email,
            employee.EmployeeRole
        });
    }
    public async Task<bool> RemoveEmployee(int employeeId)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        await connection.ExecuteAsync(EmployeesSql.RemoveEmployee, new
        {
            EmployeeId = employeeId
        });
        return true;
    }
}