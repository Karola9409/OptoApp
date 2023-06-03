using System;
namespace OptoApi.Models
{
    public class Product
    {
        public Product(int id, string name, string description, int stockCount, decimal grossPrice, decimal vatPercentage, string photoUrl)
        {
            Id = id;
            Name = name;
            Description = description;
            StockCount = stockCount;
            GrossPrice = grossPrice;
            VatPercentage = vatPercentage;
            PhotoUrl = photoUrl;
        }
        public int Id { get; set; }

        public string Name { get; }

        public string Description { get; }

        public int StockCount { get; }

        public decimal GrossPrice { get; }

        public decimal VatPercentage { get; }

        public string PhotoUrl { get; }

    }
}

