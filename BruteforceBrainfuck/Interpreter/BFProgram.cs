using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BruteforceBrainfuck.Interpreter.Nodes;

namespace BruteforceBrainfuck.Interpreter
{
    public class BFProgram
    {
        private RootNode RootNode { get; set; }

        public BFProgram(string programText)
        {
            RootNode = CreateProgram(programText);
        }

        private static RootNode CreateProgram(string programText)
        {
            var enumerator = programText.GetEnumerator();
            var nodes = new List<INode>();
            while (enumerator.MoveNext())
            {
                nodes.Add(NextNode(enumerator));
            }
            return new RootNode(nodes);
        }

        private static INode NextNode(CharEnumerator enumerator)
        {
            do
            {
                var symbol = enumerator.Current;
                switch (symbol)
                {
                    case ',':
                        return new InputNode();
                    case '.':
                        return new OutputNode();
                    default:
                        break;
                }
            }
            while (enumerator.MoveNext());
            return null;
        }
    }
}
