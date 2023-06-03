using System;
using System.Xml.Linq;
using OptoApi.Models;

namespace OptoApi.Services
{
    public class ProductsService
    {
        private List<Product> ProductsList = new List<Product>
        {
            new Product(1,"Frame","Corrective frame", 1, 249.99m, 8,"https://example.com/photo" ),
            new Product(2,"Frame","Sunglasses", 1, 149.99m, 23,"https://example.com/photo" ),
            new Product(3,"Contact Lens","1-Day Contact Lens", 2, 49.99m, 8,"https://example.com/photo" ),
            new Product(4,"Eye drops","Eye drops without preservative", 10, 59.99m, 8,"https://example.com/photo" )
        };

        public List<Product> GetAllProducts()
        {
            return ProductsList;
        }

        public Product? GetProduct(int id)
        {
            var result = ProductsList.Find(x => x.Id == id);
            return result;
        }
    }
}
