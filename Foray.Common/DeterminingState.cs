namespace Foray.Common
{
    public class DeterminingState : SchemaParserState
    {
        public DeterminingState(IContext<IStringReader,Schema> context ) : base(context)
        {
           
        }
        public override void Handle()
        {
            if (Context.Input.CurrentLine.StartsWith("\t"))
            {
                Context.SetState(new FieldState(Context));
                return;
            }
            if (Context.Input.CurrentLine.IndexOfAny(new [] {'<','>','-'})>=0)
            {
                Context.SetState(new RelationshipState(Context));
                return;
            }

            Context.SetState(new EntityState(Context));
        }
    }
}