using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Npgsql;
using OptoApi.Models;
using OptoApi.Models.ReadModels;
using OptoApi.Options;
using OptoApi.Sql;

namespace OptoApi.Services;

public class BranchesService : IBranchesService 
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;
    public BranchesService(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }
    public List<Branch> GetAllBranches()
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);
        
        var readModels = connection.Query<BranchReadModel>(BranchesSql.GetAllBranches);
        return BranchReadModelMapper.Map(readModels);
    }
    public Branch? GetBranch(int id)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var readModels = connection.Query<BranchReadModel>(BranchesSql.GetBranch, new
        {
            BranchId = id
        });
        var mappedBranches = BranchReadModelMapper.Map(readModels);
        return mappedBranches.SingleOrDefault();
    }
    public int AddBranch(Branch branch)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);
        
        var result = connection.Execute(BranchesSql.AddBranch, new
        {
            branch.City,
            branch.StreetName,
            branch.StreetNumber,
            branch.BranchStatus
        });
        return result;
    }

    public void AddEmployee(int employeeId, int branchId)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result = connection.Execute(BranchesSql.AddEmployee, new
        {
            branchId,
            employeeId
        });
    }
    public void RemoveEmployee(int employeeId, int branchId)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result = connection.Execute(BranchesSql.RemoveEmployee, new
        {
            employeeId,
            branchId
        });
    }
    public void ChangeStatus(int branchId, BranchStatus branchStatus)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        connection.Execute(BranchesSql.ChangeStatus, new
        {
            branchId,
            branchStatus
        });
    }
}