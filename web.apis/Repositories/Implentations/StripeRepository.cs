using System;
namespace web.apis
{
    public class StripeRepository : IStripeRepository
    {
        private readonly ILogger _logger;

        public StripeRepository(ILogger logger)
        {
            _logger = logger;
        }

        // public async Task<Product> AddProduct(string name, string type, string _stripeConfigurationKey)
        // {
        //     try
        //     {
        //         StripeConfiguration.ApiKey = _stripeConfigurationKey;
        // 
        //         var productService = new ProductService();
        //         var productOptions = new ProductCreateOptions
        //         {
        //             Name = name,
        //             Type = type,  // You can use "good" for physical goods
        //         };
        // 
        //         return await productService.CreateAsync(productOptions);
        //     }
        //     catch (Exception ex)
        //     {
        //         var extraInfo = $"An error occurred while adding a Product";
        //         _logger.Error(extraInfo, ex.Message);
        //         throw new Exception(extraInfo);
        //     }
        // }
        // 
        // public async Task<Price> AddProductPrice(string productId, long amount, string currency, string _stripeConfigurationKey, PriceRecurringOptions priceRecurringOptions)
        // {
        //     try
        //     {
        //         StripeConfiguration.ApiKey = _stripeConfigurationKey;
        // 
        //         var priceOption = new PriceCreateOptions
        //         {
        //             UnitAmount = amount,  // Price amount in cents
        //             Currency = currency,
        //             Product = productId
        //         };
        // 
        //         if (priceRecurringOptions != null)
        //             priceOption.Recurring = priceRecurringOptions;
        // 
        //         var priceService = new PriceService();
        // 
        //         return await priceService.CreateAsync(priceOption);
        //     }
        //     catch (Exception ex)
        //     {
        //         var extraInfo = $"An error occurred while adding a Product Price";
        //         _logger.Error(extraInfo, ex.Message);
        //         throw new Exception(extraInfo);
        //     }
        // }
        // 
        // public IEnumerable<Product> GetAllProducts(bool isActive, string _stripeConfigurationKey, List<string> ids = null)
        // {
        //     try
        //     {
        //         StripeConfiguration.ApiKey = _stripeConfigurationKey;
        // 
        //         ProductListOptions options;
        // 
        //         if (ids != null)
        //             options = new ProductListOptions { Active = true, Ids = ids };
        //         else
        //             options = new ProductListOptions { Active = true };
        // 
        //         var service = new ProductService();
        //         return service.List(options).AsEnumerable();
        //     }
        //     catch (Exception ex)
        //     {
        //         var extraInfo = $"An error occurred while fetching Product list";
        //         _logger.Error(extraInfo, ex.Message);
        //         throw new Exception(extraInfo);
        //     }
        // }
        // 
        // public async Task<Product> UpdateProduct(string productId, string name, string description, string _stripeConfigurationKey, List<string> images = null)
        // {
        //     try
        //     {
        //         StripeConfiguration.ApiKey = _stripeConfigurationKey;
        // 
        //         var productService = new ProductService();
        //         var productOptions = new ProductUpdateOptions
        //         {
        //             Name = "Updated Product Name",
        //             Description = "Updated product description"
        //         };
        // 
        //         if (images != null)
        //             productOptions.Images = images;
        // 
        //         return await productService.UpdateAsync(productId, productOptions);
        //     }
        //     catch (Exception ex)
        //     {
        //         var extraInfo = $"An error occurred while updating a Product";
        //         _logger.Error(extraInfo, ex.Message);
        //         throw new Exception(extraInfo);
        //     }
        // }
        // 
        // public async Task<Price> UpdateProductPrice(string productId, string priceId, int newAmountInCents, string _stripeConfigurationKey)
        // {
        //     try
        //     {
        //         StripeConfiguration.ApiKey = _stripeConfigurationKey;
        // 
        //         var priceService = new PriceService();
        // 
        //         var priceUpdateOptions = new PriceUpdateOptions
        //         {
        //             // UnitAmount = 1500
        //         };
        // 
        //         return await priceService.UpdateAsync(productId, priceUpdateOptions);
        //     }
        //     catch (Exception ex)
        //     {
        //         var extraInfo = $"An error occurred while updating a Product price";
        //         _logger.Error(extraInfo, ex.Message);
        //         throw new Exception(extraInfo);
        //     }
        // }
    }
}
