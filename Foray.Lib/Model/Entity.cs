using System.Collections.Generic;

namespace Foray.Lib.Model
{
    public class Entity
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Description { get; set; }
        public List<EntityField> Fields { get; set; }

        public Entity()
        {
            Fields = new List<EntityField>();
        }
    }

}
