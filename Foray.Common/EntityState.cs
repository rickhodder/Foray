namespace Foray.Common
{
    public class EntityState : SchemaParserState
    {
        public EntityState(IContext<IStringReader, Schema> context) : base(context)
        {
        }

        public override void Handle()
        {
            var entityName = Context.Input.CurrentLine.Trim();
            var entity = Context.Output.FindEntity(entityName);

            if (entity != null)
            {
                Context.SetState(new ErrorState(Context){Message = $"Entity {entityName} is has already been created."});
                Context.Finished = true;
                return;
            }

            entity = new Entity { Name = entityName };
            Context.Output.Entities.Add(entity);
            Context.Variables["lastentity"] = entity;
            Context.SetState(new FindingState(Context));
        }
    }
}