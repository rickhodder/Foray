using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
