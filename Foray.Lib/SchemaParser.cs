using Foray.Lib.Model;

namespace Foray.Lib
{
    public class SchemaParser
    {
        private readonly IParser<Entity> _entityParser;
        private readonly ISchemaFactory _schemaFactory;

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
