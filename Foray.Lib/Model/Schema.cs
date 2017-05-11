using System.Collections.Generic;

namespace Foray.Lib.Model
{
    public class Schema
    {
        public IList<Entity> Entities { get; private set; } = new List<Entity>();
        public IList<Relationship> Relationships { get; private set; } = new List<Relationship>();

    }
}
