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
