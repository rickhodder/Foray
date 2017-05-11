using Foray.Lib.Model;
using System.Collections.Generic;

namespace Foray.Lib
{
    public interface IParser<TModel>
    {
        List<TModel> Parse(Schema schema, string input);

    }
}
