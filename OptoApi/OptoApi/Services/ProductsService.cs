using System;
using System.Xml.Linq;
using OptoApi.Exceptions;
using OptoApi.Models;

namespace OptoApi.Services
{
    public class ProductsService
    {
        private static List<Product> ProductsList = new List<Product>
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

        public bool Exists(string name)
        {
            var nameTrimmed = name.ToLower().Trim();
            var result = ProductsList.Any(x => x.Name.ToLower() == nameTrimmed);
            return result;
        }

        public int AddProduct(Product product)
        {
            var productId = ProductsList.Max(x => x.Id) + 1;
            product.Id = productId;
            ProductsList.Add(product);
            return productId;
        }

        public void UpdateProduct(Product product)
        {
            var productToUpdate = ProductsList.Find(x => x.Id == product.Id);

            if (product.Name == productToUpdate?.Name || !Exists(product.Name))
            {
                ProductsList.Remove(productToUpdate!);
                ProductsList.Add(product);
                return;
            }
            throw new ProductNameDuplicateException();
        }

        public bool RemoveProduct(int productId)
        {
            var productToRemove = ProductsList.Find(x => x.Id == productId);

            if (productToRemove != null)
            {
                ProductsList.Remove(productToRemove);
                return true;
            }
            return false;
        }
    }
}
