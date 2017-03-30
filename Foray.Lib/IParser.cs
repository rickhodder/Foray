using Foray.Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foray.Lib
{
    public interface IParser<TModel>
    {
        List<TModel> Parse(Schema schema);
    }
}
