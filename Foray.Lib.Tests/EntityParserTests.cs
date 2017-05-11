using System.Linq;
using Castle.Core.Internal;
using Foray.Lib.Model;
using NUnit.Framework;

namespace Foray.Lib.Tests
{
    [TestFixture]
    public class EntityParserTests
    {
        readonly EntityParser _sut = new EntityParser();

        [Test]
        public void Parse_Always_PerformsExpectedWork()
        {
            var input = "[Customer\r\nName\r\nAge]";
            var schema = new Schema();
            var result = _sut.Parse(schema, input);
            var expected = new Entity
            {
                Name = "Customer",

            };

            expected.Fields.Add(new EntityField { Name = "Name" });
            expected.Fields.Add(new EntityField { Name = "Age" });
            var firstResult = result.First();


            Assert.That(firstResult.Name, Is.EqualTo(expected.Name));
            Assert.That(firstResult.Fields.Count,Is.EqualTo(expected.Fields.Count));

        }

        [Test]
        public void Parse_WhenMultipleEntitiesSpecified_ReturnsExpectedResult()
        {
            var input = "[Customer\r\nName\r\nAge][Customer2\r\nName\r\nAge]";
            var schema = new Schema();
            var result = _sut.Parse(schema, input);
            var expected = new Entity
            {
                Name = "Customer",

            };

            expected.Fields.Add(new EntityField { Name = "Name" });
            expected.Fields.Add(new EntityField { Name = "Age" });
           // var firstResult = result.First();


            //Assert.That(firstResult.Name, Is.EqualTo(expected.Name));
            //Assert.That(firstResult.Fields.Count, Is.EqualTo(expected.Fields.Count));

            Assert.That(result.Count,Is.EqualTo(2));
        }
    }

   
}