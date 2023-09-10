using Dapper;
using OptoApi.Models;
using OptoApi.Repositories;
using OptoApi.Validators;

namespace OptoApi.Services;

public class BranchesService : IBranchesService
{
    private readonly IBranchRepository _branchRepository;
    private readonly BranchValidator _branchValidator;
    private readonly IEmployeeRepository _employeeRepository;
    public BranchesService(
        IBranchRepository branchRepository, 
        BranchValidator branchValidator,
        IEmployeeRepository employeeRepository)
    {
        _branchRepository = branchRepository;
        _branchValidator = branchValidator;
        _employeeRepository = employeeRepository;
    }
    public async Task<List<Branch>> GetAllBranches()
    {
        var result =await  _branchRepository.GetAllBranches();
        return result.AsList();
    }
    public async Task<OperationResult<Branch>> GetBranch(int id)
    {
        var readModels =await _branchRepository.GetBranch(id);
        if (readModels == null)
        {
            return OperationResult<Branch>.Failure(
                $"Branch with id {id} not found", 
                ErrorStatus.NotFound);
        }
        return OperationResult<Branch>.Success(readModels);
    }
    public async Task<OperationResult<int>> AddBranch(Branch branch)
    {
        var validationResult = _branchValidator.IsValid(branch);
        if (validationResult.IsValid is false)
        {
            return OperationResult<int>.Failure(
                $"Branch is invalid: {validationResult.ErrorMessage}",
                ErrorStatus.NotValid);
        }
        var result =await _branchRepository.AddBranch(branch);
        return OperationResult<int>.Success(result);
    }
    public async Task<OperationResult> AddEmployee(int employeeId, int branchId)
    {
        var employee = await _employeeRepository.GetEmployee(employeeId);
        if (employee == null)
        {
            return OperationResult.Failure(
                $"Employee with id {employeeId} not found",
                ErrorStatus.NotFound);
        }

        var branch = await _branchRepository.GetBranch(branchId);
        if (branch == null)
        {
            return OperationResult.Failure(
                $"Branch with id {branchId} not found",
                ErrorStatus.NotFound);
        }

        await _branchRepository.AddEmployee(employeeId, branchId);
        return OperationResult.Success();
    }
    public async Task<OperationResult> RemoveEmployee(int employeeId, int branchId)
    {
        var employee =await _employeeRepository.GetEmployee(employeeId);
        if (employee == null)
        {
            return OperationResult.Failure(
                $"Employee with id {employeeId} not found",
                ErrorStatus.NotFound);
        }

        var branch =await _branchRepository.GetBranch(branchId);
        if (branch == null)
        {
            return OperationResult.Failure(
                $"Branch with id {branchId} not found",
                ErrorStatus.NotFound);
        }

        await _branchRepository.RemoveEmployee(employeeId, branchId);
        return OperationResult.Success();
    }
    public async Task<OperationResult> ChangeStatus(int branchId, BranchStatus branchStatus)
    {
        var branch = await _branchRepository.GetBranch(branchId);
        if (branch == null)
        {
            return OperationResult.Failure(
                $"Branch with id {branchId} not found", 
                ErrorStatus.NotFound);
        }

        if (branchStatus == BranchStatus.Active && branch.Employees.Count == 0)
        {
            return OperationResult.Failure(
                $"Active branch must have employees",
                ErrorStatus.Failure);
        }
        await _branchRepository.ChangeStatus(branchId, branchStatus);
        return OperationResult.Success();
    }
}