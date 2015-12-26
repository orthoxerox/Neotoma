using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace Neotoma
{
    public struct Position
    {
        public readonly string String;
        public readonly int Index;
        public readonly int Line;
        public readonly int Column;
        public readonly bool EOF;

        public Position(string str)
        {
            Contract.Requires(str != null);

            String = str;
            Index = 0;
            Line = 1;
            Column = 1;
            EOF = Index == 0;
        }

        public Position Advance(int step = 1)
        {
            Contract.Requires(step > 0);

            if (EOF) return this;

            var line = Line;
            var column = Column;
            var index = Index;
            var eof = EOF;

            for (int i = 1; i <= step; i++) {
                if ((index + i) == String.Length) {
                    //TODO  
                }
            }
        }
    }
}
