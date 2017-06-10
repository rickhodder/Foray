namespace Foray.Common
{
    public class ErrorState : SchemaParserState
    {
        public ErrorState(IContext<IStringReader, Schema> context) : base(context)
        {
        }

        public string Message { get; set; }

        
        public override void Handle()
        {
            Context.ErrorMessages.Add($"Line {Context.Input.LineNumber}: {Context.Input.CurrentLine}\r\n{Message}");
            Context.Finished = true;
        }
    }
}