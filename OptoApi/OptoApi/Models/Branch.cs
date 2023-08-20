namespace OptoApi.Models;

public class Branch
{
    public Branch(int branchId, string city, string streetName, string streetNumber, List<Employee> employees, BranchStatus branchStatus)
    {
        BranchId = branchId;
        StreetName = streetName;
        StreetNumber = streetNumber;
        Employees = employees;
        City = city;
        BranchStatus = branchStatus;
    }
    public int BranchId { get; }
    public string City { get; }
    public string StreetName { get; }
    public string StreetNumber { get; }
    public List<Employee> Employees { get; }
    public BranchStatus BranchStatus { get; }
}