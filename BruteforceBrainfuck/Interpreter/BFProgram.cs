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

        private uint Executions { get; set; }

        public BFProgram(string programText, BFMemory memory)
        {
            RootNode = CreateProgram(programText);
            Memory = memory;
        }

        public ICollection<byte> Execute(IEnumerable<byte> input)
        {
            if (Executions++ > 0)
            {
                Memory.Reset();
            }
            var output = new List<byte>();
            switch (RootNode)
            {
                case LoopNode loopNode:
                    ExecuteImpl(loopNode, input.GetEnumerator(), output);
                    break;
                default:
                    throw new InvalidBFProgramException("Expected loop node");
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
                            throw new InvalidBFProgramException("No input available anymore");
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
                        throw new InvalidBFProgramException("Unexpected child node");
                }
            }
        }

        private static INode CreateProgram(string programText)
        {
            var enumerator = programText.GetEnumerator();
            var nodes = new List<INode>();
            while (enumerator.MoveNext())
            {
                if (NextNode(enumerator, out var nextNode))
                {
                    nodes.Add(nextNode);
                }
            }
            return new LoopNode(nodes);
        }

        private static bool NextNode(CharEnumerator enumerator, out INode node)
        {
            do
            {
                switch (enumerator.Current)
                {
                    case '>':
                        node = new MoveRightNode();
                        return true;
                    case '<':
                        node = new MoveLeftNode();
                        return true;
                    case '+':
                        node = new IncrementNode();
                        return true;
                    case '-':
                        node = new DecrementNode();
                        return true;
                    case ',':
                        node = new InputNode();
                        return true;
                    case '.':
                        node = new OutputNode();
                        return true;
                    case '[':
                        var nodes = new List<INode>();
                        while (enumerator.MoveNext())
                        {
                            switch (enumerator.Current)
                            {
                                case ']':
                                    node = new LoopNode(nodes);
                                    return true;
                                default:
                                    if (NextNode(enumerator, out var nextNode))
                                    {
                                        nodes.Add(nextNode);
                                    }
                                    break;
                            }
                        }
                        throw new InvalidBFProgramException("Expected more symbols after encountering a loop begin symbol");
                    default:
                        break;
                }
            }
            while (enumerator.MoveNext());
            node = null;
            return false;
        }
    }
}
