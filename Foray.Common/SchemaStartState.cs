namespace Foray.Common
{
    public class SchemaStartState : SchemaParserState
    {
        public string Text { get; set; } = "";

        public SchemaStartState(IContext<IStringReader, Schema> context) : base(context)
        {
        }

        public override void Handle()
        {
            Context.Input = new StringReader(Text);
            Context.Output = new Schema();
            Context.SetState(new FindingState(Context));
        }
    }
}