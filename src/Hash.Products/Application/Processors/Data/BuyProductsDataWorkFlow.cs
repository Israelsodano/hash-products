using System;
using System.Collections.Generic;
using Hash.Products.Domain.Entities;
using Hash.Products.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Hash.Products.Application.Processors.Data
{
    public class BuyProductsDataWorkFlow
    {
        public BuyProductsDataWorkFlow(IConfiguration configuration)
        {
            if(configuration is null) throw new ArgumentNullException(nameof(configuration));
            BlackFridayDate = configuration.GetValue<DateTime?>(nameof(BlackFridayDate));
        }

        public DateTime? BlackFridayDate { get; set; }    
        public IEnumerable<PurchasedProduct> PurchasedProducts { get; set; }
        public string StringJsonProductsCommand { get; set; }
    }
}