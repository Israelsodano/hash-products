using System.Collections.Generic;
using System.Linq;
using Hash.Products.Domain.Result;

namespace Hash.Products.Application.Factories
{
    public class ResultFactory
    {

        public static IResult WithSuccess(object value = null) => new Result(value);
        public static IResult WithError(params (string message, string code)[] messagesAndCodes) => 
            new Result(null, messagesAndCodes.Select(x=> (IError)new Error(x.message, x.code)));

        private struct Result : IResult
        {
            public Result(object value, 
                          IEnumerable<IError> errors = null)
            {
                Errors = errors ?? new IError[]{ };
                Value = value;
            }

            public bool IsSuccess => !Errors.Any();
            public IEnumerable<IError> Errors { get; }
            public object Value { get; }
        }

        private struct Error : IError
        {
            public Error(string message, string code)
            {
                Message = message;
                Code = code;
            }

            public string Code { get; set; }
            public string Message { get; set; }
        }
    }
}