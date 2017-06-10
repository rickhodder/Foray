namespace Foray.Common
{
    public class FindingState : SchemaParserState
    {
        public FindingState(IContext<IStringReader, Schema> context) : base(context)
        {
        }

        public override void Handle()
        {
            while (!Context.Input.IsDone())
            {
                if (!string.IsNullOrEmpty(Context.Input.GetLine()))
                {
                    Context.SetState(new DeterminingState(Context));
                    return;
                };
            }

            Context.Finished = true;
        }
    }
}