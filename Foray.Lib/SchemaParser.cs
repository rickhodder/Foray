using Foray.Lib.Model;

namespace Foray.Lib
{
    public class SchemaParser
    {
        private readonly IParser<Entity> _entityParser;
        private readonly ISchemaFactory _schemaFactory;
        private readonly IParser<Relationship> _relationshipParser;

        public SchemaParser(ISchemaFactory schemaFactory, IParser<Entity> entityParser, IParser<Relationship> relationshipParser)
        {
            _schemaFactory = schemaFactory;
            _entityParser = entityParser;
            _relationshipParser = relationshipParser;
        }

        public void Parse()
        {
            var schema = _schemaFactory.Create();
            var entities = _entityParser.Parse(schema);
            var relationships = _relationshipParser.Parse(schema);
        }
    }
}
