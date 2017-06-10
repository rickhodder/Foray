using System.Collections.Generic;

namespace Foray.Common
{
    public interface IResult
    {
        bool Success { get; set; }
        List<string> ErrorMessages { get; set; }
    }

    public interface IResult<TOutput> : IResult
    {
        TOutput Output { get; set; }
    }
}