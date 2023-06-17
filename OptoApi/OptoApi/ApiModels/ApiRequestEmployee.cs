using System;
using OptoApi.Models;

namespace OptoApi.ApiModels
{
    public class ApiRequestEmployee
    {
        public ApiRequestEmployee (string firstName, string lastName, string email, EmployeeRole employeeRole)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            EmployeeRole = employeeRole;
        }
        
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public EmployeeRole EmployeeRole { get; }
        
    }
}

