using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neotoma
{
    public class Grammar
    {
        private readonly Dictionary<string, Pattern> patterns = new Dictionary<string, Pattern>();

        public Pattern this[string name]
        {
            get
            {
                return patterns[name];
            }
            set
            {
                patterns[name] = value.Memoize(name);
            }
        }
    }
}
