using Foray.Lib.Model;

namespace Foray.Lib
{
    public class Schema : ISchemaFactory
    {
        public Schema Create()
        {
            return new Model.Schema();
        }
    }
}
