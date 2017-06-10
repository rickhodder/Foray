namespace Foray.Common
{
    public class FieldState : SchemaParserState
    {
        public FieldState(IContext<IStringReader, Schema> context) : base(context)
        {
        }

        public override void Handle()
        {
            if (Context.Variables.ContainsKey("lastentity"))
            {
                var table = (Entity)Context.Variables["lastentity"];
                var fieldName = Context.Input.CurrentLine.Trim();
                var field = new EntityField { Name = fieldName };
                table.Fields.Add(field);
                Context.SetState(new FindingState(Context));
            }
            else
            {
                Context.SetState(new ErrorState(Context){Message = $"Field found {Context.Input.CurrentLine.Trim()} but no table"});
            }
        }
    }
}