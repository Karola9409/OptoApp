using System.Text.RegularExpressions;
using OptoApi.Models;

namespace OptoApi.Validators
{
    public class EmployeeValidator
    {
        public ValidationResult IsValid(Employee employee)
        {
            if (string.IsNullOrEmpty(employee.FirstName))
            {
                return new ValidationResult(false, "Name can't be empty field.");
            }

            if(string.IsNullOrEmpty(employee.LastName))
            {
                return new ValidationResult(false, "Name can't be empty field.");
            }

            if (!Enum.IsDefined(typeof(EmployeeRole), employee.EmployeeRole))
            {
                return new ValidationResult(false, "Employee role is not found.");
            }

            if (!IsValidEmail(employee.Email))
            {
                return new ValidationResult(false, "Email adress is incorrect.");
            }

            return new ValidationResult(true, "");
        }
        
        public ValidationResult IsValid(UpdateEmployeeModel employee)
        {
            if(string.IsNullOrEmpty(employee.LastName))
            {
                return new ValidationResult(false, "Name can't be empty field.");
            }

            if (!Enum.IsDefined(typeof(EmployeeRole), employee.EmployeeRole))
            {
                return new ValidationResult(false, "Employee role is not found.");
            }

            if (!IsValidEmail(employee.Email))
            {
                return new ValidationResult(false, "Email adress is incorrect.");
            }

            return new ValidationResult(true, "");
        }

        private static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}

