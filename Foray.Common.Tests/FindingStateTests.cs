using System.Runtime.Remoting;
using Moq;
using NUnit.Framework;

namespace Foray.Common.Tests
{

    [TestFixture]
    public class FindingStateTests : TestBase
    {
        [Test]
        public void Handle_InputEmpty_MarksContextFinished()
        {
            var context = CreateMock<IContext<IStringReader, Schema>>();
            var sut = new FindingState(context.Object);
            var mockStringReader = CreateMock<IStringReader>();
            mockStringReader.Setup(p => p.IsDone()).Returns(true);
            context.SetupGet(p => p.Input).Returns(mockStringReader.Object);
            context.SetupSet(p=>p.Finished=true);
     
            sut.Handle();

            MockRepository.VerifyAll();
        }
        

    }
}
