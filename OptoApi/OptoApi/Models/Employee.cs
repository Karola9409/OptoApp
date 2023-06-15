using System;
namespace OptoApi.Models
{
    public class Employee
    {
        public Employee(int employeeId, string firstName, string lastName, string email, EmployeeRole employeeRole, bool isDeleted = false)
        {
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            EmployeeRole = employeeRole;
            IsDeleted = isDeleted;
        }

        public int EmployeeId { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public EmployeeRole EmployeeRole { get; }

        public bool IsDeleted { get; }
    }
}

