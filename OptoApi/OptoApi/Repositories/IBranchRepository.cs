using OptoApi.Models;

namespace OptoApi.Repositories;

public interface IBranchRepository
{
    Task<List<Branch>> GetAllBranches();
    Task<Branch?> GetBranch(int id);
    Task<int> AddBranch(Branch branch);
    Task AddEmployee(int employeeId, int branchId);
    Task RemoveEmployee(int employeeId, int branchId);
    Task ChangeStatus(int branchId, BranchStatus branchStatus);
}