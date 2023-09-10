using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using OptoApi.Models;
using OptoApi.Models.ReadModels;
using OptoApi.Options;
using OptoApi.Sql;

namespace OptoApi.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;

    public BranchRepository(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }
    
    public async Task<List<Branch>> GetAllBranches()
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);
        
        var readModels =await connection.QueryAsync<BranchReadModel>(BranchesSql.GetAllBranches);
        return BranchReadModelMapper.Map(readModels);
    }
    public async Task<Branch?> GetBranch(int id)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var readModels = await connection.QueryAsync<BranchReadModel>(BranchesSql.GetBranch, new
        {
            BranchId = id
        });
        var mappedBranches = BranchReadModelMapper.Map(readModels);
        return mappedBranches.SingleOrDefault();
    }
    public async Task<int> AddBranch(Branch branch)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);
        
        var result =await connection.ExecuteAsync(BranchesSql.AddBranch, new
        {
            branch.City,
            branch.StreetName,
            branch.StreetNumber,
            branch.BranchStatus
        });
        return result;
    }

    public async Task AddEmployee(int employeeId, int branchId)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result = await connection.ExecuteAsync(BranchesSql.AddEmployee, new
        {
            branchId,
            employeeId
        });
    }
    public async Task RemoveEmployee(int employeeId, int branchId)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result =await connection.ExecuteAsync(BranchesSql.RemoveEmployee, new
        {
            employeeId,
            branchId
        });
    }
    public async Task ChangeStatus(int branchId, BranchStatus branchStatus)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        await connection.ExecuteAsync(BranchesSql.ChangeStatus, new
        {
            branchId,
            branchStatus
        });
    }
}