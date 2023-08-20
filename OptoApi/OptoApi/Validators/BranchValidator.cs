using System.Security.Cryptography;
using OptoApi.Models;
namespace OptoApi.Validators;

public class BranchValidator
{
    public ValidationResult IsValid(Branch branch)
    {
        if(string.IsNullOrEmpty(branch.City))
        {
            return new ValidationResult(false, "The City field can't be empty.");
        }
        if(string.IsNullOrEmpty(branch.StreetName))
        {
            return new ValidationResult(false, "The Street name field can't be empty.");
        }
        if(string.IsNullOrEmpty(branch.StreetNumber))
        {
            return new ValidationResult(false, "The Street number field can't be empty.");
        }

        if (!branch.StreetNumber.Any(char.IsDigit))
        {
            return new ValidationResult(false, "The Street number field must contain some number.");
        }

        if (branch.BranchStatus == BranchStatus.Active && branch.Employees.Count == 0)
        {
            return new ValidationResult(false, "Active branch must have some employee");
        }
        if (!Enum.IsDefined(typeof(BranchStatus), branch.BranchStatus))
        {
            return new ValidationResult(false, "Branch status is not found.");
        }
        return new ValidationResult(true, "");
    }
}