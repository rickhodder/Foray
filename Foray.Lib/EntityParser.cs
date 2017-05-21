using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Foray.Lib.Model;

namespace Foray.Lib
{
    public class EntityParser: IParser<Entity>
    {
        public List<Entity> Parse(Model.Schema schema, string input)
        {
            //var input = "[Customer\r\n* Name\r\nAge]";
            var result = new List<Entity>();
            var splitEntities = input.Split(new[] {'[',']'},StringSplitOptions.RemoveEmptyEntries);
            foreach (var splitEntity in splitEntities)
            {
                var entity = CreateEntity(splitEntity);
                result.Add(entity);
            }
            
       

            return result;
        }

        private static Entity CreateEntity(string input)
        {

            var entities = Regex.Split(input, "\r\n");
            var entity = new Entity {Name = entities[0]};

            foreach (var field in entities.Skip(1))
            {
                CreateEntityField(field, entity);
            }

            return entity;
        }

        private static void CreateEntityField(string field, Entity entity)
        {
            var entityField = new EntityField {Name = field};
            entity.Fields.Add(entityField);
        }
    }
}