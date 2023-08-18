namespace OptoApi.Sql;

public class ProductSql
{
   public static string GetAllProducts = @"SELECT
                 ""ID"",
                 ""Name"",
                 ""Description"",
                 ""StockCount"",
                 ""GrossPrice"",
                 ""VatPercentage"",
                 ""PhotoUrl""
            FROM public.""Products"";";
   
   public static string GetProduct = @"SELECT
                 ""ID"",
                 ""Name"",
                 ""Description"",
                 ""StockCount"",
                 ""GrossPrice"",
                 ""VatPercentage"",
                 ""PhotoUrl""
            FROM public.""Products""
            WHERE ""ID""=@ID;";
   
   public static string AddProduct = @"INSERT INTO public.""Products""
            (""Name"",
             ""Description"",
             ""StockCount"",
             ""GrossPrice"",
             ""VatPercentage"",
             ""PhotoUrl"")
            VALUES (@Name,
                    @Description,
                    @StockCount,
                    @GrossPrice,
                    @VatPercentage,
                    @PhotoUrl);";
   
   public static string UpdateProduct = @"UPDATE public.""Products"" SET 
        ""Name""= @Name, 
        ""Description""= @Description, 
        ""StockCount""= @StockCount, 
        ""GrossPrice""= @GrossPrice, 
        ""VatPercentage""= @VatPercentage, 
        ""PhotoUrl""= @PhotoUrl
        WHERE ""ID""=@Id ;";
   
   public static string RemoveProduct = @"
            DELETE 
            FROM public.""Products""
            WHERE ""ID""=@ID;";

   public static string ProductExists = @"SELECT EXISTS(
        SELECT 1
        FROM public.""Products""
        WHERE ""Name""= @Name)
        ";
}
