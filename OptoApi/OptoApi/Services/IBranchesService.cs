using OptoApi.Models;

namespace OptoApi.Services;

public interface IBranchesService
{
    List<Branch> GetAllBranches();
    Branch? GetBranch(int id);
    int AddBranch(Branch branchToAdd);
    void AddEmployee(int employeeId, int branchId);
    void RemoveEmployee(int employeeId, int branchId);
    void ChangeStatus(int branchId, BranchStatus branchStatus);
}