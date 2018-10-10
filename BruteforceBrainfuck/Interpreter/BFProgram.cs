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
        private INode RootNode { get; set; }

        public BFProgram(string programText)
        {
            RootNode = CreateProgram(programText);
        }

        private static INode CreateProgram(string programText)
        {
            var enumerator = programText.GetEnumerator();
            var nodes = new List<INode>();
            while (enumerator.MoveNext())
            {
                nodes.Add(NextNode(enumerator));
            }
            return new LoopNode(nodes);
        }

        private static INode NextNode(CharEnumerator enumerator)
        {
            do
            {
                switch (enumerator.Current)
                {
                    case '>':
                        return new MoveRightNode();
                    case '<':
                        return new MoveLeftNode();
                    case '+':
                        return new IncrementNode();
                    case '-':
                        return new DecrementNode();
                    case ',':
                        return new InputNode();
                    case '.':
                        return new OutputNode();
                    case '[':
                        var nodes = new List<INode>();
                        while (enumerator.MoveNext())
                        {
                            switch (enumerator.Current)
                            {
                                case ']':
                                    return new LoopNode(nodes);
                                default:
                                    nodes.Add(NextNode(enumerator));
                                    break;
                            }
                        }
                        throw new InvalidProgramException("Expected more symbols after encoutnering a loop begin symbol");
                    default:
                        break;
                }
            }
            while (enumerator.MoveNext());
            return null;
        }
    }
}
