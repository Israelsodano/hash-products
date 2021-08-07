using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Hash.Products.Domain.Commands;

namespace Hash.Products.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;
        public BaseController(IMediator mediator) => 
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));  

        public async Task<IActionResult> ExecuteCommand<TCommand>(TCommand command)
            where TCommand : BaseCommand
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess ?
                    result.Value is null ? 
                        NoContent() : 
                        new ObjectResult(result.Value) { StatusCode = (int)command.DefaultSuccesResponse }
                    : BadRequest(result.Errors);
        }
    }
}