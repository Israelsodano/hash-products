using System.Threading.Tasks;
using Hash.Products.Domain.Commands;
using Hash.Products.Domain.Result;

namespace Hash.Products.Application.Processors
{
    public interface IBuyProductsProcessor
    {
        Task<IResult> ProcessAsync(BuyProductsCommand command);
    }
}