using System.Collections.Generic;

namespace Hash.Products.Domain.Result
{
    public interface IResult
    {
        object Value { get; }
        bool IsSuccess { get; }
        IEnumerable<IError> Errors { get; } 
    }

    public interface IError 
    {
        string Code { get; set; }
        string Message { get; set; }
    }
}