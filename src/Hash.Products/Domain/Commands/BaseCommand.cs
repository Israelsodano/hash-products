using System.Net;
using Hash.Products.Domain.Result;
using MediatR;

namespace Hash.Products.Domain.Commands
{
    public abstract class BaseCommand : IRequest<IResult>
    {
        public abstract HttpStatusCode DefaultSuccesResponse { get; }
    }
}