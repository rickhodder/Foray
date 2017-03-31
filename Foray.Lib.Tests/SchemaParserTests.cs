﻿using Foray.Lib.Model;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Foray.Lib.Tests
{
    [TestFixture]
    public class SchemaParserTests
    {
        SchemaParser _sut ;
        MockRepository _mockRepository;
        Mock<ISchemaFactory> _mockSchemaFactory;
        Mock<IParser<Entity>> _mockEntityParser;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockSchemaFactory = _mockRepository.Create<ISchemaFactory>();
            _mockEntityParser = _mockRepository.Create<IParser<Entity>>();
            
            _sut = new SchemaParser(_mockSchemaFactory.Object, _mockEntityParser.Object);

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
            _mockSchemaFactory.Setup(m => m.Create()).Returns(schema);

            _mockEntityParser.Setup(m => m.Parse(schema)).Returns(new List<Entity>());
            _sut.Parse();

            // verify that all setups are fulfilled on mock
            _mockRepository.VerifyAll();
        }

        
    }
}
