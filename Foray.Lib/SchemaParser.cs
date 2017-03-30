using Foray.Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foray.Lib
{
    public class SchemaParser
    {
        private IParser<Entity> _entityParser;
        private ISchemaFactory _schemaFactory;

        public SchemaParser(ISchemaFactory schemaFactory, IParser<Entity> entityParser)
        {
            _schemaFactory = schemaFactory;
            _entityParser = entityParser;
        }

        public void Parse()
        {
            var schema = _schemaFactory.Create();
            var entities = _entityParser.Parse(schema);
        }
    }
}
