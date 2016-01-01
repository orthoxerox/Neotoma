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
            Contract.Requires<ArgumentNullException>(str != null);

            String = str;
            Index = 0;
            Line = 1;
            Column = 1;
            EOF = String.Length == 0;
        }

        private Position(
            string str, 
            int index, 
            int line, 
            int column)
        {
            String = str;
            Index = index;
            Line = line;
            Column = column;
            EOF = String.Length == Index;
        }

        public Position Advance()
        {
            if (EOF) return this;

            var line = Line;
            var column = Column + 1;
            var index = Index + 1;

            if (index != String.Length) {
                if (String[Index] == '\n') {
                    line++;
                    column = 1;
                }
                if (String[Index] == '\r' &&
                    String[index] != '\n') {
                    line++;
                    column = 1;
                }
            }
            return new Position(String, index, line, column);
        }
    }
}
