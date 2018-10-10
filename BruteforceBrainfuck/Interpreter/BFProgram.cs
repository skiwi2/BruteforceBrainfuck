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

        private BFMemory Memory { get; set; }

        public BFProgram(string programText, BFMemory memory)
        {
            RootNode = CreateProgram(programText);
            Memory = memory;
        }

        public ICollection<byte> Execute(IEnumerable<byte> input)
        {
            var output = new List<byte>();
            switch (RootNode)
            {
                case LoopNode loopNode:
                    ExecuteImpl(RootNode as LoopNode, input.GetEnumerator(), output);
                    break;
                default:
                    throw new InvalidProgramException("Expected loop node");
            }
            return output;
        }

        private void ExecuteImpl(LoopNode loopNode, IEnumerator<byte> inputEnumerator, ICollection<byte> output)
        {
            foreach (var node in loopNode.Children)
            {
                switch (node)
                {
                    case MoveRightNode moveRightNode:
                        Memory.MoveRight();
                        break;
                    case MoveLeftNode moveLeftNode:
                        Memory.MoveLeft();
                        break;
                    case IncrementNode incrementNode:
                        Memory.Increment();
                        break;
                    case DecrementNode decrementNode:
                        Memory.Decrement();
                        break;
                    case InputNode inputNode:
                        if (!inputEnumerator.MoveNext())
                        {
                            throw new InvalidProgramException("No input available anymore");
                        }
                        Memory.SetValue(inputEnumerator.Current);
                        break;
                    case OutputNode outputNode:
                        output.Add(Memory.GetValue());
                        break;
                    case LoopNode innerLoopNode:
                        while (Memory.GetValue() > 0)
                        {
                            ExecuteImpl(innerLoopNode, inputEnumerator, output);
                        }
                        break;
                    default:
                        throw new InvalidProgramException("Unexpected child node");
                }
            }
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
                        throw new InvalidProgramException("Expected more symbols after encountering a loop begin symbol");
                    default:
                        break;
                }
            }
            while (enumerator.MoveNext());
            return null;
        }
    }
}
