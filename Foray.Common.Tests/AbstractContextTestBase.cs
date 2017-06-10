using System;
using NUnit.Framework;

namespace Foray.Common.Tests
{
    [TestFixture]
    public abstract class AbstractContextTestBase : TestBase
    {
        protected Schema Execute(string whatToParse)
        {
            var context = new SchemaContext();

            var mockState = CreateMock<IState<IStringReader, Schema>>();
            mockState.Setup(m => m.Handle()).Callback(() => { context.Finished = true;
                context.Output = new Schema();
            });

            context.SetState(mockState.Object);

            var result = context.Execute();
            
            if (context.ErrorMessages.Count > 0)
            {
                foreach (var message in context.ErrorMessages)
                {
                    Console.WriteLine($"Error: {message}");
                }
            }

            Render(result);
            MockRepository.VerifyAll();

            return result;
        }

        protected void Render(Schema schema)
        {
            if (schema==null)
            {
                Console.WriteLine("Schema is null");
                return;
            }
            Console.WriteLine("Entities:\r\n");
            if (schema.Entities!=null)
            {
                foreach (var entity in schema.Entities)
                {
                    RenderEntity(entity);
                    Console.WriteLine();
                }
            }
           

            Console.WriteLine("Relationships:\r\n");

            if (schema.Relationships!=null)
            {
                foreach (var relationship in schema.Relationships)
                {
                    RenderRelationship(relationship);
                } 
            }
        }

        private void RenderRelationship(Relationship relationship)
        {
            Console.WriteLine("Relationship: " + relationship);
        }

        private void RenderEntity(Entity entity)
        {
            Console.WriteLine("Entity: " + entity);
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
