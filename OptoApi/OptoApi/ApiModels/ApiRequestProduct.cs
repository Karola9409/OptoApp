using System;
namespace OptoApi.ApiModels
{
    public class ApiRequestProduct 
    {
        public ApiRequestProduct( string name, string description, int stockCount, decimal grossPrice, decimal vatPercentage, string photoUrl)
        {
            Name = name;
            Description = description;
            StockCount = stockCount;
            GrossPrice = grossPrice;
            VatPercentage = vatPercentage;
            PhotoUrl = photoUrl;
        }

        public string Name { get; }

        public string Description { get; }

        public int StockCount { get; }

        public decimal GrossPrice { get; }

        public decimal VatPercentage { get; }

        public string PhotoUrl { get; }

    }
}

