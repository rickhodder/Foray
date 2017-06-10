using System;
using NUnit.Framework;

namespace Foray.Common.Tests
{
    [TestFixture]
    public class SchemaContextTests : AbstractContextTestBase
    {
        [Test]
        public void Test()
        {
            var result = Execute("Customer\r\n\tFirstName\r\n\tLastName\r\nOrder\r\n\tOrderDate\r\n\tPrice\r\nCustomer>Order\r\nOrder>OrderDetail");
            Render(result);
        }

        [Test]
        public void Execute_WhenInputIsEmpty_ReturnsEmptySchema()
        {            
            var result = Execute("");
            Assert.AreEqual(0, result.Entities.Count);
            Assert.AreEqual(0, result.Relationships.Count);
        }

        [Test]
        public void Execute_WhenInputHasOneEntity_ReturnsSchemaContainingEntity()
        {
            var result = Execute("Customer");
            Assert.IsNotNull(result.FindEntity("Customer"));
        }

        [Test]
        public void Execute_TwoLineEntity_TwoEntity()
        {
            var result = Execute("Customer\r\nOrder");
            Assert.IsNotNull(result.FindEntity("Customer"));
            Assert.IsNotNull(result.FindEntity("Order"));
        }

        [Test]
        public void Execute_OneLineEntityOneLineField_OneEntityWithOneField()
        {
            var result = Execute("Customer\r\n\tFirstName");
            var entity = result.FindEntity("Customer");
            Assert.IsNotNull(entity);
            Assert.IsTrue(entity.Fields.Count==1);
        }

        [Test]
        public void Execute_OneToOneRelationship_TwoEntities()
        {
            var result = Execute("Customer-Address");

            Assert.IsNotNull(result.FindEntity("Customer"));
            Assert.IsNotNull(result.FindEntity("Address"));
        }

        [Test]
        public void Execute_OneToManyRelationship_TwoEntities()
        {
            var result = Execute("Customer->Order");

            Assert.IsNotNull(result.FindEntity("Customer"));
            Assert.IsNotNull(result.FindEntity("Order"));
        }

        [Test]
        public void Execute_ManyToManyRelationship_TwoEntities()
        {
            var result = Execute("Customer<>Order");

            Assert.IsNotNull(result.FindEntity("Customer"));
            Assert.IsNotNull(result.FindEntity("Order"));
        }

        [Test]
        public void Execute_ManyToMany2Relationship_ThreeEntities()
        {
            var result = Execute("Customer<CustomerOrder>Order");

            Assert.IsNotNull(result.FindEntity("Customer"));
            Assert.IsNotNull(result.FindEntity("CustomerOrder"));
            Assert.IsNotNull(result.FindEntity("Order"));
        }

        [Test]
        public void Test2()
        {
            var input
                = new TextBuilder()
                    .Entity("Customer")
                        .Field("FirstName")
                        .Field("MiddleName")
                        .Field("LastName")
                    .Entity("Address")
                        .Field("Addr1")
                        .Field("Addr2")
                        .Field("City")
                        .Field("State")
                        .Field("Zipcode")
                    .Entity("Order")
                        .Field("OrderDate")
                        .Field("ShipDate")
                    .Relationship("Customer-Address")
                    .Relationship("Customer->Order")
                    .Output;    

            var result = Execute(input);
            Render(result);

            Assert.IsNotNull(result.FindEntity("Customer"));
            Assert.AreEqual(3, result.FindEntity("Customer").Fields.Count);

            Assert.IsNotNull(result.FindEntity("Address"));
            Assert.AreEqual(5, result.FindEntity("Address").Fields.Count);

            Assert.IsNotNull(result.FindEntity("Order"));
            Assert.AreEqual(2,result.FindEntity("Order").Fields.Count);

            Assert.AreEqual(2, result.Relationships.Count);
        }

        [Test]
        public void Execute_DuplicateEntityName_ErrorState()
        {
            var result = Execute("product\r\ncustomer\r\nproduct"); 
        }

        public class TextBuilder
        {
            public string Output { get; set; }

            public TextBuilder Entity(string entityName)
            {
                Output += entityName+"\r\n";
                return this;
            }

            public TextBuilder Field(string fieldName)
            {
                Output += $"\t{fieldName}\r\n";
                return this;
            }

            public TextBuilder Relationship(string relationship)
            {
                Output += $"{relationship}\r\n";
                return this;
            }
        }       
    }
}