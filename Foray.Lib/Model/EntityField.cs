using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foray.Lib.Model
{
    public class EntityField
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool Sorted { get; set; }
    }
}
