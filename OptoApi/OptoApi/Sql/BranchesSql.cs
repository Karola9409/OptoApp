namespace OptoApi.Sql;

public class BranchesSql
{
    public static string GetAllBranches = @"SELECT 
       b.""BranchId"", 
       b.""City"",
       b.""StreetName"", 
       b.""StreetNumber"",
       b.""BranchStatusID"" AS ""BranchStatus"",
       e.""EmployeeId"",
       e.""FirstName"",
       e.""LastName"",
       e.""Email"",
       e.""EmployeeRoleId"" AS ""EmployeeRole"",
       e.""IsDeleted""
        FROM public.""Branch"" b
        LEFT JOIN public.""BranchEmployees"" be ON b.""BranchId"" = be.""BranchID""
        LEFT JOIN public.""Employee"" e ON e.""EmployeeId"" = be.""EmployeeID""";

    public static string GetBranch = @"SELECT 
       b.""BranchId"", 
       b.""City"",
       b.""StreetName"", 
       b.""StreetNumber"",
       b.""BranchStatusID"" AS ""BranchStatus"",
       e.""EmployeeId"",
       e.""FirstName"",
       e.""LastName"",
       e.""Email"",
       e.""EmployeeRoleId"" AS ""EmployeeRole"",
       e.""IsDeleted""
        FROM public.""Branch"" b
        LEFT JOIN public.""BranchEmployees"" be ON b.""BranchId"" = be.""BranchID""
        LEFT JOIN public.""Employee"" e ON e.""EmployeeId"" = be.""EmployeeID""
        WHERE b.""BranchId"" = @BranchId";

    public static string ChangeStatus = @"UPDATE public.""Branch""
        SET   ""BranchStatusID"" = @branchStatus
        WHERE ""BranchID"" = @branchID";

    public static string RemoveEmployee = @"DELETE 
    FROM public.""BranchEmployees""
    WHERE ""EmployeeID""=@EmployeeID
    AND ""BranchID""=@BranchID;";

    public static string AddEmployee =@"INSERT INTO public.""BranchEmployees""
        (""EmployeeID"",
         ""BranchID"")
    VALUES (@EmployeeID,
            @BranchID);";

    public static string AddBranch = @"INSERT INTO public.""Branch""
        (""City"",
        ""StreetName"",
        ""StreetNumber"",
        ""BranchStatusID"")
        VALUES(@City,
               @StreetName,
               @StreetNumber,
               @BranchStatus);";
}