using System.Collections.Generic;

namespace Foray.Lib.Model
{
    public class Entity
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Description { get; set; }
        public IList<EntityField> Fields { get; private set; } = new List<EntityField>();
    }

}
