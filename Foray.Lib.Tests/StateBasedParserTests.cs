using Moq;
using NUnit.Framework;

namespace Foray.Lib.Tests
{

    [TestFixture]
    public class ParserContextTests
    {
        private ParserStateStub _state;
        private Mock<IParserContext> _sut;

        [SetUp]
        public void Setup()
        {
            _state = new ParserStateStub();
            _sut = new Mock<IParserContext>();
            _sut.Setup(m => m.CurrentState).Returns(_state);
        }

        [Test]
        public void SetState_SettingState_SetsState()
        {
            _sut.Object.SetState(_state);
            Assert.That(_sut.Object.CurrentState, Is.EqualTo(_state));
        }
    }

    [TestFixture]
    public class ParserStateTests
    {
        [Test]
        public void Handle_GivenContext_AcceptsContext()
        {
            IParserContext context = new ParserContextStub();
            var sut = new Mock<IParserState>();
            sut.Object.Handle(context);
            // what would I assert here? 
            //- the fact that it compiles I think 
            // is passing and defines the contract if nothing else
        }
    }

    public class ParserContextStub : IParserContext
    {
        public IParserState CurrentState { get; set; }

        public void SetState(IParserState state)
        {
            CurrentState = state;
        }
    }

    public interface IParserState
    {
        void Handle(IParserContext context);
    }

    public class ParserStateStub : IParserState
    {
        public void Handle(IParserContext context)
        {
        }
    }

    public interface IParserContext
    {
        void SetState(IParserState state);
        IParserState CurrentState { get; set; }
    }
}
