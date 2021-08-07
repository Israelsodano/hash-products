using AutoFixture;
using AutoFixture.Xunit2;
using Discount;
using Hash.Products.Application.Extensions;
using Hash.Products.Application.Processors.Data;
using Hash.Products.Domain.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Threading.Tasks;
using static Discount.Discount;

namespace Hash.Products.IntegratedTests
{
    public class AutoDataSubstitute : AutoDataAttribute
    {
        public AutoDataSubstitute() : base(GetFixture)
        {

        }

        public static IFixture GetFixture()
        {
            var fixture = new Fixture();
            fixture.Register(() => new BuyProductsCommand
            {
                Products = new ProductCommand[]
                {
                    new ProductCommand
                    {
                        Id = 1,
                        Quantity = 1
                    },
                    new ProductCommand
                    {
                        Id = 2,
                        Quantity = 1
                    },
                    new ProductCommand
                    {
                        Id = 3,
                        Quantity = 1
                    },
                    new ProductCommand
                    {
                        Id = 4,
                        Quantity = 1
                    }
                }
            });

            return fixture;
        }
    }
}
