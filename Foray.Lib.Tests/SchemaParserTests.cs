using Foray.Lib.Model;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Foray.Lib.Tests
{
    [TestFixture]
    public class SchemaParserTests
    {
        SchemaParser _sut;
        MockRepository _mockRepository;
        Mock<ISchemaFactory> _mockSchemaFactory;
        Mock<IParser<Entity>> _mockEntityParser;
        Mock<IParser<Relationship>> _mockRelationshipParser;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSchemaFactory = _mockRepository.Create<ISchemaFactory>();
            _mockEntityParser = _mockRepository.Create<IParser<Entity>>();
            _mockRelationshipParser = _mockRepository.Create<IParser<Relationship>>();
            _sut = new SchemaParser(_mockSchemaFactory.Object, _mockEntityParser.Object, _mockRelationshipParser.Object);

        }

        [Test]
        // Subject Scenario Result
        public void Constructor_Always_PerformsExpectedWork()
        {
            // verify that all setups are fulfilled on mock
            _mockRepository.VerifyAll();
        }

        [Test]

        public void Parse_Always_PerformsExpectedWork()
        {
            var schema = new Schema();
            var input = "[Customer\r\n* Name\r\nAge]";

            var entity = new Entity
            {
                Name = "Customer",
               
            };

            entity.Fields.Add(new EntityField{ Name="Name"});
            entity.Fields.Add(new EntityField { Name = "Age" });

            _mockSchemaFactory.Setup(m => m.Create()).Returns(schema);

            _mockEntityParser.Setup(m => m.Parse(schema, input)).Returns(new List<Entity>{entity});
            _mockRelationshipParser.Setup(m => m.Parse(schema, input)).Returns(new List<Relationship>());
            var result = _sut.Parse(input);

            Assert.That(result.Entities.Count, Is.EqualTo(1));
            Assert.That(result.Entities.First(), Is.SameAs(entity)); // same exact instance
            Assert.That(result.Relationships.Count, Is.EqualTo(0));



            // verify that all setups are fulfilled on mock
            _mockRepository.VerifyAll();
        }
    }
}
