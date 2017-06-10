namespace Foray.Common
{
    public abstract class SchemaParserState : IState<IStringReader, Schema>
    {
        protected IContext<IStringReader, Schema> Context;

        protected SchemaParserState(IContext<IStringReader, Schema> context)
        {
            Context = context;
        }

        public abstract void Handle();
    }
}