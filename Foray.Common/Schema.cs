using System.Collections.Generic;
using System.Linq;

namespace Foray.Common
{
    public class Schema
    {
        public List<Entity> Entities { get; set; } = new List<Entity>();
        public List<Relationship> Relationships { get; set; } = new List<Relationship>();

        public Entity AddEntity(string name)
        {
            var entity = FindEntity(name);
            if (entity != null)
            {
                return entity;
            }

            entity = new Entity { Name = name.ToLower().Trim() };
            Entities.Add(entity);
            return entity;
        }

        public Entity FindEntity(string name)
        {
            return Entities.FirstOrDefault(e => e.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public Relationship AddRelationship(RelationshipType reltype, string first, string second, string nested = "")
        {
            var entityFirst = AddEntity(first);
            var entitySecond = AddEntity(second);
            Entity entityNested = string.IsNullOrEmpty(nested) ? null : AddEntity(nested);

            var r = new Relationship
            {
                FirstEntity = entityFirst,
                SecondEntity = entitySecond,
                NestedEntity = entityNested,
                RelationshipType = reltype
            };

            Relationships.Add(r);

            return r;
        }
    }
}