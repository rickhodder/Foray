using System.Collections.Generic;

namespace Foray.Common
{
    public class Entity
    {
        public string Name { get; set; }
        public List<EntityField> Fields { get; set; } = new List<EntityField>();

        public override string ToString()
        {
            return Name;
        }
    }
}