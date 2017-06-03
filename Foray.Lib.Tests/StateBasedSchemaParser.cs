using System;
using System.IO;
using Foray.Lib.Model;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace Foray.Lib.Tests
{
    [TestFixture]
    public class Tests
    {
        private StateBasedSchemaParser _sut;

        [SetUp]
        public void Setup()
        {
            string dsl = @"
Customer\r\n
\tFirstName\r\n
\tLastName\r\n
";

            _sut = new StateBasedSchemaParser(dsl,new SchemaFactory());
            _sut.SetState(new SchemaStartState());
            var schema = _sut.Execute();
        }

        [Test]
        public void Test()
        {
            
        }
    }

    public interface IFactory<out TModel>
    {
        TModel Create();
    }

    public class SchemaFactory : ISchemaFactory
    {
        public Schema Create()
        {
            return new Schema();
        }
    }

    public class StateBasedSchemaParser : AbstractStateMachineContext<Schema, IState<Schema>>
    {
        private readonly ISchemaFactory _factory;
        public StreamReader Reader { get; private set; }

        public StateBasedSchemaParser(string dsl, ISchemaFactory factory)
        {       
            _factory = factory;
        }

        private void CreateReader(string dsl)
        {
            var ms = new MemoryStream();
            using (var sw = new StreamWriter(ms))
            {
                sw.WriteLine(dsl);
                ms.Position = 0;
                Reader = new StreamReader(ms);
            }
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

    public class FindState : ISchemaParserState
    {
        public void Handle(IStateMachineContext<Schema, IState<Schema>> context)
        {
            
        }

        public bool IsStopState { get; }
    }

}
