using System;
using System.Runtime.Remoting;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Foray.Common.Tests
{

    [TestFixture]
    public class FindingStateTests
    {
        
        [Test]
        public void Handle_InputEmpty_MarksContextFinished()
        {
            var sut = new FindingState();
            var context = new Mock<IContext<IStringReader, Schema>>();
            context.Setup(m => m.Input.IsDone()).Returns(true);
     
            sut.Handle(context.Object);
            Assert.IsTrue(context.Object.Finished);
        }
        
    }

    [TestFixture]
    public class SchemaParserTests
    {
       

        [Test]
        public void Test()
        {
            var result = RunParserFor("Customer\r\n\tFirstName\r\n\tLastName\r\nOrder\r\n\tOrderDate\r\n\tPrice\r\nCustomer>Order\r\nOrder>OrderDetail");
            Render(result);
        }

        [Test]
        public void Execute_EmptyString_EmptySchema()
        {            
            var result = RunParserFor("");
            Assert.AreEqual(0, result.Entities.Count);
            Assert.AreEqual(0, result.Relationships.Count);
        }

        [Test]
        public void Execute_OneLineEntity_OneEntity()
        {
            var result = RunParserFor("Customer");
            Assert.IsNotNull(result.FindEntity("Customer"));
        }

        [Test]
        public void Execute_TwoLineEntity_TwoEntity()
        {
            var result = RunParserFor("Customer\r\nOrder");
            Assert.IsNotNull(result.FindEntity("Customer"));
            Assert.IsNotNull(result.FindEntity("Order"));
        }

        [Test]
        public void Execute_OneLineEntityOneLineField_OneEntityWithOneField()
        {
            var result = RunParserFor("Customer\r\n\tFirstName");
            var entity = result.FindEntity("Customer");
            Assert.IsNotNull(entity);
            Assert.IsTrue(entity.Fields.Count==1);
        }

        [Test]
        public void Execute_OneToOneRelationship_TwoEntities()
        {
            var result = RunParserFor("Customer-Address");

            Assert.IsNotNull(result.FindEntity("Customer"));
            Assert.IsNotNull(result.FindEntity("Address"));
        }

        [Test]
        public void Execute_OneToManyRelationship_TwoEntities()
        {
            var result = RunParserFor("Customer->Order");

            Assert.IsNotNull(result.FindEntity("Customer"));
            Assert.IsNotNull(result.FindEntity("Order"));
        }

        [Test]
        public void Execute_ManyToManyRelationship_TwoEntities()
        {
            var result = RunParserFor("Customer<>Order");

            Assert.IsNotNull(result.FindEntity("Customer"));
            Assert.IsNotNull(result.FindEntity("Order"));
        }

        [Test]
        public void Execute_ManyToMany2Relationship_ThreeEntities()
        {
            var result = RunParserFor("Customer<CustomerOrder>Order");

            Assert.IsNotNull(result.FindEntity("Customer"));
            Assert.IsNotNull(result.FindEntity("CustomerOrder"));
            Assert.IsNotNull(result.FindEntity("Order"));
        }

        private Schema RunParserFor(string whatToParse)
        {
            var state = new SchemaStartState(whatToParse);
            var context = new SchemaContext();
            context.SetState(state);
            var result = context.Execute();
            if (context.ErrorMessages.Count>0)
            {
                foreach (var message in context.ErrorMessages)
                {
                    Console.WriteLine($"Error: {message}");
                }
            }
            Render(result);
            return result;

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

            var result = RunParserFor(input);
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
            var result = RunParserFor("product\r\ncustomer\r\nproduct");
              
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

        private void Render(Schema schema)
        {
            Console.WriteLine("Entities:\r\n");

            foreach (var entity in schema.Entities)
            {
                RenderEntity(entity);
                Console.WriteLine();
            }

            Console.WriteLine("Relationships:\r\n");

            foreach (var relationship in schema.Relationships)
            {
                RenderRelationship(relationship);
            }
        }

        private void RenderRelationship(Relationship relationship)
        {
            Console.WriteLine("Relationship: "+relationship);
        }

        private void RenderEntity(Entity entity)
        {
            Console.WriteLine("Entity: "+entity);
            Console.WriteLine("Fields:");
            foreach (var field in entity.Fields)
            {
                RenderField(field);
            }
        }

        private void RenderField(EntityField field)
        {
            Console.WriteLine("    Field: " + field);
        }
    }
}
