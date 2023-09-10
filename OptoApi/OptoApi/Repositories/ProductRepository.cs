using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using OptoApi.Exceptions;
using OptoApi.Models;
using OptoApi.Options;
using OptoApi.Sql;

namespace OptoApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;

    public ProductRepository(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }
    public async Task<List<Product>> GetAllProducts()
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);
        
        var result = await connection.QueryAsync<Product>(ProductSql.GetAllProducts);

        return result.AsList();
    }
    public async Task<Product?> GetProduct(int id)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result = await connection.QueryFirstOrDefaultAsync<Product>(ProductSql.GetProduct, new
        {
            ID = id
        });
        return result;
    }
    public async Task<int>AddProduct(Product product)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);
        
        var result = await connection.ExecuteAsync(ProductSql.AddProduct, new
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
    public async Task UpdateProduct(Product product)
    {
        try
        {
            using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);
            
            await connection.ExecuteAsync(ProductSql.UpdateProduct, new
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
    public async Task<bool> RemoveProduct(int productId)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);
        
        await connection.ExecuteAsync(ProductSql.RemoveProduct, new
        {
            ID = productId
        });
        return true;
    }
    public async Task<bool> Exists(string productName)
    {
        using var connection = new NpgsqlConnection(_databaseOptions.Value.ConnectionString);

        var result = await connection.ExecuteScalarAsync<bool>(ProductSql.ProductExists, new
        {
            Name = productName
        });
        return result;
    }
}