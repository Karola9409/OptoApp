using System;
using OptoApi.Models;

namespace OptoApi.Services
{
    public class EmployeesService
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

    }
}

