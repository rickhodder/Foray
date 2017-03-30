using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foray.Lib.Model;

namespace Foray.Lib
{
    public class SchemaFactory : ISchemaFactory
    {
        public Schema Create()
        {
            return new Schema();
        }
    }
}
