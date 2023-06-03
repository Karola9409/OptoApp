using System;
using OptoApi.Models;

namespace OptoApi.Validators
{
    public class ProductValidator
    {
        private const int DescriptionMinLenght = 10;
        private static readonly decimal[] ValidVatPercentages = new[] { 23m, 8m, 5m, 0m };
        private readonly string ValidVatPercentagesString = string.Join(",", ValidVatPercentages);

        public ValidationResult IsValid(Product product) 
        {
            if (string.IsNullOrEmpty(product.Name))
            {
                return new ValidationResult(false, "Name can't be empty field.");
            }
            if (string.IsNullOrEmpty(product.Description))
            {
                return new ValidationResult(false, "Description can not be empty field.");
            }
            if (product.Description.Length < DescriptionMinLenght)
            {
                return new ValidationResult(false, $"Description minimal lenght is {DescriptionMinLenght}.");
            }
            if (product.StockCount < 0)
            {
                return new ValidationResult(false, "Product stock count must be greater than 0");
            }
            if (product.GrossPrice <= 0)
            {
                return new ValidationResult(false, "Product gross price must be greater than 0");
            }
            if (ValidVatPercentages.Contains(product.VatPercentage) is false)
            {
                return new ValidationResult(false, $"Vat Percentage value is different than:{ValidVatPercentagesString}");
            }
            if (Uri.TryCreate(product.PhotoUrl, new UriCreationOptions(), out var result) is false)
            {
                return new ValidationResult(false, "Photo URL is not valid");
            };
            return new ValidationResult(true, "");
        }
    }
}

