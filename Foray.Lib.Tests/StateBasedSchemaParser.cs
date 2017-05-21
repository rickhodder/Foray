using System;

namespace Foray.Lib.Tests
{
    public interface IFactory<out TModel>
    {
        TModel Create();
    }

    public class StateBasedSchemaParser : AbstractStateMachineContext<Schema, IState<Schema>>
    {
        private readonly ISchemaFactory _factory;

        public StateBasedSchemaParser(ISchemaFactory factory)
        {
            _factory = factory;
        }

        protected override Schema CreateOutput()
        {
            return _factory.Create();
        }
    }

    public interface ISchemaParserState : IState<Schema>
    {
        
    }

    public class SchemaStartState: ISchemaParserState
    {
        public void Handle(IStateMachineContext<Schema, IState<Schema>> context)
        {
            throw new NotImplementedException();
        }

        public bool IsStopState { get; }
    }
}
