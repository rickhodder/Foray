using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foray.Lib.Model
{
    public class Relationship
    {
        public Entity ParentEntity { get; set; }
        public Entity ChildEntity { get; set; }
        public Entity NestedEntity { get; set; }
        public RelationshipType RelationshipType { get; set; }
    }
}
