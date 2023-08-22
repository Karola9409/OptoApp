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
    public IActionResult GetAllBranches()
    {
        var branches = _branchesService.GetAllBranches();
        return Ok(branches);
    }

    [HttpGet("Get/{id}")]
    public IActionResult GetBranch(int id)
    {
        var result = _branchesService.GetBranch(id);
        if (result == null)
        {
            return NotFound($"404, Branch with id {id} not found");
        }
        return Ok(result);
    }

    [HttpPost("Add")]
    public IActionResult AddBranch([FromBody] ApiRequestAddBranch addBranch)
    {
        var branchToAdd = new Branch(
            0,
            addBranch.City,
            addBranch.StreetName,
            addBranch.StreetNumber,
            new List<Employee>(),
            BranchStatus.Pending);

        var validationResult = _branchValidator.IsValid(branchToAdd);
        if (validationResult.IsValid is false)
        {
            return BadRequest($"Branch is invalid: {validationResult.ErrorMessage}");
        }
        var branchId = _branchesService.AddBranch(branchToAdd);
        return Ok(branchId);
    }
    
    [HttpPut("{branchId}/AddEmployee/{employeeId}")]
    public IActionResult AddEmployee(int employeeId, int branchId)
    {
        var employee = _employeesService.GetEmployee(employeeId);
        if (employee == null)
        {
            return NotFound($"404, Employee with id {employeeId} not found");
        }

        var branch = _branchesService.GetBranch(branchId);
        if (branch == null)
        {
            return NotFound($"404, Branch with id {branchId} not found");
        }
        _branchesService.AddEmployee(employeeId, branchId);
        return Ok();
    }
    
    [HttpPut("{branchId}/RemoveEmployee/{employeeId}")]
    public IActionResult RemoveEmployee(int employeeId, int branchId)
    {
        var employee = _employeesService.GetEmployee(employeeId);
        if (employee == null)
        {
            return NotFound($"404, Employee with id {employeeId} not found");
        }

        var branch = _branchesService.GetBranch(branchId);
        if (branch == null)
        {
            return NotFound($"404, Branch with id {branchId} not found");
        }
        _branchesService.RemoveEmployee(employeeId, branchId);
        return Ok();
    }
    
    [HttpPut("{branchId}/ChangeStatus/{branchStatus}")]
    public IActionResult ChangeStatus(BranchStatus branchStatus, int branchId)
    {
        var branch = _branchesService.GetBranch(branchId);
        if (branch == null)
        {
            return NotFound($"404, Branch with id {branchId} not found");
        }
        _branchesService.ChangeStatus(branchId, branchStatus);
        return Ok();
    }
}
