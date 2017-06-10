using System.Collections.Generic;

namespace Foray.Common
{
    public class RelationshipTypeMapper : Dictionary<string, RelationshipType>
    {
        public RelationshipTypeMapper()
        {
            this["-"] = RelationshipType.OneToOne;
            this[">"] = RelationshipType.OneToMany;

        }
    }
}