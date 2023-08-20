﻿using System;
using OptoApi.Models;

namespace OptoApi.Services
{
    public class FakeEmployeesService : IEmployeesService
    {
        private static List<Employee> EmployeeList = new List<Employee>
        {
            new Employee(1,"Kasia","Nowak", "kasia.nowak@optoapi.com", EmployeeRole.Boss),
            new Employee(2,"Adam","Kowalski", "adam.kowalski@optoapi.com", EmployeeRole.Manager),
            new Employee(3,"Ingeborga","Adamiec", "ingeborga.adamiec@optoapi.com", EmployeeRole.Optician),
            new Employee(4,"Piotr","Waciak", "piotr.waciak@optoapi.com", EmployeeRole.Optometrist),
            new Employee(5,"Agata","Majewska", "agata.majewska@optoapi.com",EmployeeRole.Seller),
        };

        public List<Employee> GetAllEmployees()
        {
            return EmployeeList;
        }

        public Employee? GetEmployee(int id)
        {
            var result = EmployeeList.Find(x => x.EmployeeId == id);
            return result;
        }
        
        public int AddEmployee(Employee employee)
        {
            var employeeId = EmployeeList.Max(x => x.EmployeeId) + 1;
            employee.EmployeeId = employeeId;
            EmployeeList.Add(employee);
            return employeeId;
        }
        public void UpdateEmployee(Employee employee)
        {
            var employeeToUpdate = EmployeeList.Find(x => x.EmployeeId == employee.EmployeeId);
            
            EmployeeList.Remove(employeeToUpdate!);
            EmployeeList.Add(employee);
        }
        public bool RemoveEmployee(int employeeId)
        {
            var employeeToRemove = EmployeeList.Find(x => x.EmployeeId == employeeId);

            if (employeeToRemove is not null)
            {
                employeeToRemove.IsDeleted = true;
                return true;
            }
            return false;
        }
    }
}
