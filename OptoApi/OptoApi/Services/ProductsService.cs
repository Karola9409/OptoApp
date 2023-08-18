using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using OptoApi.Exceptions;
using OptoApi.Models;
using OptoApi.Options;
using OptoApi.Sql;

namespace OptoApi.Services;

public class ProductsService : IProductsService
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;

    public ProductsService(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public List<Product> GetAllProducts()
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);
        
        var result = connection.Query<Product>(ProductSql.GetAllProducts);

        return result.AsList();
    }

    public Product? GetProduct(int id)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result = connection.QueryFirstOrDefault<Product>(ProductSql.GetProduct, new
        {
            ID = id
        });

        return result;
    }
    
    public int AddProduct(Product product)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);
        
        var result = connection.Execute(ProductSql.AddProduct, new
        {
            product.Name,
            product.Description,
            product.StockCount,
            product.GrossPrice,
            product.VatPercentage,
            product.PhotoUrl
        });
        return result;
    }
    public void UpdateProduct(Product product)
    {
        try
        {
            using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);
            
            connection.Execute(ProductSql.UpdateProduct, new
            {
                product.Id,
                product.Name,
                product.Description,
                product.StockCount,
                product.GrossPrice,
                product.VatPercentage,
                product.PhotoUrl
            });
        }
        catch(PostgresException pex)
        {
            if (pex.SqlState == "23505")
            {
                throw new ProductNameDuplicateException();
            }
        }
    }

    public bool RemoveProduct(int productId)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);
        
        connection.Execute(ProductSql.RemoveProduct, new
        {
            ID = productId
        });
        return true;
    }
    public bool Exists(string productName)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result = connection.ExecuteScalar<bool>(ProductSql.ProductExists, new
        {
            Name = productName
        });
        return result;
    }
}