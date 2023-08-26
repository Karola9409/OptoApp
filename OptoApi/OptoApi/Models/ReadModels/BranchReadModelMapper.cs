namespace OptoApi.Models.ReadModels;

public class BranchReadModelMapper
{
    public static List<Branch> Map(IEnumerable<BranchReadModel>readModels)
    {
        var result = new List<Branch>();
        var groupedReadModels = readModels.GroupBy(x => x.BranchId).ToDictionary(x => x.Key, x => x.ToList());
        foreach (var (branchId, group) in groupedReadModels)
        {
            var employees = group.Where(x => x.EmployeeId != null).Select(x => new Employee(
                x.EmployeeId.Value,
                x.FirstName,
                x.LastName,
                x.Email,
                x.EmployeeRole.Value,
                x.IsDeleted)).ToList();

            var firstReadModel = group.First();
            var branch = new Branch(
                firstReadModel.BranchId,
                firstReadModel.City,
                firstReadModel.StreetName,
                firstReadModel.StreetNumber,
                employees,
                firstReadModel.BranchStatus);

            result.Add(branch);
        }
        return result;
    }
}