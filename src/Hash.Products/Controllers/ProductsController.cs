using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Hash.Products.Domain.Commands;
using Hash.Products.Domain.Models;
using Hash.Products.Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hash.Products.Controllers
{
    public class ProductsController : BaseController
    {
        public ProductsController(IMediator mediator) : base(mediator)
        { }

        [HttpPost]
        [ProducesResponseType(typeof(PurchaseSummary), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<IError>), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> BuyProductsAsync(BuyProductsCommand command) => 
            ExecuteCommand(command); 
    }
}