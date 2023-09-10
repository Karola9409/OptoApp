using OptoApi.Models;

namespace OptoApi.Services;

public interface IBranchesService
{
    Task<List<Branch>> GetAllBranches();
    Task<OperationResult<Branch>> GetBranch(int id);
    Task<OperationResult<int>> AddBranch(Branch branchToAdd);
    Task<OperationResult> AddEmployee(int employeeId, int branchId);
    Task<OperationResult> RemoveEmployee(int employeeId, int branchId);
    Task<OperationResult> ChangeStatus(int branchId, BranchStatus branchStatus);
}