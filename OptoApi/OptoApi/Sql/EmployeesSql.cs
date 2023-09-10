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
		""LastName""= @LastName,
		""Email""= @Email,
		""EmployeeRoleId""= @EmployeeRole
    	WHERE ""EmployeeId""= @EmployeeId;";

	public static string RemoveEmployee = @"UPDATE public.""Employee""
	SET ""IsDeleted""= true
	WHERE ""EmployeeId""= @employeeId";
}

