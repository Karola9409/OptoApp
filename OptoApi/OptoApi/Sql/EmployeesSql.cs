using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OptoApi.Models;

namespace OptoApi.Sql;

public class EmployeesSql
{
	public static string GetAllEmployees = @"SELECT 
	""EmployeeId"",
    ""FirstName"",
    ""LastName"",
    ""Email"",
    ""EmployeeRoleId"" AS EmployeeRole,
    ""IsDeleted""
    FROM public.""Employee"";";

	public static string GetEmployee = @"SELECT 
	""EmployeeId"",
    ""FirstName"",
    ""LastName"",
    ""Email"",
    ""EmployeeRoleId"" AS EmployeeRole,
    ""IsDeleted""
    FROM public.""Employee""
    WHERE ""EmployeeId""= @EmployeeId;";

	public static string AddEmployee = @"INSERT INTO public.""Employee""
	    (""FirstName"",
		""LastName"",
		""Email"",
		""EmployeeRoleId"",
		""IsDeleted"")
	    VALUES(@FirstName,
		       @LastName,
		       @Email,
		       @EmployeeRole,
		       @IsDeleted);";

	public static string UpdateEmployee = @"UPDATE public.""Employee"" SET
	    ""FirstName""= @FirstName,
		""LastName""= @LastName,
		""Email""= @Email,
		""EmployeeRoleId""= @EmployeeRole,
		""IsDeleted""= @IsDeleted
    	WHERE ""EmployeeId""= @EmployeeId;";

	public static string RemoveEmployee = @"UPDATE public.""Employee""
	SET ""IsDeleted""= true
	WHERE ""EmployeeId""= @employeeId";
}

