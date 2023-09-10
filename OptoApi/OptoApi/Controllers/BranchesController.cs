using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OptoApi.ApiModels;
using OptoApi.Models;
using OptoApi.Services;
using OptoApi.Validators;

namespace OptoApi.Controllers;

[ApiController]
[Route("[controller]")]

public class BranchesController : ControllerBase
{
    private readonly IBranchesService _branchesService;
    private readonly BranchValidator _branchValidator;
    private readonly IEmployeesService _employeesService;

    public BranchesController(IBranchesService branchesService, BranchValidator branchValidator, IEmployeesService employeesService)
    {
        _branchesService = branchesService;
        _branchValidator = branchValidator;
        _employeesService = employeesService;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllBranches()
    {
        var branches =await _branchesService.GetAllBranches();
        return Ok(branches);
    }

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetBranch(int id)
    {
        var operationResult = await _branchesService.GetBranch(id);
        if (operationResult.Succeeded is false)
        {
            return BadRequest($"Operation result: {operationResult.Status}, {operationResult.Message}");
        }
        return Ok(operationResult.Data);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddBranch([FromBody] ApiRequestAddBranch addBranch)
    {
        var branchToAdd = new Branch(
            0,
            addBranch.City,
            addBranch.StreetName,
            addBranch.StreetNumber,
            new List<Employee>(),
            BranchStatus.Pending);

        var operationResult =await _branchesService.AddBranch(branchToAdd);
        if (operationResult.Succeeded is false)
        {
            return BadRequest($"Branch is invalid: {operationResult.Status}, {operationResult.Message}");
        }
        return Ok(operationResult.Data);
    }
    
    [HttpPut("{branchId}/AddEmployee/{employeeId}")]
    public async Task<IActionResult> AddEmployee(int employeeId, int branchId)
    {
        var operationResult =await _branchesService.AddEmployee(employeeId, branchId);
        if(operationResult.Succeeded is false)
        {
            return BadRequest(
                $"Operation result: {operationResult.Status}, {operationResult.Message}");
        }
        return Ok();
    }
    
    [HttpPut("{branchId}/RemoveEmployee/{employeeId}")]
    public async Task<IActionResult> RemoveEmployee(int employeeId, int branchId)
    {
        var operationResult = await _branchesService.RemoveEmployee(employeeId, branchId);
        if(operationResult.Succeeded is false)
        {
            return BadRequest(
                $"Operation result: {operationResult.Status}, {operationResult.Message}");
        }
        return Ok();
    }
    
    [HttpPut("{branchId}/ChangeStatus/{branchStatus}")]
    public async Task<IActionResult> ChangeStatus(BranchStatus branchStatus, int branchId)
    {
        var operationResult =await _branchesService.ChangeStatus(branchId, branchStatus);
        if(operationResult.Succeeded is false)
        {
            return BadRequest(
                $"Operation result: {operationResult.Status}, {operationResult.Message}");
        }
        return Ok();
    }
}
