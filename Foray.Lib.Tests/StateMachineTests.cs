using System;
using System.IO;
using System.Text;
using Castle.DynamicProxy.Tokens;
using Moq;
using NUnit.Framework;

namespace Foray.Lib.Tests
{
    [TestFixture]
    public class StateMachineTests
    {
        private Mock<IStateMachineContext<string, IState<string>>> _sut;
        private Mock<IState<string>> _state;
        private Mock<IState<string>> _stopState;

        [SetUp]
        public void Setup()
        {
            _state = new Mock<IState<string>>();
            _stopState = new Mock<IState<string>>();
            _stopState.Setup(m=>m.IsStopState).Returns(true);
            _sut = new Mock<IStateMachineContext<string, IState<string>>>();
        }

        [Test]
        public void SetState_SetAState_SetsState()
        {
            var expected = _state.Object;
            _sut.Setup(m => m.CurrentState).Returns(_state.Object);
            _sut.Object.SetState(expected);
            Assert.That(_sut.Object.CurrentState,Is.EqualTo(expected));
        }

        [Test]
        public void Execute_StopState_StopsExecuting()
        {
            _sut.Object.SetState(_stopState.Object);
            _sut.Object.Execute();
            _stopState.Verify(v=>v.Handle(It.IsAny<IStateMachineContext<string, IState<string>>>()), Times.Never());
        }
    }

    interface IContext<TInput>
    {
        
    }

    public class x
    {
        public void Test()
        {
            string _dsl = "";
            var ms = new MemoryStream();

            ms.Write(Encoding.Default.GetBytes(_dsl.ToCharArray(), 0, _dsl.Length), 0, _dsl.Length);
            ms.Position = 0;
        }
    }


    public abstract class AbstractStateMachineContext<TOutput, TState> : IDisposable, IStateMachineContext<TOutput, TState> where TState : IState<TOutput>
    {
        
        public TOutput Output { get; private set; }
        public TState CurrentState { get; private set; }

        public virtual void SetState(TState state)
        {
            CurrentState = state;
        }
        
        protected abstract TOutput CreateOutput();

        public virtual TOutput Execute()
        {
            Output = CreateOutput();
            while (!CurrentState.IsStopState)
            {
                CurrentState.Handle((IStateMachineContext<TOutput, IState<TOutput>>) this);
            }

            return Output;
        }

        public void Dispose()
        {
            CurrentState = default(TState);
            Output = default(TOutput);
        }
    }

    public interface IStateMachineContext<out TOutput, TState> where TState: IState<TOutput>
    {
        TState CurrentState { get; }
        void SetState(TState state);
        TOutput Execute();
    }

    public interface IState
    {
        bool IsStopState { get; }
    }

    public interface IState<TOutput> : IState
    {
        void Handle(IStateMachineContext<TOutput, IState<TOutput>> context);
    }


    public interface IStringReader : IDisposable
    {
        string GetLine();
        bool IsDone();
    }

    public class StringReader : IStringReader
    {
        StreamReader _reader;

        public StringReader(string content)
        {
            var ms = new MemoryStream();

            ms.Write(Encoding.Default.GetBytes(content.ToCharArray(), 0, content.Length), 0, content.Length);
            ms.Position = 0;
            _reader = new StreamReader(ms);
        }

        public string GetLine()
        {
            return _reader.ReadLine();
        }

        public bool IsDone()
        {
            return _reader.EndOfStream;
        }

        public void Dispose()
        {
            _reader?.Close();
            _reader = null;
        }
    }

}