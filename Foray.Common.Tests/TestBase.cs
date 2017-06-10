using Moq;
using NUnit.Framework;

namespace Foray.Common.Tests
{
    public class TestBase
    {
        protected MockRepository MockRepository;

        protected Mock<T> CreateMock<T>() where T:class
        {
            return MockRepository.Create<T>();
        }

        [SetUp]
        public void Setup()
        {
            MockRepository=new MockRepository(MockBehavior.Strict);
        }
    }
}