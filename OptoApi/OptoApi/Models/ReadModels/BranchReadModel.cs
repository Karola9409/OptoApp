namespace OptoApi.Models.ReadModels;

public class BranchReadModel 
{
    public int BranchId { get; set; }
    
    public string City { get; set; }
    
    public string StreetName { get; set; }
    
    public string StreetNumber { get; set; }
    
    public BranchStatus BranchStatus { get; set; }
    
    public int? EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public EmployeeRole? EmployeeRole { get; set; }

    public bool IsDeleted { get; set; }
}