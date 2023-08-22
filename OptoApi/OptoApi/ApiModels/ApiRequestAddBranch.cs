using OptoApi.Models;
namespace OptoApi.ApiModels;

public class ApiRequestAddBranch
{
    public ApiRequestAddBranch(string city, string streetName, string streetNumber)
    {
        City = city;
        StreetName = streetName;
        StreetNumber = streetNumber;
    }
    public string StreetNumber { get; set; }

    public string StreetName { get; set; }

    public string City { get; set; }
    public List<Employee> Employee { get; set; }
}